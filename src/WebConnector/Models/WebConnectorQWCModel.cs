using System;

namespace QbSync.WebConnector.Models
{
    /// <summary>
    /// AuthFlag.
    /// </summary>
    [Flags]
    public enum AuthFlag
    {
        /// <summary>
        /// All.
        /// </summary>
        All = 0,

        /// <summary>
        /// SupportQBSimpleStart.
        /// </summary>
        SupportQBSimpleStart = 1,

        /// <summary>
        /// SupportQBPro.
        /// </summary>
        SupportQBPro = 2,

        /// <summary>
        /// SupportQBPremier.
        /// </summary>
        SupportQBPremier = 4,

        /// <summary>
        /// SupportQBEnterprise.
        /// </summary>
        SupportQBEnterprise = 8
    }

    /// <summary>
    /// PersonalDataPref.
    /// </summary>
    public enum PersonalDataPref
    {
        /// <summary>
        /// PdpOptional.
        /// </summary>
        PdpOptional,

        /// <summary>
        /// PdpRequired.
        /// </summary>
        PdpRequired
    }

    /// <summary>
    /// QBType.
    /// </summary>
    public enum QBType
    {
        /// <summary>
        /// QBFS.
        /// </summary>
        QBFS,

        /// <summary>
        /// QBPOS.
        /// </summary>
        QBPOS
    }

    /// <summary>
    /// UnattendedModePref.
    /// </summary>
    public enum UnattendedModePref
    {
        /// <summary>
        /// UmpOptional.
        /// </summary>
        UmpOptional,

        /// <summary>
        /// UmpRequired.
        /// </summary>
        UmpRequired
    }

    /// <summary>
    /// Models holding the information on how to generate the QWC file.
    /// </summary>
    public class WebConnectorQwcModel
    {
        /// <summary>
        /// Creates the WebConnectorQwcModel.
        /// To get more information about each parameter, look at each property.
        /// </summary>
        /// <param name="appName">The name of the application visible to the user.</param>
        /// <param name="appURL">The URL of your web service.</param>
        /// <param name="appDescription">This brief description of the application.</param>
        /// <param name="appSupport">The URL where your user can go to get support for your application.</param>
        /// <param name="userName">The name your user must use to access your web service.</param>
        /// <param name="ownerID">This is a GUID that represents your application or suite of applications.</param>
        /// <param name="fileID">The Web Connector stores this as an extension to the company record with a specific OwnerID known only to your web service.</param>
        /// <param name="qbType">Specify the value QBFS if your web service is designed for use with QuickBooks Financial software.</param>
        /// <param name="appID">The AppID of the application.</param>
        /// <param name="runEvery">Your end user can specify the update interval in the QB web connector UI. Be aware that the user can override your settings in the UI.</param>
        /// <param name="appDisplayName">QBWC will use this to display name in the QBWC UI.</param>
        /// <param name="appUniqueName">If this element is available in QWC file, QBWC will not go into it's typical clone/replace mode for AppName and directly use the replace routine.</param>
        /// <param name="authFlags">This element is used only for QuickBooks (QBType=QBFS)</param>
        /// <param name="isReadOnly">Used to inform QBXMLRP2 whether your service is reading data only, or is also writing data to the company</param>
        /// <param name="notify">Value of true will enable notification (pop up at systray) at app level.</param>
        /// <param name="personalDataPrep">Used to inform QBXMLRP2 (request processor) whether your service requires access to personal/sensitive data.</param>
        /// <param name="unattendedModePref">Used to inform QBXMLRP2 (request processor) whether your service needs permissions to run in Unattended Mode supply the value umpRequired if it does, or umpOptional if it does not.</param>
        public WebConnectorQwcModel(
            string appName,
            string appURL,
            string appDescription,
            string appSupport,
            string userName,
            Guid ownerID,
            Guid fileID,
            QBType qbType,
            string? appID = default,
            TimeSpan? runEvery = default,
            string? appDisplayName = default,
            string? appUniqueName = default,
            AuthFlag? authFlags = default,
            bool isReadOnly = default,
            bool notify = default,
            PersonalDataPref? personalDataPrep = default,
            UnattendedModePref? unattendedModePref = default
        )
        {
            AppName = appName;
            AppURL = appURL;
            AppDescription = appDescription;
            AppSupport = appSupport;
            UserName = userName;
            OwnerID = ownerID;
            FileID = fileID;
            QBType = qbType;
            AppID = appID;
            RunEvery = runEvery;
            AppDisplayName = appDisplayName;
            AppUniqueName = appUniqueName;
            AuthFlags = authFlags;
            IsReadOnly = isReadOnly;
            Notify = notify;
            PersonalDataPrep = personalDataPrep;
            UnattendedModePref = unattendedModePref;
        }

        /// <summary>
        /// <para>Required.</para>
        /// <para>The name of the application visible to the user. This name is
        /// displayed in the QB web connector. It is also the name supplied in
        /// the SDK OpenConnection call to QuickBooks or QuickBooks POS.</para>
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// <para>Optional.</para>
        /// <para>The AppID of the application, supplied in the call to OpenConnection.
        /// Currently QB and QB POS don't do much with the AppID, so you can
        /// supply the tag without supplying any value for it. However, you can
        /// supply an AppID value here if you want.</para>
        /// </summary>
        public string? AppID { get; set; }

