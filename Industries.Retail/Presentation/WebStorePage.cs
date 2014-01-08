/* Empiria® Industries Framework 2014 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Industries Framework 2014               System   : Retail Industry Components        *
*  Namespace : Empiria.Industries.Retail.Presentation           Assembly : Empiria.Industries.Retail.dll     *
*  Type      : WebStorePage                                     Pattern  : Model View Controller             *
*  Date      : 28/Mar/2014                                      Version  : 5.5     License: CC BY-NC-SA 4.0  *
*                                                                                                            *
*  Summary   : Abstract type that represents a web page for online stores.                                   *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2014. **/
using System;

using Empiria.Presentation.Web;
using Empiria.Presentation.Web.Controllers;

namespace Empiria.Industries.Retail.Presentation {

  /// <summary>Abstract type that represents a web page for online stores.</summary>
  public abstract class WebStorePage : WebPage {

    #region Abstract members

    protected abstract bool ExecutePageCommand();
    protected abstract void Initialize();
    protected abstract void LoadPageControls();

    #endregion Abstract members

    #region Fields

    private string title = String.Empty;
    private string workareaTitle = String.Empty;
    private string workareaHint = String.Empty;

    #endregion Fields

    #region Protected properties

    protected string AppPath {
      get {
        return (Request.ApplicationPath == "/") ? String.Empty : Request.ApplicationPath;
      }
    }

    //protected string CommandName {
    //  get { return commandName; }
    //}

    //protected NameValueCollection CommandArguments {
    //  get { return commandArguments; }
    //}

    protected new WebStoreMasterPage Master {
      get { return (WebStoreMasterPage) base.Master; }
    }

    protected new string Title {
      get { return title; }
      set {
        title = EmpiriaString.TrimAll(value);
        title = ExecutionServer.CustomerName + ((title.Length != 0) ? " | " + title : String.Empty);
        Page.Title = title;
      }
    }

    protected WebStoreSession WebStoreSession {
      get { return Master.WebStoreSession; }
    }

    protected string WorkareaHint {
      get { return workareaHint; }
      set { workareaHint = value; }
    }

    protected string WorkareaTitle {
      get { return workareaTitle; }
      set { workareaTitle = value; }
    }

    #endregion Protected properties

    #region Protected methods

    //protected string GetCommandArgument(string argumentName) {
    //  return commandArguments[argumentName];
    //}

    protected void Page_Load(object sender, EventArgs e) {
      Initialize();
      if (IsPostBack) {
        ProcessCommand();
        LoadPageControls();
      } else {
        LoadPageControls();
      }
    }

    protected override void OnLoadComplete(EventArgs e) {
      base.OnLoadComplete(e);
    }

    protected override void OnPreLoad(EventArgs e) {
      CreateGuestSessionIfUnauthenticated();
      base.OnPreLoad(e);
      LoadControlFields();
      this.Title = String.Empty;
    }

    private void LoadControlFields() {

    }

    #endregion Protected methods

    #region Private methods

    private void ProcessCommand() {
      if (ExecutePageCommand()) {
        return;
      }
      switch (this.CommandName) {
        case "SearchProducts":
          WebStoreSession.PushProductSearch(base.CommandParameters["query"]);
          Response.Redirect(this.AppPath + "/products/search.products.aspx", true);
          return;
        case "moveFirst":
        case "movePrevious":
        case "moveNext":
        case "moveLast":
          //ExecuteDataSourceCommand(GetCachedDataSource());
          return;
        default:
          throw new WebPresentationException(WebPresentationException.Msg.UnrecognizedCommandName, this.CommandName);
      }
    }

    private void CreateGuestSessionIfUnauthenticated() {
      if (!IsSessionAlive) {
        GuestLogonController guestLogon = new GuestLogonController();
        if (!guestLogon.Logon()) {
          throw new Security.SecurityException(Empiria.Security.SecurityException.Msg.WrongAuthentication);
        }
      }
    }

    #endregion Private methods

  } // class WebStorePage

} // namespace Empiria.Industries.Retail.Presentation