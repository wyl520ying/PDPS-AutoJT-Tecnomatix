





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "CheckUserOwnerVersion", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class CheckUserOwnerVersionRequest
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string unionID;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string combo_meal_code;

    public CheckUserOwnerVersionRequest()
    {
    }

    public CheckUserOwnerVersionRequest(string unionID, string combo_meal_code)
    {
      this.unionID = unionID;
      this.combo_meal_code = combo_meal_code;
    }
  }
}
