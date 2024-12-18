





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "MultiCalcRealBoxLocation", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class MultiCalcRealBoxLocationRequest
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public double[] txFrameCenter_1;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public double[] leftLower_1;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public double[] rightUpper_1;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 3)]
    public double[] txVector_ora_1;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 4)]
    public double[] txFrameCenter_2;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 5)]
    public double[] leftLower_2;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 6)]
    public double[] rightUpper_2;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 7)]
    public double[] txVector_ora_2;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 8)]
    public bool isCalcWordLoc_1;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 9)]
    public bool isCalcWordLoc_2;

    public MultiCalcRealBoxLocationRequest()
    {
    }

    public MultiCalcRealBoxLocationRequest(
      double[] txFrameCenter_1,
      double[] leftLower_1,
      double[] rightUpper_1,
      double[] txVector_ora_1,
      double[] txFrameCenter_2,
      double[] leftLower_2,
      double[] rightUpper_2,
      double[] txVector_ora_2,
      bool isCalcWordLoc_1,
      bool isCalcWordLoc_2)
    {
      this.txFrameCenter_1 = txFrameCenter_1;
      this.leftLower_1 = leftLower_1;
      this.rightUpper_1 = rightUpper_1;
      this.txVector_ora_1 = txVector_ora_1;
      this.txFrameCenter_2 = txFrameCenter_2;
      this.leftLower_2 = leftLower_2;
      this.rightUpper_2 = rightUpper_2;
      this.txVector_ora_2 = txVector_ora_2;
      this.isCalcWordLoc_1 = isCalcWordLoc_1;
      this.isCalcWordLoc_2 = isCalcWordLoc_2;
    }
  }
}
