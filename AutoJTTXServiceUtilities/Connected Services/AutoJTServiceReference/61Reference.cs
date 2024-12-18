





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "Tx_Refresh_MCM", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class Tx_Refresh_MCMRequest
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string project;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string model;

    public Tx_Refresh_MCMRequest()
    {
    }

    public Tx_Refresh_MCMRequest(string project, string model)
    {
      this.project = project;
      this.model = model;
    }
  }
}
