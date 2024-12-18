





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "Tx_Refresh_MCM_new1", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class Tx_Refresh_MCM_new1Request
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string uuid;

    public Tx_Refresh_MCM_new1Request()
    {
    }

    public Tx_Refresh_MCM_new1Request(string uuid) => this.uuid = uuid;
  }
}
