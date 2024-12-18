





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "CreateTx_Project_Model_Users_MCM", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class CreateTx_Project_Model_Users_MCMRequest
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string tbName;

    public CreateTx_Project_Model_Users_MCMRequest()
    {
    }

    public CreateTx_Project_Model_Users_MCMRequest(string tbName) => this.tbName = tbName;
  }
}
