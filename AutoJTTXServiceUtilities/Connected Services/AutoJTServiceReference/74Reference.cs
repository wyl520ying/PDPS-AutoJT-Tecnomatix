





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "CalcWD_Rotation_4Response", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class CalcWD_Rotation_4Response
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public double[] CalcWD_Rotation_4Result;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string exInfos;

    public CalcWD_Rotation_4Response()
    {
    }

    public CalcWD_Rotation_4Response(double[] CalcWD_Rotation_4Result, string exInfos)
    {
      this.CalcWD_Rotation_4Result = CalcWD_Rotation_4Result;
      this.exInfos = exInfos;
    }
  }
}
