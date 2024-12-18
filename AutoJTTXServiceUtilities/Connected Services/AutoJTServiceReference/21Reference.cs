





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "UpdateWchatNiceName", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class UpdateWchatNiceNameRequest
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string username;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string email;

    public UpdateWchatNiceNameRequest()
    {
    }

    public UpdateWchatNiceNameRequest(string username, string email)
    {
      this.username = username;
      this.email = email;
    }
  }
}