        /// <summary>
        /// <para>Required.</para>
        /// <para>The URL of your web service. The domain name used in the
        /// AppSupport URL must match the domain name used in the AppURL.</para>
        /// <para>For internal development and testing only, you can specify localhost
        /// or a machine name in place of the domain name. If you specify a
        /// machine name, the machine name used for AppSupport must match
        /// the machine name used for AppURL.</para>
        /// <para>Unless you are using localhost (for development and testing only!)
        /// the domain name specified in AppSupport and in AppURL must
        /// match.</para>
        /// <para>To maintain a secure exchange of financial data, your AppURL must
        /// use the HTTP protocol over SSL(https://...). If you don't use HTTPS,
        /// the web connector won't connect with your web service.</para>
        /// </summary>
        public string AppURL { get; set; }

        /// <summary>
        /// <para>Required.</para>
        /// <para>This brief description of the application is displayed in the QB web
        /// connector(in the authorization dialog and in the main QBWC form
        /// under the application name. For best results we recommend a
        /// maximum description size of 80 characters.</para>
        /// </summary>
        public string AppDescription { get; set; }

        /// <summary>
        /// <para>Required.</para>
        /// <para>The URL where your user can go to get support for your application.
        /// (Do not specify an Intuit site!)</para>
        /// <para>The domain name used in the AppSupport URL must match the
        /// domain name used in the AppURL.</para>
        /// <para>For internal development and testing only, you can specify localhost
        /// or a machine name in place of the domain name. If you specify a
        /// machine name, the machine name used for AppSupport must match
        /// the machine name used for AppURL.</para>
        /// </summary>
        public string AppSupport { get; set; }

        /// <summary>
        /// <para>Required.</para>
        /// <para>The name your user must use to access your web service. The web
        /// connector uses this name when it invokes the authenticate call on
        /// your web service.</para>
        /// <para>To avoid disclosure of the password, there is no provision for any
        /// password field in the QWC file. Your user must enter the password
        /// into the web connector themselves, where it can be stored in the
        /// Windows registry securely via encryption.</para>
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// <para>Required.</para>
        /// <para>This is a GUID that represents your application or suite of
        /// applications, if your application needs to store private data in the
        /// company file for one reason or another. One of the most common
        /// uses is to check (via a QuickBooks SDK CompanyQuery request)
        /// whether you have communicated with this company file before, and
        /// possibly some data about that communication.</para>
        /// <para>You should generate one GUID per application only and not per
        /// application version or per QWC file!</para>
        /// <para>This private data will be visible to any application that knows the
        /// OwnerID.</para>
        /// </summary>
        public Guid OwnerID { get; set; }

        /// <summary>
        /// <para>Required.</para>
        /// <para>The Web Connector stores this as an extension to the
        /// company record with a specific OwnerID known only to your web
        /// service (see below in the table for information on OwnerID) the first
        /// time the Web Connector connects to the company. The point is to
        /// ensure that your web service knows it is talking to the file it has
        /// always talked to. The OwnerID value is specific to only your web
        /// service.</para>
        /// </summary>
        public Guid FileID { get; set; }

        /// <summary>
        /// <para>Optional.</para>
        /// <para>Your end user can specify the update interval in the QB web
        /// connector UI. Be aware that the user
        /// can override your settings in the UI.</para>
        /// </summary>
        public TimeSpan? RunEvery { get; set; }

        /// <summary>
        /// <para>Optional.</para>
        /// <para>QBWC will use this to display name in the QBWC UI. Otherwise, use
        /// <see cref="AppName"/> as usual. This is just for UI purpose. Update process
        /// still uses the <see cref="AppName"/> (or, AppUniqueName if provided).</para>
        /// </summary>
        public string? AppDisplayName { get; set; }

        /// <summary>
        /// <para>Optional.</para>
        /// <para>If this element is available in QWC file, QBWC will not go into it's
        /// typical clone/replace mode for AppName and directly use the replace
        /// routine.</para>
        /// </summary>
        public string? AppUniqueName { get; set; }

        /// <summary>
        /// <para>Optional.</para>
        /// <para>This element is used only for QuickBooks (QBType=QBFS). It
        /// specifies which QuickBooks editions are supported by your web
        /// service. By default, all editions are supported, including Simple Start
        /// edition.</para>
        /// <para>If you set this to exclude some QuickBooks edition, and QBWC
        /// attempts to connect to that excluded edition, there will be a
        /// connection error.</para>
        /// </summary>
        public AuthFlag? AuthFlags { get; set; }

        /// <summary>
        /// <para>Optional.</para>
        /// <para>Used to inform QBXMLRP2 (request processor) whether your service
        /// is reading data only, or is also writing data to the company. Specify
        /// true if write access is needed, or false if not.</para>
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// <para>Optional.</para>
        /// <para>Value of true will enable notification (pop up at systray) at app level.
        /// Anything else will disable notification.</para>
        /// </summary>
        public bool Notify { get; set; }

        /// <summary>
        /// <para>Optional.</para>
        /// <para>Used to inform QBXMLRP2 (request processor) whether your service
        /// requires access to personal/sensitive data.</para>
        /// </summary>
        public PersonalDataPref? PersonalDataPrep { get; set; }

        /// <summary>
        /// <para>Required.</para>
        /// <para>Specify the value QBFS if your web service is designed for use with
        /// QuickBooks Financial software.</para>
        /// <para>Specify the value QBPOS if your web service is designed for use with
        /// QuickBooks Point-of-Sale(QBPOS).</para>
        /// </summary>
        public QBType QBType { get; set; }

        /// <summary>
        /// <para>Optional.</para>
        /// <para>Used to inform QBXMLRP2 (request processor) whether your service
        /// needs permissions to run in Unattended Mode supply the value
        /// umpRequired if it does, or umpOptional if it does not.</para>
        /// </summary>
        public UnattendedModePref? UnattendedModePref { get; set; }
    }
}
