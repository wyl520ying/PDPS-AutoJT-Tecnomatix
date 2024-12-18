





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "CalcWD_Rotation_5Response", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class CalcWD_Rotation_5Response
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public double[] CalcWD_Rotation_5Result;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string exInfos;

    public CalcWD_Rotation_5Response()
    {
    }

    public CalcWD_Rotation_5Response(double[] CalcWD_Rotation_5Result, string exInfos)
    {
      this.CalcWD_Rotation_5Result = CalcWD_Rotation_5Result;
      this.exInfos = exInfos;
    }
  }
}
