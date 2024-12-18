





using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  public class AutoJTServiceClient : ClientBase<IAutoJTService>, IAutoJTService
  {
    public AutoJTServiceClient()
    {
    }

    public AutoJTServiceClient(string endpointConfigurationName)
      : base(endpointConfigurationName)
    {
    }

    public AutoJTServiceClient(string endpointConfigurationName, string remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public AutoJTServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public AutoJTServiceClient(Binding binding, EndpointAddress remoteAddress)
      : base(binding, remoteAddress)
    {
    }

    public string GetCPPRuntimeUrl() => this.Channel.GetCPPRuntimeUrl();

    public Task<string> GetCPPRuntimeUrlAsync() => this.Channel.GetCPPRuntimeUrlAsync();

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    GetUnionIDNickName4ClientInfosResponse IAutoJTService.GetUnionIDNickName4ClientInfos(
      GetUnionIDNickName4ClientInfosRequest request)
    {
      return this.Channel.GetUnionIDNickName4ClientInfos(request);
    }

    public bool GetUnionIDNickName4ClientInfos(
      bool isInternal,
      string category,
      string userlnfos,
      string clientInfos,
      string clientVersion,
      string softHostVersion,
      string ipAddress,
      out string internal_tag,
      out string expiryDate,
      out string unionId,
      out string nickName,
      out string regId,
      out string version_desc,
      out string[] moduleIDList)
    {
      GetUnionIDNickName4ClientInfosResponse name4ClientInfos = ((IAutoJTService) this).GetUnionIDNickName4ClientInfos(new GetUnionIDNickName4ClientInfosRequest()
      {
        isInternal = isInternal,
        category = category,
        userlnfos = userlnfos,
        clientInfos = clientInfos,
        clientVersion = clientVersion,
        softHostVersion = softHostVersion,
        ipAddress = ipAddress
      });
      internal_tag = name4ClientInfos.internal_tag;
      expiryDate = name4ClientInfos.expiryDate;
      unionId = name4ClientInfos.unionId;
      nickName = name4ClientInfos.nickName;
      regId = name4ClientInfos.regId;
      version_desc = name4ClientInfos.version_desc;
      moduleIDList = name4ClientInfos.moduleIDList;
      return name4ClientInfos.GetUnionIDNickName4ClientInfosResult;
    }

    public Task<GetUnionIDNickName4ClientInfosResponse> GetUnionIDNickName4ClientInfosAsync(
      GetUnionIDNickName4ClientInfosRequest request)
    {
      return this.Channel.GetUnionIDNickName4ClientInfosAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    UserAuthenticationResponse IAutoJTService.UserAuthentication(UserAuthenticationRequest request)
    {
      return this.Channel.UserAuthentication(request);
    }

    public bool UserAuthentication(
      string unionid,
      string nickName,
      bool isInternal,
      string category,
      string userlnfos,
      string clientVersion,
      string softHostVersion,
      string ipAddress,
      out string internal_tag,
      out string expiryDate,
      out string version_desc,
      out string[] moduleIDList,
      out string regid,
      out string nickNameResout)
    {
      UserAuthenticationResponse authenticationResponse = ((IAutoJTService) this).UserAuthentication(new UserAuthenticationRequest()
      {
        unionid = unionid,
        nickName = nickName,
        isInternal = isInternal,
        category = category,
        userlnfos = userlnfos,
        clientVersion = clientVersion,
        softHostVersion = softHostVersion,
        ipAddress = ipAddress
      });
      internal_tag = authenticationResponse.internal_tag;
      expiryDate = authenticationResponse.expiryDate;
      version_desc = authenticationResponse.version_desc;
      moduleIDList = authenticationResponse.moduleIDList;
      regid = authenticationResponse.regid;
      nickNameResout = authenticationResponse.nickNameResout;
      return authenticationResponse.UserAuthenticationResult;
    }

    public Task<UserAuthenticationResponse> UserAuthenticationAsync(
      UserAuthenticationRequest request)
    {
      return this.Channel.UserAuthenticationAsync(request);
    }

    public bool UpgradeNickname(string uuid, string newNickname, bool isInternal)
    {
      return this.Channel.UpgradeNickname(uuid, newNickname, isInternal);
    }

    public Task<bool> UpgradeNicknameAsync(string uuid, string newNickname, bool isInternal)
    {
      return this.Channel.UpgradeNicknameAsync(uuid, newNickname, isInternal);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    UpdateUserPaymentDataResponse IAutoJTService.UpdateUserPaymentData(
      UpdateUserPaymentDataRequest request)
    {
      return this.Channel.UpdateUserPaymentData(request);
    }

    public bool UpdateUserPaymentData(
      string unionID,
      string order_number,
      string amount,
      string combo_meal_code,
      string category,
      out string expiry_date)
    {
      UpdateUserPaymentDataResponse paymentDataResponse = ((IAutoJTService) this).UpdateUserPaymentData(new UpdateUserPaymentDataRequest()
      {
        unionID = unionID,
        order_number = order_number,
        amount = amount,
        combo_meal_code = combo_meal_code,
        category = category
      });
      expiry_date = paymentDataResponse.expiry_date;
      return paymentDataResponse.UpdateUserPaymentDataResult;
    }

    public Task<UpdateUserPaymentDataResponse> UpdateUserPaymentDataAsync(
      UpdateUserPaymentDataRequest request)
    {
      return this.Channel.UpdateUserPaymentDataAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    QueryAllComboMealInfoResponse IAutoJTService.QueryAllComboMealInfo(
      QueryAllComboMealInfoRequest request)
    {
      return this.Channel.QueryAllComboMealInfo(request);
    }

    public string QueryAllComboMealInfo(string unionID, out string combo_dt, out string userInfos)
    {
      QueryAllComboMealInfoResponse mealInfoResponse = ((IAutoJTService) this).QueryAllComboMealInfo(new QueryAllComboMealInfoRequest()
      {
        unionID = unionID
      });
      combo_dt = mealInfoResponse.combo_dt;
      userInfos = mealInfoResponse.userInfos;
      return mealInfoResponse.QueryAllComboMealInfoResult;
    }

    public Task<QueryAllComboMealInfoResponse> QueryAllComboMealInfoAsync(
      QueryAllComboMealInfoRequest request)
    {
      return this.Channel.QueryAllComboMealInfoAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    CheckUserOwnerVersionResponse IAutoJTService.CheckUserOwnerVersion(
      CheckUserOwnerVersionRequest request)
    {
      return this.Channel.CheckUserOwnerVersion(request);
    }

    public bool CheckUserOwnerVersion(string unionID, string combo_meal_code, out int? ouwnerVer)
    {
      CheckUserOwnerVersionResponse ownerVersionResponse = ((IAutoJTService) this).CheckUserOwnerVersion(new CheckUserOwnerVersionRequest()
      {
        unionID = unionID,
        combo_meal_code = combo_meal_code
      });
      ouwnerVer = ownerVersionResponse.ouwnerVer;
      return ownerVersionResponse.CheckUserOwnerVersionResult;
    }

    public Task<CheckUserOwnerVersionResponse> CheckUserOwnerVersionAsync(
      CheckUserOwnerVersionRequest request)
    {
      return this.Channel.CheckUserOwnerVersionAsync(request);
    }

    public string GetUserAllEnableVersion(string uuid)
    {
      return this.Channel.GetUserAllEnableVersion(uuid);
    }

    public Task<string> GetUserAllEnableVersionAsync(string uuid)
    {
      return this.Channel.GetUserAllEnableVersionAsync(uuid);
    }

    public string GetData(int value) => this.Channel.GetData(value);

    public Task<string> GetDataAsync(int value) => this.Channel.GetDataAsync(value);

    public CompositeType GetDataUsingDataContract(CompositeType composite)
    {
      return this.Channel.GetDataUsingDataContract(composite);
    }

    public Task<CompositeType> GetDataUsingDataContractAsync(CompositeType composite)
    {
      return this.Channel.GetDataUsingDataContractAsync(composite);
    }

    public bool GetCurrentClientLoginStatus(string userName, string clientInfos, string openID)
    {
      return this.Channel.GetCurrentClientLoginStatus(userName, clientInfos, openID);
    }

    public Task<bool> GetCurrentClientLoginStatusAsync(
      string userName,
      string clientInfos,
      string openID)
    {
      return this.Channel.GetCurrentClientLoginStatusAsync(userName, clientInfos, openID);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    GetCurrentMachineLoginCodeResponse IAutoJTService.GetCurrentMachineLoginCode(
      GetCurrentMachineLoginCodeRequest request)
    {
      return this.Channel.GetCurrentMachineLoginCode(request);
    }

    public string GetCurrentMachineLoginCode(string clientInfos, out string openid)
    {
      GetCurrentMachineLoginCodeResponse machineLoginCode = ((IAutoJTService) this).GetCurrentMachineLoginCode(new GetCurrentMachineLoginCodeRequest()
      {
        clientInfos = clientInfos
      });
      openid = machineLoginCode.openid;
      return machineLoginCode.GetCurrentMachineLoginCodeResult;
    }

    public Task<GetCurrentMachineLoginCodeResponse> GetCurrentMachineLoginCodeAsync(
      GetCurrentMachineLoginCodeRequest request)
    {
      return this.Channel.GetCurrentMachineLoginCodeAsync(request);
    }

    public string GetLoginPopular() => this.Channel.GetLoginPopular();

    public Task<string> GetLoginPopularAsync() => this.Channel.GetLoginPopularAsync();

    public string QueryVersionDesc4ModuleID(string moduleID)
    {
      return this.Channel.QueryVersionDesc4ModuleID(moduleID);
    }

    public Task<string> QueryVersionDesc4ModuleIDAsync(string moduleID)
    {
      return this.Channel.QueryVersionDesc4ModuleIDAsync(moduleID);
    }

    public bool ValidateAuth(string txtCode) => this.Channel.ValidateAuth(txtCode);

    public Task<bool> ValidateAuthAsync(string txtCode) => this.Channel.ValidateAuthAsync(txtCode);

    public int ExitLoginMethod(string uuid) => this.Channel.ExitLoginMethod(uuid);

    public Task<int> ExitLoginMethodAsync(string uuid) => this.Channel.ExitLoginMethodAsync(uuid);

    public DataTable GetTableFromDatabase(string dataBaseTable)
    {
      return this.Channel.GetTableFromDatabase(dataBaseTable);
    }

    public Task<DataTable> GetTableFromDatabaseAsync(string dataBaseTable)
    {
      return this.Channel.GetTableFromDatabaseAsync(dataBaseTable);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    GetUserFromDatabaseResponse IAutoJTService.GetUserFromDatabase(
      GetUserFromDatabaseRequest request)
    {
      return this.Channel.GetUserFromDatabase(request);
    }

    public DataSet GetUserFromDatabase(
      string dataBaseTable,
      string pass,
      string username,
      out string resultstatus,
      out bool resultstatus2,
      out int resultstatus3)
    {
      GetUserFromDatabaseResponse userFromDatabase = ((IAutoJTService) this).GetUserFromDatabase(new GetUserFromDatabaseRequest()
      {
        dataBaseTable = dataBaseTable,
        pass = pass,
        username = username
      });
      resultstatus = userFromDatabase.resultstatus;
      resultstatus2 = userFromDatabase.resultstatus2;
      resultstatus3 = userFromDatabase.resultstatus3;
      return userFromDatabase.GetUserFromDatabaseResult;
    }

    public Task<GetUserFromDatabaseResponse> GetUserFromDatabaseAsync(
      GetUserFromDatabaseRequest request)
    {
      return this.Channel.GetUserFromDatabaseAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    GetUserFromDatabase2Response IAutoJTService.GetUserFromDatabase2(
      GetUserFromDatabase2Request request)
    {
      return this.Channel.GetUserFromDatabase2(request);
    }

    public DataSet GetUserFromDatabase2(
      string category,
      string userlnfos,
      string clientVersion,
      string dataBaseTable,
      string pass,
      string username,
      out string resultstatus,
      out bool resultstatus2,
      out int resultstatus3)
    {
      GetUserFromDatabase2Response userFromDatabase2 = ((IAutoJTService) this).GetUserFromDatabase2(new GetUserFromDatabase2Request()
      {
        category = category,
        userlnfos = userlnfos,
        clientVersion = clientVersion,
        dataBaseTable = dataBaseTable,
        pass = pass,
        username = username
      });
      resultstatus = userFromDatabase2.resultstatus;
      resultstatus2 = userFromDatabase2.resultstatus2;
      resultstatus3 = userFromDatabase2.resultstatus3;
      return userFromDatabase2.GetUserFromDatabase2Result;
    }

    public Task<GetUserFromDatabase2Response> GetUserFromDatabase2Async(
      GetUserFromDatabase2Request request)
    {
      return this.Channel.GetUserFromDatabase2Async(request);
    }

    public void GetUserFromDatabase_wechat(
      string category,
      string userlnfos,
      string clientVersion,
      string username)
    {
      this.Channel.GetUserFromDatabase_wechat(category, userlnfos, clientVersion, username);
    }

    public Task GetUserFromDatabase_wechatAsync(
      string category,
      string userlnfos,
      string clientVersion,
      string username)
    {
      return this.Channel.GetUserFromDatabase_wechatAsync(category, userlnfos, clientVersion, username);
    }

    public void GetUserFromDatabase_wechat2(
      string category,
      string userlnfos,
      string clientVersion,
      string username,
      string engineeringSoftInfos)
    {
      this.Channel.GetUserFromDatabase_wechat2(category, userlnfos, clientVersion, username, engineeringSoftInfos);
    }

    public Task GetUserFromDatabase_wechat2Async(
      string category,
      string userlnfos,
      string clientVersion,
      string username,
      string engineeringSoftInfos)
    {
      return this.Channel.GetUserFromDatabase_wechat2Async(category, userlnfos, clientVersion, username, engineeringSoftInfos);
    }

    public string DeleteItemData(string dataBaseTable, string item)
    {
      return this.Channel.DeleteItemData(dataBaseTable, item);
    }

    public Task<string> DeleteItemDataAsync(string dataBaseTable, string item)
    {
      return this.Channel.DeleteItemDataAsync(dataBaseTable, item);
    }

    public string DeleteItemData2(string dataBaseTable, string item)
    {
      return this.Channel.DeleteItemData2(dataBaseTable, item);
    }

    public Task<string> DeleteItemData2Async(string dataBaseTable, string item)
    {
      return this.Channel.DeleteItemData2Async(dataBaseTable, item);
    }

    public string InsertByDataTable(DataTable dataTable)
    {
      return this.Channel.InsertByDataTable(dataTable);
    }

    public Task<string> InsertByDataTableAsync(DataTable dataTable)
    {
      return this.Channel.InsertByDataTableAsync(dataTable);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    ImportTableToDB1Response IAutoJTService.ImportTableToDB1(ImportTableToDB1Request request)
    {
      return this.Channel.ImportTableToDB1(request);
    }

    public bool ImportTableToDB1(
      DataTable dtImp,
      string SqlTableName,
      out string err,
      out string info)
    {
      ImportTableToDB1Response db1 = ((IAutoJTService) this).ImportTableToDB1(new ImportTableToDB1Request()
      {
        dtImp = dtImp,
        SqlTableName = SqlTableName
      });
      err = db1.err;
      info = db1.info;
      return db1.ImportTableToDB1Result;
    }

    public Task<ImportTableToDB1Response> ImportTableToDB1Async(ImportTableToDB1Request request)
    {
      return this.Channel.ImportTableToDB1Async(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    ImportTableToDB2Response IAutoJTService.ImportTableToDB2(ImportTableToDB2Request request)
    {
      return this.Channel.ImportTableToDB2(request);
    }

    public bool ImportTableToDB2(
      DataTable dtImp,
      string SqlTableName,
      string currentTime,
      out string err,
      out string info)
    {
      ImportTableToDB2Response db2 = ((IAutoJTService) this).ImportTableToDB2(new ImportTableToDB2Request()
      {
        dtImp = dtImp,
        SqlTableName = SqlTableName,
        currentTime = currentTime
      });
      err = db2.err;
      info = db2.info;
      return db2.ImportTableToDB2Result;
    }

    public Task<ImportTableToDB2Response> ImportTableToDB2Async(ImportTableToDB2Request request)
    {
      return this.Channel.ImportTableToDB2Async(request);
    }

    public string[] GetSqlDataReader1(string SqlString)
    {
      return this.Channel.GetSqlDataReader1(SqlString);
    }

    public Task<string[]> GetSqlDataReader1Async(string SqlString)
    {
      return this.Channel.GetSqlDataReader1Async(SqlString);
    }

    public int GetRowsCount(string SqlString) => this.Channel.GetRowsCount(SqlString);

    public Task<int> GetRowsCountAsync(string SqlString)
    {
      return this.Channel.GetRowsCountAsync(SqlString);
    }

    public bool QueryUsernameFidle(string username) => this.Channel.QueryUsernameFidle(username);

    public Task<bool> QueryUsernameFidleAsync(string username)
    {
      return this.Channel.QueryUsernameFidleAsync(username);
    }

    public string CreateAccount(string username, string password, string email)
    {
      return this.Channel.CreateAccount(username, password, email);
    }

    public Task<string> CreateAccountAsync(string username, string password, string email)
    {
      return this.Channel.CreateAccountAsync(username, password, email);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    UpdateWchatNiceNameResponse IAutoJTService.UpdateWchatNiceName(
      UpdateWchatNiceNameRequest request)
    {
      return this.Channel.UpdateWchatNiceName(request);
    }

    public string UpdateWchatNiceName(string username, string email, out DataSet m_DataSet)
    {
      UpdateWchatNiceNameResponse niceNameResponse = ((IAutoJTService) this).UpdateWchatNiceName(new UpdateWchatNiceNameRequest()
      {
        username = username,
        email = email
      });
      m_DataSet = niceNameResponse.m_DataSet;
      return niceNameResponse.UpdateWchatNiceNameResult;
    }

    public Task<UpdateWchatNiceNameResponse> UpdateWchatNiceNameAsync(
      UpdateWchatNiceNameRequest request)
    {
      return this.Channel.UpdateWchatNiceNameAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    UpdateWchatNiceName_2Net6Response IAutoJTService.UpdateWchatNiceName_2Net6(
      UpdateWchatNiceName_2Net6Request request)
    {
      return this.Channel.UpdateWchatNiceName_2Net6(request);
    }

    public string UpdateWchatNiceName_2Net6(
      string username,
      string email,
      out bool m_validity_status,
      out string remarkName,
      out bool isJT)
    {
      UpdateWchatNiceName_2Net6Response name2Net6Response = ((IAutoJTService) this).UpdateWchatNiceName_2Net6(new UpdateWchatNiceName_2Net6Request()
      {
        username = username,
        email = email
      });
      m_validity_status = name2Net6Response.m_validity_status;
      remarkName = name2Net6Response.remarkName;
      isJT = name2Net6Response.isJT;
      return name2Net6Response.UpdateWchatNiceName_2Net6Result;
    }

    public Task<UpdateWchatNiceName_2Net6Response> UpdateWchatNiceName_2Net6Async(
      UpdateWchatNiceName_2Net6Request request)
    {
      return this.Channel.UpdateWchatNiceName_2Net6Async(request);
    }

    public bool VerificationCodeValid(string email, string Verification_code)
    {
      return this.Channel.VerificationCodeValid(email, Verification_code);
    }

    public Task<bool> VerificationCodeValidAsync(string email, string Verification_code)
    {
      return this.Channel.VerificationCodeValidAsync(email, Verification_code);
    }

    public bool SendVerifyCode(string strToMailbox) => this.Channel.SendVerifyCode(strToMailbox);

    public Task<bool> SendVerifyCodeAsync(string strToMailbox)
    {
      return this.Channel.SendVerifyCodeAsync(strToMailbox);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    GetAutoJTTecnomatixVersionCodeResponse IAutoJTService.GetAutoJTTecnomatixVersionCode(
      GetAutoJTTecnomatixVersionCodeRequest request)
    {
      return this.Channel.GetAutoJTTecnomatixVersionCode(request);
    }

    public string GetAutoJTTecnomatixVersionCode(
      string userInfos,
      out string versionAndContents,
      out string FORCEDUPDATE)
    {
      GetAutoJTTecnomatixVersionCodeResponse tecnomatixVersionCode = ((IAutoJTService) this).GetAutoJTTecnomatixVersionCode(new GetAutoJTTecnomatixVersionCodeRequest()
      {
        userInfos = userInfos
      });
      versionAndContents = tecnomatixVersionCode.versionAndContents;
      FORCEDUPDATE = tecnomatixVersionCode.FORCEDUPDATE;
      return tecnomatixVersionCode.GetAutoJTTecnomatixVersionCodeResult;
    }

    public Task<GetAutoJTTecnomatixVersionCodeResponse> GetAutoJTTecnomatixVersionCodeAsync(
      GetAutoJTTecnomatixVersionCodeRequest request)
    {
      return this.Channel.GetAutoJTTecnomatixVersionCodeAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    GetAutoJTTecnomatixVersionCode2Response IAutoJTService.GetAutoJTTecnomatixVersionCode2(
      GetAutoJTTecnomatixVersionCode2Request request)
    {
      return this.Channel.GetAutoJTTecnomatixVersionCode2(request);
    }

    public string GetAutoJTTecnomatixVersionCode2(
      out string versionAndContents,
      out string FORCEDUPDATE)
    {
      GetAutoJTTecnomatixVersionCode2Response tecnomatixVersionCode2 = ((IAutoJTService) this).GetAutoJTTecnomatixVersionCode2(new GetAutoJTTecnomatixVersionCode2Request());
      versionAndContents = tecnomatixVersionCode2.versionAndContents;
      FORCEDUPDATE = tecnomatixVersionCode2.FORCEDUPDATE;
      return tecnomatixVersionCode2.GetAutoJTTecnomatixVersionCode2Result;
    }

    public Task<GetAutoJTTecnomatixVersionCode2Response> GetAutoJTTecnomatixVersionCode2Async(
      GetAutoJTTecnomatixVersionCode2Request request)
    {
      return this.Channel.GetAutoJTTecnomatixVersionCode2Async(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    GetAutoJTTecnomatixVersionCode3Response IAutoJTService.GetAutoJTTecnomatixVersionCode3(
      GetAutoJTTecnomatixVersionCode3Request request)
    {
      return this.Channel.GetAutoJTTecnomatixVersionCode3(request);
    }

    public string GetAutoJTTecnomatixVersionCode3(
      out string versionAndContents,
      out string FORCEDUPDATE,
      out string downloadLink,
      out string downloadLink2)
    {
      GetAutoJTTecnomatixVersionCode3Response tecnomatixVersionCode3 = ((IAutoJTService) this).GetAutoJTTecnomatixVersionCode3(new GetAutoJTTecnomatixVersionCode3Request());
      versionAndContents = tecnomatixVersionCode3.versionAndContents;
      FORCEDUPDATE = tecnomatixVersionCode3.FORCEDUPDATE;
      downloadLink = tecnomatixVersionCode3.downloadLink;
      downloadLink2 = tecnomatixVersionCode3.downloadLink2;
      return tecnomatixVersionCode3.GetAutoJTTecnomatixVersionCode3Result;
    }

    public Task<GetAutoJTTecnomatixVersionCode3Response> GetAutoJTTecnomatixVersionCode3Async(
      GetAutoJTTecnomatixVersionCode3Request request)
    {
      return this.Channel.GetAutoJTTecnomatixVersionCode3Async(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    GetAutoJTTasksVersionCodeResponse IAutoJTService.GetAutoJTTasksVersionCode(
      GetAutoJTTasksVersionCodeRequest request)
    {
      return this.Channel.GetAutoJTTasksVersionCode(request);
    }

    public string GetAutoJTTasksVersionCode(
      out string versionAndContents,
      out string FORCEDUPDATE,
      out string downloadLink)
    {
      GetAutoJTTasksVersionCodeResponse tasksVersionCode = ((IAutoJTService) this).GetAutoJTTasksVersionCode(new GetAutoJTTasksVersionCodeRequest());
      versionAndContents = tasksVersionCode.versionAndContents;
      FORCEDUPDATE = tasksVersionCode.FORCEDUPDATE;
      downloadLink = tasksVersionCode.downloadLink;
      return tasksVersionCode.GetAutoJTTasksVersionCodeResult;
    }

    public Task<GetAutoJTTasksVersionCodeResponse> GetAutoJTTasksVersionCodeAsync(
      GetAutoJTTasksVersionCodeRequest request)
    {
      return this.Channel.GetAutoJTTasksVersionCodeAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    GetAutoJTTecnomatixVersionCodeExternalResponse IAutoJTService.GetAutoJTTecnomatixVersionCodeExternal(
      GetAutoJTTecnomatixVersionCodeExternalRequest request)
    {
      return this.Channel.GetAutoJTTecnomatixVersionCodeExternal(request);
    }

    public string GetAutoJTTecnomatixVersionCodeExternal(
      out string versionAndContents,
      out string FORCEDUPDATE)
    {
      GetAutoJTTecnomatixVersionCodeExternalResponse versionCodeExternal = ((IAutoJTService) this).GetAutoJTTecnomatixVersionCodeExternal(new GetAutoJTTecnomatixVersionCodeExternalRequest());
      versionAndContents = versionCodeExternal.versionAndContents;
      FORCEDUPDATE = versionCodeExternal.FORCEDUPDATE;
      return versionCodeExternal.GetAutoJTTecnomatixVersionCodeExternalResult;
    }

    public Task<GetAutoJTTecnomatixVersionCodeExternalResponse> GetAutoJTTecnomatixVersionCodeExternalAsync(
      GetAutoJTTecnomatixVersionCodeExternalRequest request)
    {
      return this.Channel.GetAutoJTTecnomatixVersionCodeExternalAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    GetAutoJTTecnomatixVersionCodeExternal3Response IAutoJTService.GetAutoJTTecnomatixVersionCodeExternal3(
      GetAutoJTTecnomatixVersionCodeExternal3Request request)
    {
      return this.Channel.GetAutoJTTecnomatixVersionCodeExternal3(request);
    }

    public string GetAutoJTTecnomatixVersionCodeExternal3(
      out string versionAndContents,
      out string FORCEDUPDATE,
      out string downloadLink,
      out string downloadLink2)
    {
      GetAutoJTTecnomatixVersionCodeExternal3Response versionCodeExternal3 = ((IAutoJTService) this).GetAutoJTTecnomatixVersionCodeExternal3(new GetAutoJTTecnomatixVersionCodeExternal3Request());
      versionAndContents = versionCodeExternal3.versionAndContents;
      FORCEDUPDATE = versionCodeExternal3.FORCEDUPDATE;
      downloadLink = versionCodeExternal3.downloadLink;
      downloadLink2 = versionCodeExternal3.downloadLink2;
      return versionCodeExternal3.GetAutoJTTecnomatixVersionCodeExternal3Result;
    }

    public Task<GetAutoJTTecnomatixVersionCodeExternal3Response> GetAutoJTTecnomatixVersionCodeExternal3Async(
      GetAutoJTTecnomatixVersionCodeExternal3Request request)
    {
      return this.Channel.GetAutoJTTecnomatixVersionCodeExternal3Async(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    CreateTx_Project_MCMResponse IAutoJTService.CreateTx_Project_MCM(
      CreateTx_Project_MCMRequest request)
    {
      return this.Channel.CreateTx_Project_MCM(request);
    }

    public bool CreateTx_Project_MCM(string tbName, out string result)
    {
      CreateTx_Project_MCMResponse txProjectMcm = ((IAutoJTService) this).CreateTx_Project_MCM(new CreateTx_Project_MCMRequest()
      {
        tbName = tbName
      });
      result = txProjectMcm.result;
      return txProjectMcm.CreateTx_Project_MCMResult;
    }

    public Task<CreateTx_Project_MCMResponse> CreateTx_Project_MCMAsync(
      CreateTx_Project_MCMRequest request)
    {
      return this.Channel.CreateTx_Project_MCMAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    CreateTx_Project_MCM_newResponse IAutoJTService.CreateTx_Project_MCM_new(
      CreateTx_Project_MCM_newRequest request)
    {
      return this.Channel.CreateTx_Project_MCM_new(request);
    }

    public bool CreateTx_Project_MCM_new(string tbName, out string result, out string error)
    {
      CreateTx_Project_MCM_newResponse txProjectMcmNew = ((IAutoJTService) this).CreateTx_Project_MCM_new(new CreateTx_Project_MCM_newRequest()
      {
        tbName = tbName
      });
      result = txProjectMcmNew.result;
      error = txProjectMcmNew.error;
      return txProjectMcmNew.CreateTx_Project_MCM_newResult;
    }

    public Task<CreateTx_Project_MCM_newResponse> CreateTx_Project_MCM_newAsync(
      CreateTx_Project_MCM_newRequest request)
    {
      return this.Channel.CreateTx_Project_MCM_newAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    AddinProject_Model2DB_MCMResponse IAutoJTService.AddinProject_Model2DB_MCM(
      AddinProject_Model2DB_MCMRequest request)
    {
      return this.Channel.AddinProject_Model2DB_MCM(request);
    }

    public bool AddinProject_Model2DB_MCM(
      DataTable readDataTable,
      string currentTime,
      out string err,
      out string info)
    {
      AddinProject_Model2DB_MCMResponse model2DbMcmResponse = ((IAutoJTService) this).AddinProject_Model2DB_MCM(new AddinProject_Model2DB_MCMRequest()
      {
        readDataTable = readDataTable,
        currentTime = currentTime
      });
      err = model2DbMcmResponse.err;
      info = model2DbMcmResponse.info;
      return model2DbMcmResponse.AddinProject_Model2DB_MCMResult;
    }

    public Task<AddinProject_Model2DB_MCMResponse> AddinProject_Model2DB_MCMAsync(
      AddinProject_Model2DB_MCMRequest request)
    {
      return this.Channel.AddinProject_Model2DB_MCMAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    AddinProject_Model2DB_MCM_newResponse IAutoJTService.AddinProject_Model2DB_MCM_new(
      AddinProject_Model2DB_MCM_newRequest request)
    {
      return this.Channel.AddinProject_Model2DB_MCM_new(request);
    }

    public bool AddinProject_Model2DB_MCM_new(
      string PROJECT,
      string MODEL,
      string REGION,
      string uuid,
      string OPERATOR,
      string tbname,
      out string infos)
    {
      AddinProject_Model2DB_MCM_newResponse dbMcmNewResponse = ((IAutoJTService) this).AddinProject_Model2DB_MCM_new(new AddinProject_Model2DB_MCM_newRequest()
      {
        PROJECT = PROJECT,
        MODEL = MODEL,
        REGION = REGION,
        uuid = uuid,
        OPERATOR = OPERATOR,
        tbname = tbname
      });
      infos = dbMcmNewResponse.infos;
      return dbMcmNewResponse.AddinProject_Model2DB_MCM_newResult;
    }

    public Task<AddinProject_Model2DB_MCM_newResponse> AddinProject_Model2DB_MCM_newAsync(
      AddinProject_Model2DB_MCM_newRequest request)
    {
      return this.Channel.AddinProject_Model2DB_MCM_newAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    CreateTx_Project_Model_Lib_MCMResponse IAutoJTService.CreateTx_Project_Model_Lib_MCM(
      CreateTx_Project_Model_Lib_MCMRequest request)
    {
      return this.Channel.CreateTx_Project_Model_Lib_MCM(request);
    }

    public bool CreateTx_Project_Model_Lib_MCM(string tbName, out string result)
    {
      CreateTx_Project_Model_Lib_MCMResponse projectModelLibMcm = ((IAutoJTService) this).CreateTx_Project_Model_Lib_MCM(new CreateTx_Project_Model_Lib_MCMRequest()
      {
        tbName = tbName
      });
      result = projectModelLibMcm.result;
      return projectModelLibMcm.CreateTx_Project_Model_Lib_MCMResult;
    }

    public Task<CreateTx_Project_Model_Lib_MCMResponse> CreateTx_Project_Model_Lib_MCMAsync(
      CreateTx_Project_Model_Lib_MCMRequest request)
    {
      return this.Channel.CreateTx_Project_Model_Lib_MCMAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    Create_MCM_LibraryTableResponse IAutoJTService.Create_MCM_LibraryTable(
      Create_MCM_LibraryTableRequest request)
    {
      return this.Channel.Create_MCM_LibraryTable(request);
    }

    public bool Create_MCM_LibraryTable(string tbName, out string infos)
    {
      Create_MCM_LibraryTableResponse mcmLibraryTable = ((IAutoJTService) this).Create_MCM_LibraryTable(new Create_MCM_LibraryTableRequest()
      {
        tbName = tbName
      });
      infos = mcmLibraryTable.infos;
      return mcmLibraryTable.Create_MCM_LibraryTableResult;
    }

    public Task<Create_MCM_LibraryTableResponse> Create_MCM_LibraryTableAsync(
      Create_MCM_LibraryTableRequest request)
    {
      return this.Channel.Create_MCM_LibraryTableAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    BulkInsertMCMData_MCMResponse IAutoJTService.BulkInsertMCMData_MCM(
      BulkInsertMCMData_MCMRequest request)
    {
      return this.Channel.BulkInsertMCMData_MCM(request);
    }

    public bool BulkInsertMCMData_MCM(
      DataTable dataTable1,
      string tbName,
      out string infos,
      out int irows)
    {
      BulkInsertMCMData_MCMResponse mcmDataMcmResponse = ((IAutoJTService) this).BulkInsertMCMData_MCM(new BulkInsertMCMData_MCMRequest()
      {
        dataTable1 = dataTable1,
        tbName = tbName
      });
      infos = mcmDataMcmResponse.infos;
      irows = mcmDataMcmResponse.irows;
      return mcmDataMcmResponse.BulkInsertMCMData_MCMResult;
    }

    public Task<BulkInsertMCMData_MCMResponse> BulkInsertMCMData_MCMAsync(
      BulkInsertMCMData_MCMRequest request)
    {
      return this.Channel.BulkInsertMCMData_MCMAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    AddinTx_Project_Model_Lib_MCMResponse IAutoJTService.AddinTx_Project_Model_Lib_MCM(
      AddinTx_Project_Model_Lib_MCMRequest request)
    {
      return this.Channel.AddinTx_Project_Model_Lib_MCM(request);
    }

    public bool AddinTx_Project_Model_Lib_MCM(
      DataTable readDataTable,
      string SqlTableName,
      out string err,
      out string info)
    {
      AddinTx_Project_Model_Lib_MCMResponse modelLibMcmResponse = ((IAutoJTService) this).AddinTx_Project_Model_Lib_MCM(new AddinTx_Project_Model_Lib_MCMRequest()
      {
        readDataTable = readDataTable,
        SqlTableName = SqlTableName
      });
      err = modelLibMcmResponse.err;
      info = modelLibMcmResponse.info;
      return modelLibMcmResponse.AddinTx_Project_Model_Lib_MCMResult;
    }

    public Task<AddinTx_Project_Model_Lib_MCMResponse> AddinTx_Project_Model_Lib_MCMAsync(
      AddinTx_Project_Model_Lib_MCMRequest request)
    {
      return this.Channel.AddinTx_Project_Model_Lib_MCMAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    DeleteProject_1Response IAutoJTService.DeleteProject_1(DeleteProject_1Request request)
    {
      return this.Channel.DeleteProject_1(request);
    }

    public string DeleteProject_1(
      string uuid,
      out string infos_2,
      out string infos_3,
      out int icount_1,
      out int icount_2,
      out int icount_3)
    {
      DeleteProject_1Response project1Response = ((IAutoJTService) this).DeleteProject_1(new DeleteProject_1Request()
      {
        uuid = uuid
      });
      infos_2 = project1Response.infos_2;
      infos_3 = project1Response.infos_3;
      icount_1 = project1Response.icount_1;
      icount_2 = project1Response.icount_2;
      icount_3 = project1Response.icount_3;
      return project1Response.infos_1;
    }

    public Task<DeleteProject_1Response> DeleteProject_1Async(DeleteProject_1Request request)
    {
      return this.Channel.DeleteProject_1Async(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    CreateTx_Project_Model_Users_MCMResponse IAutoJTService.CreateTx_Project_Model_Users_MCM(
      CreateTx_Project_Model_Users_MCMRequest request)
    {
      return this.Channel.CreateTx_Project_Model_Users_MCM(request);
    }

    public bool CreateTx_Project_Model_Users_MCM(string tbName, out string result)
    {
      CreateTx_Project_Model_Users_MCMResponse projectModelUsersMcm = ((IAutoJTService) this).CreateTx_Project_Model_Users_MCM(new CreateTx_Project_Model_Users_MCMRequest()
      {
        tbName = tbName
      });
      result = projectModelUsersMcm.result;
      return projectModelUsersMcm.CreateTx_Project_Model_Users_MCMResult;
    }

    public Task<CreateTx_Project_Model_Users_MCMResponse> CreateTx_Project_Model_Users_MCMAsync(
      CreateTx_Project_Model_Users_MCMRequest request)
    {
      return this.Channel.CreateTx_Project_Model_Users_MCMAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    AddinTx_Project_Model_Users_MCMResponse IAutoJTService.AddinTx_Project_Model_Users_MCM(
      AddinTx_Project_Model_Users_MCMRequest request)
    {
      return this.Channel.AddinTx_Project_Model_Users_MCM(request);
    }

    public bool AddinTx_Project_Model_Users_MCM(
      DataTable readDataTable,
      string SqlTableName,
      out string err,
      out string info)
    {
      AddinTx_Project_Model_Users_MCMResponse usersMcmResponse = ((IAutoJTService) this).AddinTx_Project_Model_Users_MCM(new AddinTx_Project_Model_Users_MCMRequest()
      {
        readDataTable = readDataTable,
        SqlTableName = SqlTableName
      });
      err = usersMcmResponse.err;
      info = usersMcmResponse.info;
      return usersMcmResponse.AddinTx_Project_Model_Users_MCMResult;
    }

    public Task<AddinTx_Project_Model_Users_MCMResponse> AddinTx_Project_Model_Users_MCMAsync(
      AddinTx_Project_Model_Users_MCMRequest request)
    {
      return this.Channel.AddinTx_Project_Model_Users_MCMAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    CreateAndBulkInsertUserMCMData_MCMResponse IAutoJTService.CreateAndBulkInsertUserMCMData_MCM(
      CreateAndBulkInsertUserMCMData_MCMRequest request)
    {
      return this.Channel.CreateAndBulkInsertUserMCMData_MCM(request);
    }

    public bool CreateAndBulkInsertUserMCMData_MCM(
      DataTable dataTable1,
      string tbName,
      out string infos,
      out int irows,
      out int icount)
    {
      CreateAndBulkInsertUserMCMData_MCMResponse insertUserMcmDataMcm = ((IAutoJTService) this).CreateAndBulkInsertUserMCMData_MCM(new CreateAndBulkInsertUserMCMData_MCMRequest()
      {
        dataTable1 = dataTable1,
        tbName = tbName
      });
      infos = insertUserMcmDataMcm.infos;
      irows = insertUserMcmDataMcm.irows;
      icount = insertUserMcmDataMcm.icount;
      return insertUserMcmDataMcm.CreateAndBulkInsertUserMCMData_MCMResult;
    }

    public Task<CreateAndBulkInsertUserMCMData_MCMResponse> CreateAndBulkInsertUserMCMData_MCMAsync(
      CreateAndBulkInsertUserMCMData_MCMRequest request)
    {
      return this.Channel.CreateAndBulkInsertUserMCMData_MCMAsync(request);
    }

    public string EmptyTable_1(string tableName) => this.Channel.EmptyTable_1(tableName);

    public Task<string> EmptyTable_1Async(string tableName)
    {
      return this.Channel.EmptyTable_1Async(tableName);
    }

    public string EmptyTable_new(string tableName) => this.Channel.EmptyTable_new(tableName);

    public Task<string> EmptyTable_newAsync(string tableName)
    {
      return this.Channel.EmptyTable_newAsync(tableName);
    }

    public int GetRowsCount_2(string tbName) => this.Channel.GetRowsCount_2(tbName);

    public Task<int> GetRowsCount_2Async(string tbName) => this.Channel.GetRowsCount_2Async(tbName);

    public string[] QueryJT_username() => this.Channel.QueryJT_username();

    public Task<string[]> QueryJT_usernameAsync() => this.Channel.QueryJT_usernameAsync();

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    Tx_Refresh_MCMResponse IAutoJTService.Tx_Refresh_MCM(Tx_Refresh_MCMRequest request)
    {
      return this.Channel.Tx_Refresh_MCM(request);
    }

    public DataTable Tx_Refresh_MCM(
      string project,
      string model,
      out string result_lib,
      out DataTable[] dataTable_users_MCMS,
      out string result_user)
    {
      Tx_Refresh_MCMResponse refreshMcmResponse = ((IAutoJTService) this).Tx_Refresh_MCM(new Tx_Refresh_MCMRequest()
      {
        project = project,
        model = model
      });
      result_lib = refreshMcmResponse.result_lib;
      dataTable_users_MCMS = refreshMcmResponse.dataTable_users_MCMS;
      result_user = refreshMcmResponse.result_user;
      return refreshMcmResponse.dataTable_LIB;
    }

    public Task<Tx_Refresh_MCMResponse> Tx_Refresh_MCMAsync(Tx_Refresh_MCMRequest request)
    {
      return this.Channel.Tx_Refresh_MCMAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    Tx_Refresh_MCM_new1Response IAutoJTService.Tx_Refresh_MCM_new1(
      Tx_Refresh_MCM_new1Request request)
    {
      return this.Channel.Tx_Refresh_MCM_new1(request);
    }

    public DataTable Tx_Refresh_MCM_new1(
      string uuid,
      out DataTable[] dataTable_AllUsers,
      out DataTable NickNameOpenidDic,
      out string infos)
    {
      Tx_Refresh_MCM_new1Response refreshMcmNew1Response = ((IAutoJTService) this).Tx_Refresh_MCM_new1(new Tx_Refresh_MCM_new1Request()
      {
        uuid = uuid
      });
      dataTable_AllUsers = refreshMcmNew1Response.dataTable_AllUsers;
      NickNameOpenidDic = refreshMcmNew1Response.NickNameOpenidDic;
      infos = refreshMcmNew1Response.infos;
      return refreshMcmNew1Response.dataTable_Library;
    }

    public Task<Tx_Refresh_MCM_new1Response> Tx_Refresh_MCM_new1Async(
      Tx_Refresh_MCM_new1Request request)
    {
      return this.Channel.Tx_Refresh_MCM_new1Async(request);
    }

    public bool Query_ProjectModleExists_MCM(string project_1, string model_1)
    {
      return this.Channel.Query_ProjectModleExists_MCM(project_1, model_1);
    }

    public Task<bool> Query_ProjectModleExists_MCMAsync(string project_1, string model_1)
    {
      return this.Channel.Query_ProjectModleExists_MCMAsync(project_1, model_1);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    Query_ProExists_uuid_MCMResponse IAutoJTService.Query_ProExists_uuid_MCM(
      Query_ProExists_uuid_MCMRequest request)
    {
      return this.Channel.Query_ProExists_uuid_MCM(request);
    }

    public DataTable Query_ProExists_uuid_MCM(
      string uuid,
      string openid,
      out int status,
      out string infos)
    {
      Query_ProExists_uuid_MCMResponse existsUuidMcmResponse = ((IAutoJTService) this).Query_ProExists_uuid_MCM(new Query_ProExists_uuid_MCMRequest()
      {
        uuid = uuid,
        openid = openid
      });
      status = existsUuidMcmResponse.status;
      infos = existsUuidMcmResponse.infos;
      return existsUuidMcmResponse.Query_ProExists_uuid_MCMResult;
    }

    public Task<Query_ProExists_uuid_MCMResponse> Query_ProExists_uuid_MCMAsync(
      Query_ProExists_uuid_MCMRequest request)
    {
      return this.Channel.Query_ProExists_uuid_MCMAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    Query_AllPro4OpenIDResponse IAutoJTService.Query_AllPro4OpenID(
      Query_AllPro4OpenIDRequest request)
    {
      return this.Channel.Query_AllPro4OpenID(request);
    }

    public DataTable Query_AllPro4OpenID(string openID, out string infos)
    {
      Query_AllPro4OpenIDResponse pro4OpenIdResponse = ((IAutoJTService) this).Query_AllPro4OpenID(new Query_AllPro4OpenIDRequest()
      {
        openID = openID
      });
      infos = pro4OpenIdResponse.infos;
      return pro4OpenIdResponse.Query_AllPro4OpenIDResult;
    }

    public Task<Query_AllPro4OpenIDResponse> Query_AllPro4OpenIDAsync(
      Query_AllPro4OpenIDRequest request)
    {
      return this.Channel.Query_AllPro4OpenIDAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    Query_UsersTakeProjectsOpenIDResponse IAutoJTService.Query_UsersTakeProjectsOpenID(
      Query_UsersTakeProjectsOpenIDRequest request)
    {
      return this.Channel.Query_UsersTakeProjectsOpenID(request);
    }

    public DataTable Query_UsersTakeProjectsOpenID(string openID, out string infos)
    {
      Query_UsersTakeProjectsOpenIDResponse projectsOpenId = ((IAutoJTService) this).Query_UsersTakeProjectsOpenID(new Query_UsersTakeProjectsOpenIDRequest()
      {
        openID = openID
      });
      infos = projectsOpenId.infos;
      return projectsOpenId.Query_UsersTakeProjectsOpenIDResult;
    }

    public Task<Query_UsersTakeProjectsOpenIDResponse> Query_UsersTakeProjectsOpenIDAsync(
      Query_UsersTakeProjectsOpenIDRequest request)
    {
      return this.Channel.Query_UsersTakeProjectsOpenIDAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    Query_UserNameByOpenIDResponse IAutoJTService.Query_UserNameByOpenID(
      Query_UserNameByOpenIDRequest request)
    {
      return this.Channel.Query_UserNameByOpenID(request);
    }

    public string Query_UserNameByOpenID(string openID, out string infos)
    {
      Query_UserNameByOpenIDResponse byOpenIdResponse = ((IAutoJTService) this).Query_UserNameByOpenID(new Query_UserNameByOpenIDRequest()
      {
        openID = openID
      });
      infos = byOpenIdResponse.infos;
      return byOpenIdResponse.Query_UserNameByOpenIDResult;
    }

    public Task<Query_UserNameByOpenIDResponse> Query_UserNameByOpenIDAsync(
      Query_UserNameByOpenIDRequest request)
    {
      return this.Channel.Query_UserNameByOpenIDAsync(request);
    }

    public string Module_usage_statistics(DataTable modelInfos)
    {
      return this.Channel.Module_usage_statistics(modelInfos);
    }

    public Task<string> Module_usage_statisticsAsync(DataTable modelInfos)
    {
      return this.Channel.Module_usage_statisticsAsync(modelInfos);
    }

    public string Module_usage_statistics_21(DataTable modelInfos)
    {
      return this.Channel.Module_usage_statistics_21(modelInfos);
    }

    public Task<string> Module_usage_statistics_21Async(DataTable modelInfos)
    {
      return this.Channel.Module_usage_statistics_21Async(modelInfos);
    }

    public string Module_usage_statistics2(object[] modelInfos)
    {
      return this.Channel.Module_usage_statistics2(modelInfos);
    }

    public Task<string> Module_usage_statistics2Async(object[] modelInfos)
    {
      return this.Channel.Module_usage_statistics2Async(modelInfos);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    CalcWD_Rotation_4Response IAutoJTService.CalcWD_Rotation_4(CalcWD_Rotation_4Request request)
    {
      return this.Channel.CalcWD_Rotation_4(request);
    }

    public double[] CalcWD_Rotation_4(
      double[] txTrans_new_world_num,
      double[] txTrans_new_apper_word_Inverse_num,
      double[] txTrans_old_apper_symmetry_num,
      bool istxPartAppearance_newNOTNull,
      bool isSymmetryWeld1,
      out string exInfos)
    {
      CalcWD_Rotation_4Response rotation4Response = ((IAutoJTService) this).CalcWD_Rotation_4(new CalcWD_Rotation_4Request()
      {
        txTrans_new_world_num = txTrans_new_world_num,
        txTrans_new_apper_word_Inverse_num = txTrans_new_apper_word_Inverse_num,
        txTrans_old_apper_symmetry_num = txTrans_old_apper_symmetry_num,
        istxPartAppearance_newNOTNull = istxPartAppearance_newNOTNull,
        isSymmetryWeld1 = isSymmetryWeld1
      });
      exInfos = rotation4Response.exInfos;
      return rotation4Response.CalcWD_Rotation_4Result;
    }

    public Task<CalcWD_Rotation_4Response> CalcWD_Rotation_4Async(CalcWD_Rotation_4Request request)
    {
      return this.Channel.CalcWD_Rotation_4Async(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    CalcWD_Rotation_5Response IAutoJTService.CalcWD_Rotation_5(CalcWD_Rotation_5Request request)
    {
      return this.Channel.CalcWD_Rotation_5(request);
    }

    public double[] CalcWD_Rotation_5(
      double[] txTrans_new_world_num,
      double[] txTrans_new_apper_word_Inverse_num,
      double[] txTrans_old_apper_symmetry_num,
      bool istxPartAppearance_newNOTNull,
      bool isSymmetryWeld1,
      out string exInfos)
    {
      CalcWD_Rotation_5Response rotation5Response = ((IAutoJTService) this).CalcWD_Rotation_5(new CalcWD_Rotation_5Request()
      {
        txTrans_new_world_num = txTrans_new_world_num,
        txTrans_new_apper_word_Inverse_num = txTrans_new_apper_word_Inverse_num,
        txTrans_old_apper_symmetry_num = txTrans_old_apper_symmetry_num,
        istxPartAppearance_newNOTNull = istxPartAppearance_newNOTNull,
        isSymmetryWeld1 = isSymmetryWeld1
      });
      exInfos = rotation5Response.exInfos;
      return rotation5Response.CalcWD_Rotation_5Result;
    }

    public Task<CalcWD_Rotation_5Response> CalcWD_Rotation_5Async(CalcWD_Rotation_5Request request)
    {
      return this.Channel.CalcWD_Rotation_5Async(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    CalcVIA_Rotation_4Response IAutoJTService.CalcVIA_Rotation_4(CalcVIA_Rotation_4Request request)
    {
      return this.Channel.CalcVIA_Rotation_4(request);
    }

    public double[] CalcVIA_Rotation_4(
      double[] txTrans_old_VIA_OLD_apper_num,
      double[] txTrans_old_OLD_apper_world_num,
      out string exInfos)
    {
      CalcVIA_Rotation_4Response rotation4Response = ((IAutoJTService) this).CalcVIA_Rotation_4(new CalcVIA_Rotation_4Request()
      {
        txTrans_old_VIA_OLD_apper_num = txTrans_old_VIA_OLD_apper_num,
        txTrans_old_OLD_apper_world_num = txTrans_old_OLD_apper_world_num
      });
      exInfos = rotation4Response.exInfos;
      return rotation4Response.CalcVIA_Rotation_4Result;
    }

    public Task<CalcVIA_Rotation_4Response> CalcVIA_Rotation_4Async(
      CalcVIA_Rotation_4Request request)
    {
      return this.Channel.CalcVIA_Rotation_4Async(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    Calc_RPY2Matrix_TransformResponse IAutoJTService.Calc_RPY2Matrix_Transform(
      Calc_RPY2Matrix_TransformRequest request)
    {
      return this.Channel.Calc_RPY2Matrix_Transform(request);
    }

    public DataTable Calc_RPY2Matrix_Transform(DataTable dataTable1, out string exInfos)
    {
      Calc_RPY2Matrix_TransformResponse transformResponse = ((IAutoJTService) this).Calc_RPY2Matrix_Transform(new Calc_RPY2Matrix_TransformRequest()
      {
        dataTable1 = dataTable1
      });
      exInfos = transformResponse.exInfos;
      return transformResponse.Calc_RPY2Matrix_TransformResult;
    }

    public Task<Calc_RPY2Matrix_TransformResponse> Calc_RPY2Matrix_TransformAsync(
      Calc_RPY2Matrix_TransformRequest request)
    {
      return this.Channel.Calc_RPY2Matrix_TransformAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    Calc_RPY2Matrix_Transform2Response IAutoJTService.Calc_RPY2Matrix_Transform2(
      Calc_RPY2Matrix_Transform2Request request)
    {
      return this.Channel.Calc_RPY2Matrix_Transform2(request);
    }

    public DataTable Calc_RPY2Matrix_Transform2(DataTable dataTable1, out string exInfos)
    {
      Calc_RPY2Matrix_Transform2Response transform2Response = ((IAutoJTService) this).Calc_RPY2Matrix_Transform2(new Calc_RPY2Matrix_Transform2Request()
      {
        dataTable1 = dataTable1
      });
      exInfos = transform2Response.exInfos;
      return transform2Response.Calc_RPY2Matrix_Transform2Result;
    }

    public Task<Calc_RPY2Matrix_Transform2Response> Calc_RPY2Matrix_Transform2Async(
      Calc_RPY2Matrix_Transform2Request request)
    {
      return this.Channel.Calc_RPY2Matrix_Transform2Async(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    CalcNoteQuadrantResponse IAutoJTService.CalcNoteQuadrant(CalcNoteQuadrantRequest request)
    {
      return this.Channel.CalcNoteQuadrant(request);
    }

    public DataTable CalcNoteQuadrant(
      DataTable dataTable1,
      out object[] minXZ,
      out object[] maxXZ,
      out object[] centerXZ,
      out string exInfos)
    {
      CalcNoteQuadrantResponse quadrantResponse = ((IAutoJTService) this).CalcNoteQuadrant(new CalcNoteQuadrantRequest()
      {
        dataTable1 = dataTable1
      });
      minXZ = quadrantResponse.minXZ;
      maxXZ = quadrantResponse.maxXZ;
      centerXZ = quadrantResponse.centerXZ;
      exInfos = quadrantResponse.exInfos;
      return quadrantResponse.CalcNoteQuadrantResult;
    }

    public Task<CalcNoteQuadrantResponse> CalcNoteQuadrantAsync(CalcNoteQuadrantRequest request)
    {
      return this.Channel.CalcNoteQuadrantAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    CalcRealBoxLocationResponse IAutoJTService.CalcRealBoxLocation(
      CalcRealBoxLocationRequest request)
    {
      return this.Channel.CalcRealBoxLocation(request);
    }

    public bool CalcRealBoxLocation(
      double[] txFrameCenter,
      double[] leftLower,
      double[] rightUpper,
      double[] txVector_ora,
      bool isCalcWordLoc,
      out double[] bottomLeftLower,
      out double[] bottomLeftUpper,
      out double[] bottomRightLower,
      out double[] bottomRightUpper,
      out double[] topLeftLower,
      out double[] topLeftUpper,
      out double[] topRightLower,
      out double[] topRightUpper,
      out double[] pp0,
      out double[] p1_word,
      out double[] p2_word)
    {
      CalcRealBoxLocationResponse locationResponse = ((IAutoJTService) this).CalcRealBoxLocation(new CalcRealBoxLocationRequest()
      {
        txFrameCenter = txFrameCenter,
        leftLower = leftLower,
        rightUpper = rightUpper,
        txVector_ora = txVector_ora,
        isCalcWordLoc = isCalcWordLoc
      });
      bottomLeftLower = locationResponse.bottomLeftLower;
      bottomLeftUpper = locationResponse.bottomLeftUpper;
      bottomRightLower = locationResponse.bottomRightLower;
      bottomRightUpper = locationResponse.bottomRightUpper;
      topLeftLower = locationResponse.topLeftLower;
      topLeftUpper = locationResponse.topLeftUpper;
      topRightLower = locationResponse.topRightLower;
      topRightUpper = locationResponse.topRightUpper;
      pp0 = locationResponse.pp0;
      p1_word = locationResponse.p1_word;
      p2_word = locationResponse.p2_word;
      return locationResponse.CalcRealBoxLocationResult;
    }

    public Task<CalcRealBoxLocationResponse> CalcRealBoxLocationAsync(
      CalcRealBoxLocationRequest request)
    {
      return this.Channel.CalcRealBoxLocationAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    MultiCalcRealBoxLocationResponse IAutoJTService.MultiCalcRealBoxLocation(
      MultiCalcRealBoxLocationRequest request)
    {
      return this.Channel.MultiCalcRealBoxLocation(request);
    }

    public bool MultiCalcRealBoxLocation(
      double[] txFrameCenter_1,
      double[] leftLower_1,
      double[] rightUpper_1,
      double[] txVector_ora_1,
      double[] txFrameCenter_2,
      double[] leftLower_2,
      double[] rightUpper_2,
      double[] txVector_ora_2,
      bool isCalcWordLoc_1,
      bool isCalcWordLoc_2,
      out double[] bottomLeftLower_1,
      out double[] bottomLeftUpper_1,
      out double[] bottomRightLower_1,
      out double[] bottomRightUpper_1,
      out double[] topLeftLower_1,
      out double[] topLeftUpper_1,
      out double[] topRightLower_1,
      out double[] topRightUpper_1,
      out double[] pp0_1,
      out double[] p1_word_1,
      out double[] p2_word_1,
      out double[] bottomLeftLower_2,
      out double[] bottomLeftUpper_2,
      out double[] bottomRightLower_2,
      out double[] bottomRightUpper_2,
      out double[] topLeftLower_2,
      out double[] topLeftUpper_2,
      out double[] topRightLower_2,
      out double[] topRightUpper_2,
      out double[] pp0_2,
      out double[] p1_word_2,
      out double[] p2_word_2)
    {
      MultiCalcRealBoxLocationResponse locationResponse = ((IAutoJTService) this).MultiCalcRealBoxLocation(new MultiCalcRealBoxLocationRequest()
      {
        txFrameCenter_1 = txFrameCenter_1,
        leftLower_1 = leftLower_1,
        rightUpper_1 = rightUpper_1,
        txVector_ora_1 = txVector_ora_1,
        txFrameCenter_2 = txFrameCenter_2,
        leftLower_2 = leftLower_2,
        rightUpper_2 = rightUpper_2,
        txVector_ora_2 = txVector_ora_2,
        isCalcWordLoc_1 = isCalcWordLoc_1,
        isCalcWordLoc_2 = isCalcWordLoc_2
      });
      bottomLeftLower_1 = locationResponse.bottomLeftLower_1;
      bottomLeftUpper_1 = locationResponse.bottomLeftUpper_1;
      bottomRightLower_1 = locationResponse.bottomRightLower_1;
      bottomRightUpper_1 = locationResponse.bottomRightUpper_1;
      topLeftLower_1 = locationResponse.topLeftLower_1;
      topLeftUpper_1 = locationResponse.topLeftUpper_1;
      topRightLower_1 = locationResponse.topRightLower_1;
      topRightUpper_1 = locationResponse.topRightUpper_1;
      pp0_1 = locationResponse.pp0_1;
      p1_word_1 = locationResponse.p1_word_1;
      p2_word_1 = locationResponse.p2_word_1;
      bottomLeftLower_2 = locationResponse.bottomLeftLower_2;
      bottomLeftUpper_2 = locationResponse.bottomLeftUpper_2;
      bottomRightLower_2 = locationResponse.bottomRightLower_2;
      bottomRightUpper_2 = locationResponse.bottomRightUpper_2;
      topLeftLower_2 = locationResponse.topLeftLower_2;
      topLeftUpper_2 = locationResponse.topLeftUpper_2;
      topRightLower_2 = locationResponse.topRightLower_2;
      topRightUpper_2 = locationResponse.topRightUpper_2;
      pp0_2 = locationResponse.pp0_2;
      p1_word_2 = locationResponse.p1_word_2;
      p2_word_2 = locationResponse.p2_word_2;
      return locationResponse.MultiCalcRealBoxLocationResult;
    }

    public Task<MultiCalcRealBoxLocationResponse> MultiCalcRealBoxLocationAsync(
      MultiCalcRealBoxLocationRequest request)
    {
      return this.Channel.MultiCalcRealBoxLocationAsync(request);
    }

    public double[] ReCalcRotaFrme(
      double[] p1_word,
      double[] p2_word,
      double[] p11_word,
      double[] p21_word)
    {
      return this.Channel.ReCalcRotaFrme(p1_word, p2_word, p11_word, p21_word);
    }

    public Task<double[]> ReCalcRotaFrmeAsync(
      double[] p1_word,
      double[] p2_word,
      double[] p11_word,
      double[] p21_word)
    {
      return this.Channel.ReCalcRotaFrmeAsync(p1_word, p2_word, p11_word, p21_word);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    GetAutoJTVersionCodeResponse IAutoJTService.GetAutoJTVersionCode(
      GetAutoJTVersionCodeRequest request)
    {
      return this.Channel.GetAutoJTVersionCode(request);
    }

    public string GetAutoJTVersionCode(
      string userInfos,
      out string versionAndContents,
      out string FORCEDUPDATE)
    {
      GetAutoJTVersionCodeResponse autoJtVersionCode = ((IAutoJTService) this).GetAutoJTVersionCode(new GetAutoJTVersionCodeRequest()
      {
        userInfos = userInfos
      });
      versionAndContents = autoJtVersionCode.versionAndContents;
      FORCEDUPDATE = autoJtVersionCode.FORCEDUPDATE;
      return autoJtVersionCode.GetAutoJTVersionCodeResult;
    }

    public Task<GetAutoJTVersionCodeResponse> GetAutoJTVersionCodeAsync(
      GetAutoJTVersionCodeRequest request)
    {
      return this.Channel.GetAutoJTVersionCodeAsync(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    GetAutoJTVersionCode2Response IAutoJTService.GetAutoJTVersionCode2(
      GetAutoJTVersionCode2Request request)
    {
      return this.Channel.GetAutoJTVersionCode2(request);
    }

    public string GetAutoJTVersionCode2(out string versionAndContents, out string FORCEDUPDATE)
    {
      GetAutoJTVersionCode2Response autoJtVersionCode2 = ((IAutoJTService) this).GetAutoJTVersionCode2(new GetAutoJTVersionCode2Request());
      versionAndContents = autoJtVersionCode2.versionAndContents;
      FORCEDUPDATE = autoJtVersionCode2.FORCEDUPDATE;
      return autoJtVersionCode2.GetAutoJTVersionCode2Result;
    }

    public Task<GetAutoJTVersionCode2Response> GetAutoJTVersionCode2Async(
      GetAutoJTVersionCode2Request request)
    {
      return this.Channel.GetAutoJTVersionCode2Async(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    GetAutoJTVersionCodeExternal3Response IAutoJTService.GetAutoJTVersionCodeExternal3(
      GetAutoJTVersionCodeExternal3Request request)
    {
      return this.Channel.GetAutoJTVersionCodeExternal3(request);
    }

    public string GetAutoJTVersionCodeExternal3(
      out string versionAndContents,
      out string FORCEDUPDATE,
      out string downloadLink)
    {
      GetAutoJTVersionCodeExternal3Response versionCodeExternal3 = ((IAutoJTService) this).GetAutoJTVersionCodeExternal3(new GetAutoJTVersionCodeExternal3Request());
      versionAndContents = versionCodeExternal3.versionAndContents;
      FORCEDUPDATE = versionCodeExternal3.FORCEDUPDATE;
      downloadLink = versionCodeExternal3.downloadLink;
      return versionCodeExternal3.GetAutoJTVersionCodeExternal3Result;
    }

    public Task<GetAutoJTVersionCodeExternal3Response> GetAutoJTVersionCodeExternal3Async(
      GetAutoJTVersionCodeExternal3Request request)
    {
      return this.Channel.GetAutoJTVersionCodeExternal3Async(request);
    }
  }
}
