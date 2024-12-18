





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "GetUnionIDNickName4ClientInfos", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class GetUnionIDNickName4ClientInfosRequest
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public bool isInternal;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string category;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public string userlnfos;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 3)]
    public string clientInfos;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 4)]
    public string clientVersion;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 5)]
    public string softHostVersion;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 6)]
    public string ipAddress;

    public GetUnionIDNickName4ClientInfosRequest()
    {
    }

    public GetUnionIDNickName4ClientInfosRequest(
      bool isInternal,
      string category,
      string userlnfos,
      string clientInfos,
      string clientVersion,
      string softHostVersion,
      string ipAddress)
    {
      this.isInternal = isInternal;
      this.category = category;
      this.userlnfos = userlnfos;
      this.clientInfos = clientInfos;
      this.clientVersion = clientVersion;
      this.softHostVersion = softHostVersion;
      this.ipAddress = ipAddress;
    }
  }
}
