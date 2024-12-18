





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "CalcRealBoxLocation", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class CalcRealBoxLocationRequest
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public double[] txFrameCenter;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public double[] leftLower;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public double[] rightUpper;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 3)]
    public double[] txVector_ora;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 4)]
    public bool isCalcWordLoc;

    public CalcRealBoxLocationRequest()
    {
    }

    public CalcRealBoxLocationRequest(
      double[] txFrameCenter,
      double[] leftLower,
      double[] rightUpper,
      double[] txVector_ora,
      bool isCalcWordLoc)
    {
      this.txFrameCenter = txFrameCenter;
      this.leftLower = leftLower;
      this.rightUpper = rightUpper;
      this.txVector_ora = txVector_ora;
      this.isCalcWordLoc = isCalcWordLoc;
    }
  }
}
