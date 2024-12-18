





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "CalcVIA_Rotation_4", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class CalcVIA_Rotation_4Request
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public double[] txTrans_old_VIA_OLD_apper_num;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public double[] txTrans_old_OLD_apper_world_num;

    public CalcVIA_Rotation_4Request()
    {
    }

    public CalcVIA_Rotation_4Request(
      double[] txTrans_old_VIA_OLD_apper_num,
      double[] txTrans_old_OLD_apper_world_num)
    {
      this.txTrans_old_VIA_OLD_apper_num = txTrans_old_VIA_OLD_apper_num;
      this.txTrans_old_OLD_apper_world_num = txTrans_old_OLD_apper_world_num;
    }
  }
}
