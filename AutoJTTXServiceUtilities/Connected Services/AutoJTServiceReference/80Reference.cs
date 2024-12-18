





using System.CodeDom.Compiler;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "Calc_RPY2Matrix_TransformResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class Calc_RPY2Matrix_TransformResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public DataTable Calc_RPY2Matrix_TransformResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string exInfos;

    public Calc_RPY2Matrix_TransformResponse()
    {
    }

    public Calc_RPY2Matrix_TransformResponse(
      DataTable Calc_RPY2Matrix_TransformResult,
      string exInfos)
    {
      this.Calc_RPY2Matrix_TransformResult = Calc_RPY2Matrix_TransformResult;
      this.exInfos = exInfos;
    }
  }
}
