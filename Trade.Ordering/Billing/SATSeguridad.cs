﻿/* Empiria Customers Components 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Industries Framework                     System   : Automotive Industry Components    *
*  Namespace : Empiria.Customers.Pineda                         Assembly : Empiria.Customers.Pineda.dll      *
*  Type      : SATSeguridad                                     Pattern  : Storage Item                      *
*  Version   : 6.0        Date: 04/Jan/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Describes an order.                                                                           *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;

namespace Empiria.Trade.Billing {

  internal class SATSeguridad {

    //------- Parses binary asn.1 EncryptedPrivateKeyInfo; returns RSACryptoServiceProvider ---
    static public RSACryptoServiceProvider DecodeEncryptedPrivateKeyInfo(byte[] encpkcs8, SecureString secpswd) {
      // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
      // this byte[] includes the sequence byte and terminal encoded null
      byte[] OIDpkcs5PBES2 = { 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x05, 0x0D };
      byte[] OIDpkcs5PBKDF2 = { 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x05, 0x0C };
      byte[] OIDdesEDE3CBC = { 0x06, 0x08, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x03, 0x07 };
      byte[] seqdes = new byte[10];
      byte[] seq = new byte[11];
      byte[] salt;
      byte[] IV;
      byte[] encryptedpkcs8;
      byte[] pkcs8;

      int saltsize, ivsize, encblobsize;
      int iterations;

      // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
      MemoryStream mem = new MemoryStream(encpkcs8);
      int lenstream = (int) mem.Length;
      BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
      byte bt = 0;
      ushort twobytes = 0;

      try {

        twobytes = binr.ReadUInt16();
        if (twobytes == 0x8130)  //data read as little endian order (actual data order for Sequence is 30 81)
          binr.ReadByte();  //advance 1 byte
        else if (twobytes == 0x8230)
          binr.ReadInt16();  //advance 2 bytes
        else
          return null;

        twobytes = binr.ReadUInt16();  //inner sequence
        if (twobytes == 0x8130)
          binr.ReadByte();
        else if (twobytes == 0x8230)
          binr.ReadInt16();


        seq = binr.ReadBytes(11);    //read the Sequence OID
        if (!CompareByteArrays(seq, OIDpkcs5PBES2))  //is it a OIDpkcs5PBES2 ?
          return null;

        twobytes = binr.ReadUInt16();  //inner sequence for pswd salt
        if (twobytes == 0x8130)
          binr.ReadByte();
        else if (twobytes == 0x8230)
          binr.ReadInt16();

        twobytes = binr.ReadUInt16();  //inner sequence for pswd salt
        if (twobytes == 0x8130)
          binr.ReadByte();
        else if (twobytes == 0x8230)
          binr.ReadInt16();

        seq = binr.ReadBytes(11);    //read the Sequence OID
        if (!CompareByteArrays(seq, OIDpkcs5PBKDF2))  //is it a OIDpkcs5PBKDF2 ?
          return null;

        twobytes = binr.ReadUInt16();
        if (twobytes == 0x8130)
          binr.ReadByte();
        else if (twobytes == 0x8230)
          binr.ReadInt16();

        bt = binr.ReadByte();
        if (bt != 0x04)    //expect octet string for salt
          return null;
        saltsize = binr.ReadByte();
        salt = binr.ReadBytes(saltsize);

        bt = binr.ReadByte();
        if (bt != 0x02)   //expect an integer for PBKF2 interation count
          return null;

        int itbytes = binr.ReadByte();  //PBKD2 iterations should fit in 2 bytes.
        if (itbytes == 1)
          iterations = binr.ReadByte();
        else if (itbytes == 2)
          iterations = 256 * binr.ReadByte() + binr.ReadByte();
        else
          return null;

        twobytes = binr.ReadUInt16();
        if (twobytes == 0x8130)
          binr.ReadByte();
        else if (twobytes == 0x8230)
          binr.ReadInt16();


        seqdes = binr.ReadBytes(10);    //read the Sequence OID
        if (!CompareByteArrays(seqdes, OIDdesEDE3CBC))  //is it a OIDdes-EDE3-CBC ?
          return null;

        bt = binr.ReadByte();
        if (bt != 0x04)    //expect octet string for IV
          return null;
        ivsize = binr.ReadByte();  // IV byte size should fit in one byte (24 expected for 3DES)
        IV = binr.ReadBytes(ivsize);

        bt = binr.ReadByte();
        if (bt != 0x04)    // expect octet string for encrypted PKCS8 data
          return null;


        bt = binr.ReadByte();

        if (bt == 0x81)
          encblobsize = binr.ReadByte();  // data size in next byte
        else if (bt == 0x82)
          encblobsize = 256 * binr.ReadByte() + binr.ReadByte();
        else
          encblobsize = bt;    // we already have the data size


        encryptedpkcs8 = binr.ReadBytes(encblobsize);
        //if(verbose)
        //  showBytes("Encrypted PKCS8 blob", encryptedpkcs8) ;


        //SecureString  = GetSecPswd();

        pkcs8 = DecryptPBDK2(encryptedpkcs8, salt, IV, secpswd, iterations);
        if (pkcs8 == null)  // probably a bad pswd entered.
          return null;

        //if(verbose)
        //  showBytes("Decrypted PKCS #8", pkcs8) ;
        //----- With a decrypted pkcs #8 PrivateKeyInfo blob, decode it to an RSA ---
        RSACryptoServiceProvider rsa = DecodePrivateKeyInfo(pkcs8);
        return rsa;
      } catch (Exception e) {
        Empiria.Messaging.Publisher.Publish(e);
        return null;
      } finally { binr.Close(); }

    }

    static private bool CompareByteArrays(byte[] a, byte[] b) {
      if (a.Length != b.Length)
        return false;
      int i = 0;
      foreach (byte c in a) {
        if (c != b[i])
          return false;
        i++;
      }
      return true;
    }

    //  ------  Uses PBKD2 to derive a 3DES key and decrypts data --------
    static public byte[] DecryptPBDK2(byte[] edata, byte[] salt, byte[] IV, SecureString secpswd, int iterations) {
      CryptoStream decrypt = null;

      IntPtr unmanagedPswd = IntPtr.Zero;
      byte[] psbytes = new byte[secpswd.Length];
      unmanagedPswd = Marshal.SecureStringToGlobalAllocAnsi(secpswd);
      Marshal.Copy(unmanagedPswd, psbytes, 0, psbytes.Length);
      Marshal.ZeroFreeGlobalAllocAnsi(unmanagedPswd);

      try {
        Rfc2898DeriveBytes kd = new Rfc2898DeriveBytes(psbytes, salt, iterations);
        TripleDES decAlg = TripleDES.Create();
        decAlg.Key = kd.GetBytes(24);
        decAlg.IV = IV;
        MemoryStream memstr = new MemoryStream();
        decrypt = new CryptoStream(memstr, decAlg.CreateDecryptor(), CryptoStreamMode.Write);
        decrypt.Write(edata, 0, edata.Length);
        decrypt.Flush();
        decrypt.Close();  // this is REQUIRED.
        byte[] cleartext = memstr.ToArray();
        return cleartext;
      } catch (Exception e) {
        throw new Exception("Problem decrypting: " + e.Message);
      }
    }

    //------- Parses binary asn.1 PKCS #8 PrivateKeyInfo; returns RSACryptoServiceProvider ---
    static public RSACryptoServiceProvider DecodePrivateKeyInfo(byte[] pkcs8) {
      // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
      // this byte[] includes the sequence byte and terminal encoded null
      byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
      byte[] seq = new byte[15];
      // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
      MemoryStream mem = new MemoryStream(pkcs8);
      int lenstream = (int) mem.Length;
      BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
      byte bt = 0;
      ushort twobytes = 0;

      try {

        twobytes = binr.ReadUInt16();
        if (twobytes == 0x8130)  //data read as little endian order (actual data order for Sequence is 30 81)
          binr.ReadByte();  //advance 1 byte
        else if (twobytes == 0x8230)
          binr.ReadInt16();  //advance 2 bytes
        else
          return null;


        bt = binr.ReadByte();
        if (bt != 0x02)
          return null;

        twobytes = binr.ReadUInt16();

        if (twobytes != 0x0001)
          return null;

        seq = binr.ReadBytes(15);    //read the Sequence OID
        if (!CompareByteArrays(seq, SeqOID))  //make sure Sequence for OID is correct
          return null;

        bt = binr.ReadByte();
        if (bt != 0x04)  //expect an Octet string
          return null;

        bt = binr.ReadByte();    //read next byte, or next 2 bytes is  0x81 or 0x82; otherwise bt is the byte count
        if (bt == 0x81)
          binr.ReadByte();
        else
          if (bt == 0x82)
            binr.ReadUInt16();
        //------ at this stage, the remaining sequence should be the RSA private key

        byte[] rsaprivkey = binr.ReadBytes((int) (lenstream - mem.Position));
        RSACryptoServiceProvider rsacsp = DecodeRSAPrivateKey(rsaprivkey);
        return rsacsp;
      } catch (Exception) {
        return null;
      } finally { binr.Close(); }

    }

    //------- Parses binary ans.1 RSA private key; returns RSACryptoServiceProvider  ---
    static public RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey) {
      byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

      // ---------  Set up stream to decode the asn.1 encoded RSA private key  ------
      MemoryStream mem = new MemoryStream(privkey);
      BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
      byte bt = 0;
      ushort twobytes = 0;
      int elems = 0;
      try {
        twobytes = binr.ReadUInt16();
        if (twobytes == 0x8130)  //data read as little endian order (actual data order for Sequence is 30 81)
          binr.ReadByte();  //advance 1 byte
        else if (twobytes == 0x8230)
          binr.ReadInt16();  //advance 2 bytes
        else
          return null;

        twobytes = binr.ReadUInt16();
        if (twobytes != 0x0102)  //version number
          return null;
        bt = binr.ReadByte();
        if (bt != 0x00)
          return null;


        //------  all private key components are Integer sequences ----
        elems = GetIntegerSize(binr);
        MODULUS = binr.ReadBytes(elems);

        elems = GetIntegerSize(binr);
        E = binr.ReadBytes(elems);

        elems = GetIntegerSize(binr);
        D = binr.ReadBytes(elems);

        elems = GetIntegerSize(binr);
        P = binr.ReadBytes(elems);

        elems = GetIntegerSize(binr);
        Q = binr.ReadBytes(elems);

        elems = GetIntegerSize(binr);
        DP = binr.ReadBytes(elems);

        elems = GetIntegerSize(binr);
        DQ = binr.ReadBytes(elems);

        elems = GetIntegerSize(binr);
        IQ = binr.ReadBytes(elems);

        // ------- create RSACryptoServiceProvider instance and initialize with public key -----
        RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
        RSAParameters RSAparams = new RSAParameters();
        RSAparams.Modulus = MODULUS;
        RSAparams.Exponent = E;
        RSAparams.D = D;
        RSAparams.P = P;
        RSAparams.Q = Q;
        RSAparams.DP = DP;
        RSAparams.DQ = DQ;
        RSAparams.InverseQ = IQ;
        RSA.ImportParameters(RSAparams);
        return RSA;
      } catch (Exception) {
        return null;
      } finally { binr.Close(); }
    }

    static private int GetIntegerSize(BinaryReader binr) {
      byte bt = 0;
      byte lowbyte = 0x00;
      byte highbyte = 0x00;
      int count = 0;
      bt = binr.ReadByte();
      if (bt != 0x02)    //expect integer
        return 0;
      bt = binr.ReadByte();

      if (bt == 0x81)
        count = binr.ReadByte();  // data size in next byte
      else
        if (bt == 0x82) {
          highbyte = binr.ReadByte();  // data size in next 2 bytes
          lowbyte = binr.ReadByte();
          byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
          count = BitConverter.ToInt32(modint, 0);
        } else {
          count = bt;    // we already have the data size
        }

      while (binr.ReadByte() == 0x00) {  //remove high order zeros in data
        count -= 1;
      }
      binr.BaseStream.Seek(-1, SeekOrigin.Current);    //last ReadByte wasn't a removed zero, so back up a byte
      return count;
    }

  } // class SATSeguridad

} // namespace Empiria.Trade.Billing
