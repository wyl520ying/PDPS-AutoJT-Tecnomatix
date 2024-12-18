





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "GetUnionIDNickName4ClientInfosResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class GetUnionIDNickName4ClientInfosResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public bool GetUnionIDNickName4ClientInfosResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string internal_tag;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public string expiryDate;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 3)]
    public string unionId;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 4)]
    public string nickName;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 5)]
    public string regId;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 6)]
    public string version_desc;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 7)]
    public string[] moduleIDList;

    public GetUnionIDNickName4ClientInfosResponse()
    {
    }

    public GetUnionIDNickName4ClientInfosResponse(
      bool GetUnionIDNickName4ClientInfosResult,
      string internal_tag,
      string expiryDate,
      string unionId,
      string nickName,
      string regId,
      string version_desc,
      string[] moduleIDList)
    {
      this.GetUnionIDNickName4ClientInfosResult = GetUnionIDNickName4ClientInfosResult;
      this.internal_tag = internal_tag;
      this.expiryDate = expiryDate;
      this.unionId = unionId;
      this.nickName = nickName;
      this.regId = regId;
      this.version_desc = version_desc;
      this.moduleIDList = moduleIDList;
    }
  }
}
