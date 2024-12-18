





using System.CodeDom.Compiler;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "Calc_RPY2Matrix_Transform2Response", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class Calc_RPY2Matrix_Transform2Response
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public DataTable Calc_RPY2Matrix_Transform2Result;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string exInfos;

    public Calc_RPY2Matrix_Transform2Response()
    {
    }

    public Calc_RPY2Matrix_Transform2Response(
      DataTable Calc_RPY2Matrix_Transform2Result,
      string exInfos)
    {
      this.Calc_RPY2Matrix_Transform2Result = Calc_RPY2Matrix_Transform2Result;
      this.exInfos = exInfos;
    }
  }
}
