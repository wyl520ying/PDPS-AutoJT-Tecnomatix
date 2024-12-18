





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "MultiCalcRealBoxLocationResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class MultiCalcRealBoxLocationResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public bool MultiCalcRealBoxLocationResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public double[] bottomLeftLower_1;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public double[] bottomLeftUpper_1;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 3)]
    public double[] bottomRightLower_1;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 4)]
    public double[] bottomRightUpper_1;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 5)]
    public double[] topLeftLower_1;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 6)]
    public double[] topLeftUpper_1;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 7)]
    public double[] topRightLower_1;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 8)]
    public double[] topRightUpper_1;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 9)]
    public double[] pp0_1;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 10)]
    public double[] p1_word_1;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 11)]
    public double[] p2_word_1;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 12)]
    public double[] bottomLeftLower_2;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 13)]
    public double[] bottomLeftUpper_2;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 14)]
    public double[] bottomRightLower_2;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 15)]
    public double[] bottomRightUpper_2;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 16)]
    public double[] topLeftLower_2;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 17)]
    public double[] topLeftUpper_2;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 18)]
    public double[] topRightLower_2;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 19)]
    public double[] topRightUpper_2;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 20)]
    public double[] pp0_2;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 21)]
    public double[] p1_word_2;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 22)]
    public double[] p2_word_2;

    public MultiCalcRealBoxLocationResponse()
    {
    }

    public MultiCalcRealBoxLocationResponse(
      bool MultiCalcRealBoxLocationResult,
      double[] bottomLeftLower_1,
      double[] bottomLeftUpper_1,
      double[] bottomRightLower_1,
      double[] bottomRightUpper_1,
      double[] topLeftLower_1,
      double[] topLeftUpper_1,
      double[] topRightLower_1,
      double[] topRightUpper_1,
      double[] pp0_1,
      double[] p1_word_1,
      double[] p2_word_1,
      double[] bottomLeftLower_2,
      double[] bottomLeftUpper_2,
      double[] bottomRightLower_2,
      double[] bottomRightUpper_2,
      double[] topLeftLower_2,
      double[] topLeftUpper_2,
      double[] topRightLower_2,
      double[] topRightUpper_2,
      double[] pp0_2,
      double[] p1_word_2,
      double[] p2_word_2)
    {
      this.MultiCalcRealBoxLocationResult = MultiCalcRealBoxLocationResult;
      this.bottomLeftLower_1 = bottomLeftLower_1;
      this.bottomLeftUpper_1 = bottomLeftUpper_1;
      this.bottomRightLower_1 = bottomRightLower_1;
      this.bottomRightUpper_1 = bottomRightUpper_1;
      this.topLeftLower_1 = topLeftLower_1;
      this.topLeftUpper_1 = topLeftUpper_1;
      this.topRightLower_1 = topRightLower_1;
      this.topRightUpper_1 = topRightUpper_1;
      this.pp0_1 = pp0_1;
      this.p1_word_1 = p1_word_1;
      this.p2_word_1 = p2_word_1;
      this.bottomLeftLower_2 = bottomLeftLower_2;
      this.bottomLeftUpper_2 = bottomLeftUpper_2;
      this.bottomRightLower_2 = bottomRightLower_2;
      this.bottomRightUpper_2 = bottomRightUpper_2;
      this.topLeftLower_2 = topLeftLower_2;
      this.topLeftUpper_2 = topLeftUpper_2;
      this.topRightLower_2 = topRightLower_2;
      this.topRightUpper_2 = topRightUpper_2;
      this.pp0_2 = pp0_2;
      this.p1_word_2 = p1_word_2;
      this.p2_word_2 = p2_word_2;
    }
  }
}
