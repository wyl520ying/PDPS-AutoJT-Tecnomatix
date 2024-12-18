





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "CalcVIA_Rotation_4Response", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class CalcVIA_Rotation_4Response
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public double[] CalcVIA_Rotation_4Result;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string exInfos;

    public CalcVIA_Rotation_4Response()
    {
    }

    public CalcVIA_Rotation_4Response(double[] CalcVIA_Rotation_4Result, string exInfos)
    {
      this.CalcVIA_Rotation_4Result = CalcVIA_Rotation_4Result;
      this.exInfos = exInfos;
    }
  }
}
