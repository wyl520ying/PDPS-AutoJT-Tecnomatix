





using System.CodeDom.Compiler;
using System.Data;
using System.ServiceModel;
using System.Threading.Tasks;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [ServiceContract(ConfigurationName = "AutoJTServiceReference.IAutoJTService")]
  public interface IAutoJTService
  {
    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetCPPRuntimeUrl", ReplyAction = "http://tempuri.org/IAutoJTService/GetCPPRuntimeUrlResponse")]
    string GetCPPRuntimeUrl();

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetCPPRuntimeUrl", ReplyAction = "http://tempuri.org/IAutoJTService/GetCPPRuntimeUrlResponse")]
    Task<string> GetCPPRuntimeUrlAsync();

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetUnionIDNickName4ClientInfos", ReplyAction = "http://tempuri.org/IAutoJTService/GetUnionIDNickName4ClientInfosResponse")]
    GetUnionIDNickName4ClientInfosResponse GetUnionIDNickName4ClientInfos(
      GetUnionIDNickName4ClientInfosRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetUnionIDNickName4ClientInfos", ReplyAction = "http://tempuri.org/IAutoJTService/GetUnionIDNickName4ClientInfosResponse")]
    Task<GetUnionIDNickName4ClientInfosResponse> GetUnionIDNickName4ClientInfosAsync(
      GetUnionIDNickName4ClientInfosRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/UserAuthentication", ReplyAction = "http://tempuri.org/IAutoJTService/UserAuthenticationResponse")]
    UserAuthenticationResponse UserAuthentication(UserAuthenticationRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/UserAuthentication", ReplyAction = "http://tempuri.org/IAutoJTService/UserAuthenticationResponse")]
    Task<UserAuthenticationResponse> UserAuthenticationAsync(UserAuthenticationRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/UpgradeNickname", ReplyAction = "http://tempuri.org/IAutoJTService/UpgradeNicknameResponse")]
    bool UpgradeNickname(string uuid, string newNickname, bool isInternal);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/UpgradeNickname", ReplyAction = "http://tempuri.org/IAutoJTService/UpgradeNicknameResponse")]
    Task<bool> UpgradeNicknameAsync(string uuid, string newNickname, bool isInternal);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/UpdateUserPaymentData", ReplyAction = "http://tempuri.org/IAutoJTService/UpdateUserPaymentDataResponse")]
    UpdateUserPaymentDataResponse UpdateUserPaymentData(UpdateUserPaymentDataRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/UpdateUserPaymentData", ReplyAction = "http://tempuri.org/IAutoJTService/UpdateUserPaymentDataResponse")]
    Task<UpdateUserPaymentDataResponse> UpdateUserPaymentDataAsync(
      UpdateUserPaymentDataRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/QueryAllComboMealInfo", ReplyAction = "http://tempuri.org/IAutoJTService/QueryAllComboMealInfoResponse")]
    QueryAllComboMealInfoResponse QueryAllComboMealInfo(QueryAllComboMealInfoRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/QueryAllComboMealInfo", ReplyAction = "http://tempuri.org/IAutoJTService/QueryAllComboMealInfoResponse")]
    Task<QueryAllComboMealInfoResponse> QueryAllComboMealInfoAsync(
      QueryAllComboMealInfoRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/CheckUserOwnerVersion", ReplyAction = "http://tempuri.org/IAutoJTService/CheckUserOwnerVersionResponse")]
    CheckUserOwnerVersionResponse CheckUserOwnerVersion(CheckUserOwnerVersionRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/CheckUserOwnerVersion", ReplyAction = "http://tempuri.org/IAutoJTService/CheckUserOwnerVersionResponse")]
    Task<CheckUserOwnerVersionResponse> CheckUserOwnerVersionAsync(
      CheckUserOwnerVersionRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetUserAllEnableVersion", ReplyAction = "http://tempuri.org/IAutoJTService/GetUserAllEnableVersionResponse")]
    string GetUserAllEnableVersion(string uuid);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetUserAllEnableVersion", ReplyAction = "http://tempuri.org/IAutoJTService/GetUserAllEnableVersionResponse")]
    Task<string> GetUserAllEnableVersionAsync(string uuid);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetData", ReplyAction = "http://tempuri.org/IAutoJTService/GetDataResponse")]
    string GetData(int value);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetData", ReplyAction = "http://tempuri.org/IAutoJTService/GetDataResponse")]
    Task<string> GetDataAsync(int value);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetDataUsingDataContract", ReplyAction = "http://tempuri.org/IAutoJTService/GetDataUsingDataContractResponse")]
    CompositeType GetDataUsingDataContract(CompositeType composite);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetDataUsingDataContract", ReplyAction = "http://tempuri.org/IAutoJTService/GetDataUsingDataContractResponse")]
    Task<CompositeType> GetDataUsingDataContractAsync(CompositeType composite);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetCurrentClientLoginStatus", ReplyAction = "http://tempuri.org/IAutoJTService/GetCurrentClientLoginStatusResponse")]
    bool GetCurrentClientLoginStatus(string userName, string clientInfos, string openID);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetCurrentClientLoginStatus", ReplyAction = "http://tempuri.org/IAutoJTService/GetCurrentClientLoginStatusResponse")]
    Task<bool> GetCurrentClientLoginStatusAsync(string userName, string clientInfos, string openID);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetCurrentMachineLoginCode", ReplyAction = "http://tempuri.org/IAutoJTService/GetCurrentMachineLoginCodeResponse")]
    GetCurrentMachineLoginCodeResponse GetCurrentMachineLoginCode(
      GetCurrentMachineLoginCodeRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetCurrentMachineLoginCode", ReplyAction = "http://tempuri.org/IAutoJTService/GetCurrentMachineLoginCodeResponse")]
    Task<GetCurrentMachineLoginCodeResponse> GetCurrentMachineLoginCodeAsync(
      GetCurrentMachineLoginCodeRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetLoginPopular", ReplyAction = "http://tempuri.org/IAutoJTService/GetLoginPopularResponse")]
    string GetLoginPopular();

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetLoginPopular", ReplyAction = "http://tempuri.org/IAutoJTService/GetLoginPopularResponse")]
    Task<string> GetLoginPopularAsync();

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/QueryVersionDesc4ModuleID", ReplyAction = "http://tempuri.org/IAutoJTService/QueryVersionDesc4ModuleIDResponse")]
    string QueryVersionDesc4ModuleID(string moduleID);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/QueryVersionDesc4ModuleID", ReplyAction = "http://tempuri.org/IAutoJTService/QueryVersionDesc4ModuleIDResponse")]
    Task<string> QueryVersionDesc4ModuleIDAsync(string moduleID);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/ValidateAuth", ReplyAction = "http://tempuri.org/IAutoJTService/ValidateAuthResponse")]
    bool ValidateAuth(string txtCode);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/ValidateAuth", ReplyAction = "http://tempuri.org/IAutoJTService/ValidateAuthResponse")]
    Task<bool> ValidateAuthAsync(string txtCode);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/ExitLoginMethod", ReplyAction = "http://tempuri.org/IAutoJTService/ExitLoginMethodResponse")]
    int ExitLoginMethod(string uuid);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/ExitLoginMethod", ReplyAction = "http://tempuri.org/IAutoJTService/ExitLoginMethodResponse")]
    Task<int> ExitLoginMethodAsync(string uuid);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetTableFromDatabase", ReplyAction = "http://tempuri.org/IAutoJTService/GetTableFromDatabaseResponse")]
    DataTable GetTableFromDatabase(string dataBaseTable);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetTableFromDatabase", ReplyAction = "http://tempuri.org/IAutoJTService/GetTableFromDatabaseResponse")]
    Task<DataTable> GetTableFromDatabaseAsync(string dataBaseTable);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetUserFromDatabase", ReplyAction = "http://tempuri.org/IAutoJTService/GetUserFromDatabaseResponse")]
    GetUserFromDatabaseResponse GetUserFromDatabase(GetUserFromDatabaseRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetUserFromDatabase", ReplyAction = "http://tempuri.org/IAutoJTService/GetUserFromDatabaseResponse")]
    Task<GetUserFromDatabaseResponse> GetUserFromDatabaseAsync(GetUserFromDatabaseRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetUserFromDatabase2", ReplyAction = "http://tempuri.org/IAutoJTService/GetUserFromDatabase2Response")]
    GetUserFromDatabase2Response GetUserFromDatabase2(GetUserFromDatabase2Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetUserFromDatabase2", ReplyAction = "http://tempuri.org/IAutoJTService/GetUserFromDatabase2Response")]
    Task<GetUserFromDatabase2Response> GetUserFromDatabase2Async(GetUserFromDatabase2Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetUserFromDatabase_wechat", ReplyAction = "http://tempuri.org/IAutoJTService/GetUserFromDatabase_wechatResponse")]
    void GetUserFromDatabase_wechat(
      string category,
      string userlnfos,
      string clientVersion,
      string username);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetUserFromDatabase_wechat", ReplyAction = "http://tempuri.org/IAutoJTService/GetUserFromDatabase_wechatResponse")]
    Task GetUserFromDatabase_wechatAsync(
      string category,
      string userlnfos,
      string clientVersion,
      string username);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetUserFromDatabase_wechat2", ReplyAction = "http://tempuri.org/IAutoJTService/GetUserFromDatabase_wechat2Response")]
    void GetUserFromDatabase_wechat2(
      string category,
      string userlnfos,
      string clientVersion,
      string username,
      string engineeringSoftInfos);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetUserFromDatabase_wechat2", ReplyAction = "http://tempuri.org/IAutoJTService/GetUserFromDatabase_wechat2Response")]
    Task GetUserFromDatabase_wechat2Async(
      string category,
      string userlnfos,
      string clientVersion,
      string username,
      string engineeringSoftInfos);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/DeleteItemData", ReplyAction = "http://tempuri.org/IAutoJTService/DeleteItemDataResponse")]
    string DeleteItemData(string dataBaseTable, string item);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/DeleteItemData", ReplyAction = "http://tempuri.org/IAutoJTService/DeleteItemDataResponse")]
    Task<string> DeleteItemDataAsync(string dataBaseTable, string item);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/DeleteItemData2", ReplyAction = "http://tempuri.org/IAutoJTService/DeleteItemData2Response")]
    string DeleteItemData2(string dataBaseTable, string item);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/DeleteItemData2", ReplyAction = "http://tempuri.org/IAutoJTService/DeleteItemData2Response")]
    Task<string> DeleteItemData2Async(string dataBaseTable, string item);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/InsertByDataTable", ReplyAction = "http://tempuri.org/IAutoJTService/InsertByDataTableResponse")]
    string InsertByDataTable(DataTable dataTable);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/InsertByDataTable", ReplyAction = "http://tempuri.org/IAutoJTService/InsertByDataTableResponse")]
    Task<string> InsertByDataTableAsync(DataTable dataTable);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/ImportTableToDB1", ReplyAction = "http://tempuri.org/IAutoJTService/ImportTableToDB1Response")]
    ImportTableToDB1Response ImportTableToDB1(ImportTableToDB1Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/ImportTableToDB1", ReplyAction = "http://tempuri.org/IAutoJTService/ImportTableToDB1Response")]
    Task<ImportTableToDB1Response> ImportTableToDB1Async(ImportTableToDB1Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/ImportTableToDB2", ReplyAction = "http://tempuri.org/IAutoJTService/ImportTableToDB2Response")]
    ImportTableToDB2Response ImportTableToDB2(ImportTableToDB2Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/ImportTableToDB2", ReplyAction = "http://tempuri.org/IAutoJTService/ImportTableToDB2Response")]
    Task<ImportTableToDB2Response> ImportTableToDB2Async(ImportTableToDB2Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetSqlDataReader1", ReplyAction = "http://tempuri.org/IAutoJTService/GetSqlDataReader1Response")]
    string[] GetSqlDataReader1(string SqlString);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetSqlDataReader1", ReplyAction = "http://tempuri.org/IAutoJTService/GetSqlDataReader1Response")]
    Task<string[]> GetSqlDataReader1Async(string SqlString);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetRowsCount", ReplyAction = "http://tempuri.org/IAutoJTService/GetRowsCountResponse")]
    int GetRowsCount(string SqlString);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetRowsCount", ReplyAction = "http://tempuri.org/IAutoJTService/GetRowsCountResponse")]
    Task<int> GetRowsCountAsync(string SqlString);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/QueryUsernameFidle", ReplyAction = "http://tempuri.org/IAutoJTService/QueryUsernameFidleResponse")]
    bool QueryUsernameFidle(string username);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/QueryUsernameFidle", ReplyAction = "http://tempuri.org/IAutoJTService/QueryUsernameFidleResponse")]
    Task<bool> QueryUsernameFidleAsync(string username);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/CreateAccount", ReplyAction = "http://tempuri.org/IAutoJTService/CreateAccountResponse")]
    string CreateAccount(string username, string password, string email);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/CreateAccount", ReplyAction = "http://tempuri.org/IAutoJTService/CreateAccountResponse")]
    Task<string> CreateAccountAsync(string username, string password, string email);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/UpdateWchatNiceName", ReplyAction = "http://tempuri.org/IAutoJTService/UpdateWchatNiceNameResponse")]
    UpdateWchatNiceNameResponse UpdateWchatNiceName(UpdateWchatNiceNameRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/UpdateWchatNiceName", ReplyAction = "http://tempuri.org/IAutoJTService/UpdateWchatNiceNameResponse")]
    Task<UpdateWchatNiceNameResponse> UpdateWchatNiceNameAsync(UpdateWchatNiceNameRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/UpdateWchatNiceName_2Net6", ReplyAction = "http://tempuri.org/IAutoJTService/UpdateWchatNiceName_2Net6Response")]
    UpdateWchatNiceName_2Net6Response UpdateWchatNiceName_2Net6(
      UpdateWchatNiceName_2Net6Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/UpdateWchatNiceName_2Net6", ReplyAction = "http://tempuri.org/IAutoJTService/UpdateWchatNiceName_2Net6Response")]
    Task<UpdateWchatNiceName_2Net6Response> UpdateWchatNiceName_2Net6Async(
      UpdateWchatNiceName_2Net6Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/VerificationCodeValid", ReplyAction = "http://tempuri.org/IAutoJTService/VerificationCodeValidResponse")]
    bool VerificationCodeValid(string email, string Verification_code);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/VerificationCodeValid", ReplyAction = "http://tempuri.org/IAutoJTService/VerificationCodeValidResponse")]
    Task<bool> VerificationCodeValidAsync(string email, string Verification_code);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/SendVerifyCode", ReplyAction = "http://tempuri.org/IAutoJTService/SendVerifyCodeResponse")]
    bool SendVerifyCode(string strToMailbox);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/SendVerifyCode", ReplyAction = "http://tempuri.org/IAutoJTService/SendVerifyCodeResponse")]
    Task<bool> SendVerifyCodeAsync(string strToMailbox);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetAutoJTTecnomatixVersionCode", ReplyAction = "http://tempuri.org/IAutoJTService/GetAutoJTTecnomatixVersionCodeResponse")]
    GetAutoJTTecnomatixVersionCodeResponse GetAutoJTTecnomatixVersionCode(
      GetAutoJTTecnomatixVersionCodeRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetAutoJTTecnomatixVersionCode", ReplyAction = "http://tempuri.org/IAutoJTService/GetAutoJTTecnomatixVersionCodeResponse")]
    Task<GetAutoJTTecnomatixVersionCodeResponse> GetAutoJTTecnomatixVersionCodeAsync(
      GetAutoJTTecnomatixVersionCodeRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetAutoJTTecnomatixVersionCode2", ReplyAction = "http://tempuri.org/IAutoJTService/GetAutoJTTecnomatixVersionCode2Response")]
    GetAutoJTTecnomatixVersionCode2Response GetAutoJTTecnomatixVersionCode2(
      GetAutoJTTecnomatixVersionCode2Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetAutoJTTecnomatixVersionCode2", ReplyAction = "http://tempuri.org/IAutoJTService/GetAutoJTTecnomatixVersionCode2Response")]
    Task<GetAutoJTTecnomatixVersionCode2Response> GetAutoJTTecnomatixVersionCode2Async(
      GetAutoJTTecnomatixVersionCode2Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetAutoJTTecnomatixVersionCode3", ReplyAction = "http://tempuri.org/IAutoJTService/GetAutoJTTecnomatixVersionCode3Response")]
    GetAutoJTTecnomatixVersionCode3Response GetAutoJTTecnomatixVersionCode3(
      GetAutoJTTecnomatixVersionCode3Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetAutoJTTecnomatixVersionCode3", ReplyAction = "http://tempuri.org/IAutoJTService/GetAutoJTTecnomatixVersionCode3Response")]
    Task<GetAutoJTTecnomatixVersionCode3Response> GetAutoJTTecnomatixVersionCode3Async(
      GetAutoJTTecnomatixVersionCode3Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetAutoJTTasksVersionCode", ReplyAction = "http://tempuri.org/IAutoJTService/GetAutoJTTasksVersionCodeResponse")]
    GetAutoJTTasksVersionCodeResponse GetAutoJTTasksVersionCode(
      GetAutoJTTasksVersionCodeRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetAutoJTTasksVersionCode", ReplyAction = "http://tempuri.org/IAutoJTService/GetAutoJTTasksVersionCodeResponse")]
    Task<GetAutoJTTasksVersionCodeResponse> GetAutoJTTasksVersionCodeAsync(
      GetAutoJTTasksVersionCodeRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetAutoJTTecnomatixVersionCodeExternal", ReplyAction = "http://tempuri.org/IAutoJTService/GetAutoJTTecnomatixVersionCodeExternalResponse")]
    GetAutoJTTecnomatixVersionCodeExternalResponse GetAutoJTTecnomatixVersionCodeExternal(
      GetAutoJTTecnomatixVersionCodeExternalRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetAutoJTTecnomatixVersionCodeExternal", ReplyAction = "http://tempuri.org/IAutoJTService/GetAutoJTTecnomatixVersionCodeExternalResponse")]
    Task<GetAutoJTTecnomatixVersionCodeExternalResponse> GetAutoJTTecnomatixVersionCodeExternalAsync(
      GetAutoJTTecnomatixVersionCodeExternalRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetAutoJTTecnomatixVersionCodeExternal3", ReplyAction = "http://tempuri.org/IAutoJTService/GetAutoJTTecnomatixVersionCodeExternal3Response")]
    GetAutoJTTecnomatixVersionCodeExternal3Response GetAutoJTTecnomatixVersionCodeExternal3(
      GetAutoJTTecnomatixVersionCodeExternal3Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetAutoJTTecnomatixVersionCodeExternal3", ReplyAction = "http://tempuri.org/IAutoJTService/GetAutoJTTecnomatixVersionCodeExternal3Response")]
    Task<GetAutoJTTecnomatixVersionCodeExternal3Response> GetAutoJTTecnomatixVersionCodeExternal3Async(
      GetAutoJTTecnomatixVersionCodeExternal3Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/CreateTx_Project_MCM", ReplyAction = "http://tempuri.org/IAutoJTService/CreateTx_Project_MCMResponse")]
    CreateTx_Project_MCMResponse CreateTx_Project_MCM(CreateTx_Project_MCMRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/CreateTx_Project_MCM", ReplyAction = "http://tempuri.org/IAutoJTService/CreateTx_Project_MCMResponse")]
    Task<CreateTx_Project_MCMResponse> CreateTx_Project_MCMAsync(CreateTx_Project_MCMRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/CreateTx_Project_MCM_new", ReplyAction = "http://tempuri.org/IAutoJTService/CreateTx_Project_MCM_newResponse")]
    CreateTx_Project_MCM_newResponse CreateTx_Project_MCM_new(
      CreateTx_Project_MCM_newRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/CreateTx_Project_MCM_new", ReplyAction = "http://tempuri.org/IAutoJTService/CreateTx_Project_MCM_newResponse")]
    Task<CreateTx_Project_MCM_newResponse> CreateTx_Project_MCM_newAsync(
      CreateTx_Project_MCM_newRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/AddinProject_Model2DB_MCM", ReplyAction = "http://tempuri.org/IAutoJTService/AddinProject_Model2DB_MCMResponse")]
    AddinProject_Model2DB_MCMResponse AddinProject_Model2DB_MCM(
      AddinProject_Model2DB_MCMRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/AddinProject_Model2DB_MCM", ReplyAction = "http://tempuri.org/IAutoJTService/AddinProject_Model2DB_MCMResponse")]
    Task<AddinProject_Model2DB_MCMResponse> AddinProject_Model2DB_MCMAsync(
      AddinProject_Model2DB_MCMRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/AddinProject_Model2DB_MCM_new", ReplyAction = "http://tempuri.org/IAutoJTService/AddinProject_Model2DB_MCM_newResponse")]
    AddinProject_Model2DB_MCM_newResponse AddinProject_Model2DB_MCM_new(
      AddinProject_Model2DB_MCM_newRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/AddinProject_Model2DB_MCM_new", ReplyAction = "http://tempuri.org/IAutoJTService/AddinProject_Model2DB_MCM_newResponse")]
    Task<AddinProject_Model2DB_MCM_newResponse> AddinProject_Model2DB_MCM_newAsync(
      AddinProject_Model2DB_MCM_newRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/CreateTx_Project_Model_Lib_MCM", ReplyAction = "http://tempuri.org/IAutoJTService/CreateTx_Project_Model_Lib_MCMResponse")]
    CreateTx_Project_Model_Lib_MCMResponse CreateTx_Project_Model_Lib_MCM(
      CreateTx_Project_Model_Lib_MCMRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/CreateTx_Project_Model_Lib_MCM", ReplyAction = "http://tempuri.org/IAutoJTService/CreateTx_Project_Model_Lib_MCMResponse")]
    Task<CreateTx_Project_Model_Lib_MCMResponse> CreateTx_Project_Model_Lib_MCMAsync(
      CreateTx_Project_Model_Lib_MCMRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/Create_MCM_LibraryTable", ReplyAction = "http://tempuri.org/IAutoJTService/Create_MCM_LibraryTableResponse")]
    Create_MCM_LibraryTableResponse Create_MCM_LibraryTable(Create_MCM_LibraryTableRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/Create_MCM_LibraryTable", ReplyAction = "http://tempuri.org/IAutoJTService/Create_MCM_LibraryTableResponse")]
    Task<Create_MCM_LibraryTableResponse> Create_MCM_LibraryTableAsync(
      Create_MCM_LibraryTableRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/BulkInsertMCMData_MCM", ReplyAction = "http://tempuri.org/IAutoJTService/BulkInsertMCMData_MCMResponse")]
    BulkInsertMCMData_MCMResponse BulkInsertMCMData_MCM(BulkInsertMCMData_MCMRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/BulkInsertMCMData_MCM", ReplyAction = "http://tempuri.org/IAutoJTService/BulkInsertMCMData_MCMResponse")]
    Task<BulkInsertMCMData_MCMResponse> BulkInsertMCMData_MCMAsync(
      BulkInsertMCMData_MCMRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/AddinTx_Project_Model_Lib_MCM", ReplyAction = "http://tempuri.org/IAutoJTService/AddinTx_Project_Model_Lib_MCMResponse")]
    AddinTx_Project_Model_Lib_MCMResponse AddinTx_Project_Model_Lib_MCM(
      AddinTx_Project_Model_Lib_MCMRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/AddinTx_Project_Model_Lib_MCM", ReplyAction = "http://tempuri.org/IAutoJTService/AddinTx_Project_Model_Lib_MCMResponse")]
    Task<AddinTx_Project_Model_Lib_MCMResponse> AddinTx_Project_Model_Lib_MCMAsync(
      AddinTx_Project_Model_Lib_MCMRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/DeleteProject_1", ReplyAction = "http://tempuri.org/IAutoJTService/DeleteProject_1Response")]
    DeleteProject_1Response DeleteProject_1(DeleteProject_1Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/DeleteProject_1", ReplyAction = "http://tempuri.org/IAutoJTService/DeleteProject_1Response")]
    Task<DeleteProject_1Response> DeleteProject_1Async(DeleteProject_1Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/CreateTx_Project_Model_Users_MCM", ReplyAction = "http://tempuri.org/IAutoJTService/CreateTx_Project_Model_Users_MCMResponse")]
    CreateTx_Project_Model_Users_MCMResponse CreateTx_Project_Model_Users_MCM(
      CreateTx_Project_Model_Users_MCMRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/CreateTx_Project_Model_Users_MCM", ReplyAction = "http://tempuri.org/IAutoJTService/CreateTx_Project_Model_Users_MCMResponse")]
    Task<CreateTx_Project_Model_Users_MCMResponse> CreateTx_Project_Model_Users_MCMAsync(
      CreateTx_Project_Model_Users_MCMRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/AddinTx_Project_Model_Users_MCM", ReplyAction = "http://tempuri.org/IAutoJTService/AddinTx_Project_Model_Users_MCMResponse")]
    AddinTx_Project_Model_Users_MCMResponse AddinTx_Project_Model_Users_MCM(
      AddinTx_Project_Model_Users_MCMRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/AddinTx_Project_Model_Users_MCM", ReplyAction = "http://tempuri.org/IAutoJTService/AddinTx_Project_Model_Users_MCMResponse")]
    Task<AddinTx_Project_Model_Users_MCMResponse> AddinTx_Project_Model_Users_MCMAsync(
      AddinTx_Project_Model_Users_MCMRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/CreateAndBulkInsertUserMCMData_MCM", ReplyAction = "http://tempuri.org/IAutoJTService/CreateAndBulkInsertUserMCMData_MCMResponse")]
    CreateAndBulkInsertUserMCMData_MCMResponse CreateAndBulkInsertUserMCMData_MCM(
      CreateAndBulkInsertUserMCMData_MCMRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/CreateAndBulkInsertUserMCMData_MCM", ReplyAction = "http://tempuri.org/IAutoJTService/CreateAndBulkInsertUserMCMData_MCMResponse")]
    Task<CreateAndBulkInsertUserMCMData_MCMResponse> CreateAndBulkInsertUserMCMData_MCMAsync(
      CreateAndBulkInsertUserMCMData_MCMRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/EmptyTable_1", ReplyAction = "http://tempuri.org/IAutoJTService/EmptyTable_1Response")]
    string EmptyTable_1(string tableName);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/EmptyTable_1", ReplyAction = "http://tempuri.org/IAutoJTService/EmptyTable_1Response")]
    Task<string> EmptyTable_1Async(string tableName);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/EmptyTable_new", ReplyAction = "http://tempuri.org/IAutoJTService/EmptyTable_newResponse")]
    string EmptyTable_new(string tableName);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/EmptyTable_new", ReplyAction = "http://tempuri.org/IAutoJTService/EmptyTable_newResponse")]
    Task<string> EmptyTable_newAsync(string tableName);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetRowsCount_2", ReplyAction = "http://tempuri.org/IAutoJTService/GetRowsCount_2Response")]
    int GetRowsCount_2(string tbName);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetRowsCount_2", ReplyAction = "http://tempuri.org/IAutoJTService/GetRowsCount_2Response")]
    Task<int> GetRowsCount_2Async(string tbName);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/QueryJT_username", ReplyAction = "http://tempuri.org/IAutoJTService/QueryJT_usernameResponse")]
    string[] QueryJT_username();

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/QueryJT_username", ReplyAction = "http://tempuri.org/IAutoJTService/QueryJT_usernameResponse")]
    Task<string[]> QueryJT_usernameAsync();

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/Tx_Refresh_MCM", ReplyAction = "http://tempuri.org/IAutoJTService/Tx_Refresh_MCMResponse")]
    Tx_Refresh_MCMResponse Tx_Refresh_MCM(Tx_Refresh_MCMRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/Tx_Refresh_MCM", ReplyAction = "http://tempuri.org/IAutoJTService/Tx_Refresh_MCMResponse")]
    Task<Tx_Refresh_MCMResponse> Tx_Refresh_MCMAsync(Tx_Refresh_MCMRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/Tx_Refresh_MCM_new1", ReplyAction = "http://tempuri.org/IAutoJTService/Tx_Refresh_MCM_new1Response")]
    Tx_Refresh_MCM_new1Response Tx_Refresh_MCM_new1(Tx_Refresh_MCM_new1Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/Tx_Refresh_MCM_new1", ReplyAction = "http://tempuri.org/IAutoJTService/Tx_Refresh_MCM_new1Response")]
    Task<Tx_Refresh_MCM_new1Response> Tx_Refresh_MCM_new1Async(Tx_Refresh_MCM_new1Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/Query_ProjectModleExists_MCM", ReplyAction = "http://tempuri.org/IAutoJTService/Query_ProjectModleExists_MCMResponse")]
    bool Query_ProjectModleExists_MCM(string project_1, string model_1);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/Query_ProjectModleExists_MCM", ReplyAction = "http://tempuri.org/IAutoJTService/Query_ProjectModleExists_MCMResponse")]
    Task<bool> Query_ProjectModleExists_MCMAsync(string project_1, string model_1);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/Query_ProExists_uuid_MCM", ReplyAction = "http://tempuri.org/IAutoJTService/Query_ProExists_uuid_MCMResponse")]
    Query_ProExists_uuid_MCMResponse Query_ProExists_uuid_MCM(
      Query_ProExists_uuid_MCMRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/Query_ProExists_uuid_MCM", ReplyAction = "http://tempuri.org/IAutoJTService/Query_ProExists_uuid_MCMResponse")]
    Task<Query_ProExists_uuid_MCMResponse> Query_ProExists_uuid_MCMAsync(
      Query_ProExists_uuid_MCMRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/Query_AllPro4OpenID", ReplyAction = "http://tempuri.org/IAutoJTService/Query_AllPro4OpenIDResponse")]
    Query_AllPro4OpenIDResponse Query_AllPro4OpenID(Query_AllPro4OpenIDRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/Query_AllPro4OpenID", ReplyAction = "http://tempuri.org/IAutoJTService/Query_AllPro4OpenIDResponse")]
    Task<Query_AllPro4OpenIDResponse> Query_AllPro4OpenIDAsync(Query_AllPro4OpenIDRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/Query_UsersTakeProjectsOpenID", ReplyAction = "http://tempuri.org/IAutoJTService/Query_UsersTakeProjectsOpenIDResponse")]
    Query_UsersTakeProjectsOpenIDResponse Query_UsersTakeProjectsOpenID(
      Query_UsersTakeProjectsOpenIDRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/Query_UsersTakeProjectsOpenID", ReplyAction = "http://tempuri.org/IAutoJTService/Query_UsersTakeProjectsOpenIDResponse")]
    Task<Query_UsersTakeProjectsOpenIDResponse> Query_UsersTakeProjectsOpenIDAsync(
      Query_UsersTakeProjectsOpenIDRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/Query_UserNameByOpenID", ReplyAction = "http://tempuri.org/IAutoJTService/Query_UserNameByOpenIDResponse")]
    Query_UserNameByOpenIDResponse Query_UserNameByOpenID(Query_UserNameByOpenIDRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/Query_UserNameByOpenID", ReplyAction = "http://tempuri.org/IAutoJTService/Query_UserNameByOpenIDResponse")]
    Task<Query_UserNameByOpenIDResponse> Query_UserNameByOpenIDAsync(
      Query_UserNameByOpenIDRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/Module_usage_statistics", ReplyAction = "http://tempuri.org/IAutoJTService/Module_usage_statisticsResponse")]
    [return: MessageParameter(Name = "exInfos")]
    string Module_usage_statistics(DataTable modelInfos);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/Module_usage_statistics", ReplyAction = "http://tempuri.org/IAutoJTService/Module_usage_statisticsResponse")]
    [return: MessageParameter(Name = "exInfos")]
    Task<string> Module_usage_statisticsAsync(DataTable modelInfos);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/Module_usage_statistics_21", ReplyAction = "http://tempuri.org/IAutoJTService/Module_usage_statistics_21Response")]
    [return: MessageParameter(Name = "exInfos")]
    string Module_usage_statistics_21(DataTable modelInfos);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/Module_usage_statistics_21", ReplyAction = "http://tempuri.org/IAutoJTService/Module_usage_statistics_21Response")]
    [return: MessageParameter(Name = "exInfos")]
    Task<string> Module_usage_statistics_21Async(DataTable modelInfos);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/Module_usage_statistics2", ReplyAction = "http://tempuri.org/IAutoJTService/Module_usage_statistics2Response")]
    [ServiceKnownType(typeof (string[]))]
    [ServiceKnownType(typeof (object[]))]
    [ServiceKnownType(typeof (double[]))]
    [ServiceKnownType(typeof (DataTable[]))]
    [ServiceKnownType(typeof (CompositeType))]
    [return: MessageParameter(Name = "exInfos")]
    string Module_usage_statistics2(object[] modelInfos);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/Module_usage_statistics2", ReplyAction = "http://tempuri.org/IAutoJTService/Module_usage_statistics2Response")]
    [return: MessageParameter(Name = "exInfos")]
    Task<string> Module_usage_statistics2Async(object[] modelInfos);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/CalcWD_Rotation_4", ReplyAction = "http://tempuri.org/IAutoJTService/CalcWD_Rotation_4Response")]
    CalcWD_Rotation_4Response CalcWD_Rotation_4(CalcWD_Rotation_4Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/CalcWD_Rotation_4", ReplyAction = "http://tempuri.org/IAutoJTService/CalcWD_Rotation_4Response")]
    Task<CalcWD_Rotation_4Response> CalcWD_Rotation_4Async(CalcWD_Rotation_4Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/CalcWD_Rotation_5", ReplyAction = "http://tempuri.org/IAutoJTService/CalcWD_Rotation_5Response")]
    CalcWD_Rotation_5Response CalcWD_Rotation_5(CalcWD_Rotation_5Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/CalcWD_Rotation_5", ReplyAction = "http://tempuri.org/IAutoJTService/CalcWD_Rotation_5Response")]
    Task<CalcWD_Rotation_5Response> CalcWD_Rotation_5Async(CalcWD_Rotation_5Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/CalcVIA_Rotation_4", ReplyAction = "http://tempuri.org/IAutoJTService/CalcVIA_Rotation_4Response")]
    CalcVIA_Rotation_4Response CalcVIA_Rotation_4(CalcVIA_Rotation_4Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/CalcVIA_Rotation_4", ReplyAction = "http://tempuri.org/IAutoJTService/CalcVIA_Rotation_4Response")]
    Task<CalcVIA_Rotation_4Response> CalcVIA_Rotation_4Async(CalcVIA_Rotation_4Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/Calc_RPY2Matrix_Transform", ReplyAction = "http://tempuri.org/IAutoJTService/Calc_RPY2Matrix_TransformResponse")]
    Calc_RPY2Matrix_TransformResponse Calc_RPY2Matrix_Transform(
      Calc_RPY2Matrix_TransformRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/Calc_RPY2Matrix_Transform", ReplyAction = "http://tempuri.org/IAutoJTService/Calc_RPY2Matrix_TransformResponse")]
    Task<Calc_RPY2Matrix_TransformResponse> Calc_RPY2Matrix_TransformAsync(
      Calc_RPY2Matrix_TransformRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/Calc_RPY2Matrix_Transform2", ReplyAction = "http://tempuri.org/IAutoJTService/Calc_RPY2Matrix_Transform2Response")]
    Calc_RPY2Matrix_Transform2Response Calc_RPY2Matrix_Transform2(
      Calc_RPY2Matrix_Transform2Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/Calc_RPY2Matrix_Transform2", ReplyAction = "http://tempuri.org/IAutoJTService/Calc_RPY2Matrix_Transform2Response")]
    Task<Calc_RPY2Matrix_Transform2Response> Calc_RPY2Matrix_Transform2Async(
      Calc_RPY2Matrix_Transform2Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/CalcNoteQuadrant", ReplyAction = "http://tempuri.org/IAutoJTService/CalcNoteQuadrantResponse")]
    [ServiceKnownType(typeof (string[]))]
    [ServiceKnownType(typeof (object[]))]
    [ServiceKnownType(typeof (double[]))]
    [ServiceKnownType(typeof (DataTable[]))]
    [ServiceKnownType(typeof (CompositeType))]
    CalcNoteQuadrantResponse CalcNoteQuadrant(CalcNoteQuadrantRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/CalcNoteQuadrant", ReplyAction = "http://tempuri.org/IAutoJTService/CalcNoteQuadrantResponse")]
    Task<CalcNoteQuadrantResponse> CalcNoteQuadrantAsync(CalcNoteQuadrantRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/CalcRealBoxLocation", ReplyAction = "http://tempuri.org/IAutoJTService/CalcRealBoxLocationResponse")]
    CalcRealBoxLocationResponse CalcRealBoxLocation(CalcRealBoxLocationRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/CalcRealBoxLocation", ReplyAction = "http://tempuri.org/IAutoJTService/CalcRealBoxLocationResponse")]
    Task<CalcRealBoxLocationResponse> CalcRealBoxLocationAsync(CalcRealBoxLocationRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/MultiCalcRealBoxLocation", ReplyAction = "http://tempuri.org/IAutoJTService/MultiCalcRealBoxLocationResponse")]
    MultiCalcRealBoxLocationResponse MultiCalcRealBoxLocation(
      MultiCalcRealBoxLocationRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/MultiCalcRealBoxLocation", ReplyAction = "http://tempuri.org/IAutoJTService/MultiCalcRealBoxLocationResponse")]
    Task<MultiCalcRealBoxLocationResponse> MultiCalcRealBoxLocationAsync(
      MultiCalcRealBoxLocationRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/ReCalcRotaFrme", ReplyAction = "http://tempuri.org/IAutoJTService/ReCalcRotaFrmeResponse")]
    double[] ReCalcRotaFrme(
      double[] p1_word,
      double[] p2_word,
      double[] p11_word,
      double[] p21_word);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/ReCalcRotaFrme", ReplyAction = "http://tempuri.org/IAutoJTService/ReCalcRotaFrmeResponse")]
    Task<double[]> ReCalcRotaFrmeAsync(
      double[] p1_word,
      double[] p2_word,
      double[] p11_word,
      double[] p21_word);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetAutoJTVersionCode", ReplyAction = "http://tempuri.org/IAutoJTService/GetAutoJTVersionCodeResponse")]
    GetAutoJTVersionCodeResponse GetAutoJTVersionCode(GetAutoJTVersionCodeRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetAutoJTVersionCode", ReplyAction = "http://tempuri.org/IAutoJTService/GetAutoJTVersionCodeResponse")]
    Task<GetAutoJTVersionCodeResponse> GetAutoJTVersionCodeAsync(GetAutoJTVersionCodeRequest request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetAutoJTVersionCode2", ReplyAction = "http://tempuri.org/IAutoJTService/GetAutoJTVersionCode2Response")]
    GetAutoJTVersionCode2Response GetAutoJTVersionCode2(GetAutoJTVersionCode2Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetAutoJTVersionCode2", ReplyAction = "http://tempuri.org/IAutoJTService/GetAutoJTVersionCode2Response")]
    Task<GetAutoJTVersionCode2Response> GetAutoJTVersionCode2Async(
      GetAutoJTVersionCode2Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetAutoJTVersionCodeExternal3", ReplyAction = "http://tempuri.org/IAutoJTService/GetAutoJTVersionCodeExternal3Response")]
    GetAutoJTVersionCodeExternal3Response GetAutoJTVersionCodeExternal3(
      GetAutoJTVersionCodeExternal3Request request);

    [OperationContract(Action = "http://tempuri.org/IAutoJTService/GetAutoJTVersionCodeExternal3", ReplyAction = "http://tempuri.org/IAutoJTService/GetAutoJTVersionCodeExternal3Response")]
    Task<GetAutoJTVersionCodeExternal3Response> GetAutoJTVersionCodeExternal3Async(
      GetAutoJTVersionCodeExternal3Request request);
  }
}
