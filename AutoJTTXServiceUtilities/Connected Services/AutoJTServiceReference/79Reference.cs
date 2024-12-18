





using System.CodeDom.Compiler;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "Calc_RPY2Matrix_Transform", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class Calc_RPY2Matrix_TransformRequest
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public DataTable dataTable1;

    public Calc_RPY2Matrix_TransformRequest()
    {
    }

    public Calc_RPY2Matrix_TransformRequest(DataTable dataTable1) => this.dataTable1 = dataTable1;
  }
}
