





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "UserAuthenticationResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class UserAuthenticationResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public bool UserAuthenticationResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string internal_tag;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public string expiryDate;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 3)]
    public string version_desc;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 4)]
    public string[] moduleIDList;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 5)]
    public string regid;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 6)]
    public string nickNameResout;

    public UserAuthenticationResponse()
    {
    }

    public UserAuthenticationResponse(
      bool UserAuthenticationResult,
      string internal_tag,
      string expiryDate,
      string version_desc,
      string[] moduleIDList,
      string regid,
      string nickNameResout)
    {
      this.UserAuthenticationResult = UserAuthenticationResult;
      this.internal_tag = internal_tag;
      this.expiryDate = expiryDate;
      this.version_desc = version_desc;
      this.moduleIDList = moduleIDList;
      this.regid = regid;
      this.nickNameResout = nickNameResout;
    }
  }
}
