





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "CalcRealBoxLocationResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class CalcRealBoxLocationResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public bool CalcRealBoxLocationResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public double[] bottomLeftLower;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public double[] bottomLeftUpper;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 3)]
    public double[] bottomRightLower;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 4)]
    public double[] bottomRightUpper;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 5)]
    public double[] topLeftLower;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 6)]
    public double[] topLeftUpper;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 7)]
    public double[] topRightLower;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 8)]
    public double[] topRightUpper;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 9)]
    public double[] pp0;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 10)]
    public double[] p1_word;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 11)]
    public double[] p2_word;

    public CalcRealBoxLocationResponse()
    {
    }

    public CalcRealBoxLocationResponse(
      bool CalcRealBoxLocationResult,
      double[] bottomLeftLower,
      double[] bottomLeftUpper,
      double[] bottomRightLower,
      double[] bottomRightUpper,
      double[] topLeftLower,
      double[] topLeftUpper,
      double[] topRightLower,
      double[] topRightUpper,
      double[] pp0,
      double[] p1_word,
      double[] p2_word)
    {
      this.CalcRealBoxLocationResult = CalcRealBoxLocationResult;
      this.bottomLeftLower = bottomLeftLower;
      this.bottomLeftUpper = bottomLeftUpper;
      this.bottomRightLower = bottomRightLower;
      this.bottomRightUpper = bottomRightUpper;
      this.topLeftLower = topLeftLower;
      this.topLeftUpper = topLeftUpper;
      this.topRightLower = topRightLower;
      this.topRightUpper = topRightUpper;
      this.pp0 = pp0;
      this.p1_word = p1_word;
      this.p2_word = p2_word;
    }
  }
}
