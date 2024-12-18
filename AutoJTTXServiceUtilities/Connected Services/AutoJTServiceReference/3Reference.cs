





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "UserAuthentication", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class UserAuthenticationRequest
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string unionid;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string nickName;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public bool isInternal;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 3)]
    public string category;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 4)]
    public string userlnfos;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 5)]
    public string clientVersion;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 6)]
    public string softHostVersion;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 7)]
    public string ipAddress;

    public UserAuthenticationRequest()
    {
    }

    public UserAuthenticationRequest(
      string unionid,
      string nickName,
      bool isInternal,
      string category,
      string userlnfos,
      string clientVersion,
      string softHostVersion,
      string ipAddress)
    {
      this.unionid = unionid;
      this.nickName = nickName;
      this.isInternal = isInternal;
      this.category = category;
      this.userlnfos = userlnfos;
      this.clientVersion = clientVersion;
      this.softHostVersion = softHostVersion;
      this.ipAddress = ipAddress;
    }
  }
}
