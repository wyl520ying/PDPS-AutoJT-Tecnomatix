





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "CheckUserOwnerVersionResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class CheckUserOwnerVersionResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public bool CheckUserOwnerVersionResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public int? ouwnerVer;

    public CheckUserOwnerVersionResponse()
    {
    }

    public CheckUserOwnerVersionResponse(bool CheckUserOwnerVersionResult, int? ouwnerVer)
    {
      this.CheckUserOwnerVersionResult = CheckUserOwnerVersionResult;
      this.ouwnerVer = ouwnerVer;
    }
  }
}
