





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "CalcWD_Rotation_5", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class CalcWD_Rotation_5Request
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public double[] txTrans_new_world_num;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public double[] txTrans_new_apper_word_Inverse_num;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public double[] txTrans_old_apper_symmetry_num;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 3)]
    public bool istxPartAppearance_newNOTNull;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 4)]
    public bool isSymmetryWeld1;

    public CalcWD_Rotation_5Request()
    {
    }

    public CalcWD_Rotation_5Request(
      double[] txTrans_new_world_num,
      double[] txTrans_new_apper_word_Inverse_num,
      double[] txTrans_old_apper_symmetry_num,
      bool istxPartAppearance_newNOTNull,
      bool isSymmetryWeld1)
    {
      this.txTrans_new_world_num = txTrans_new_world_num;
      this.txTrans_new_apper_word_Inverse_num = txTrans_new_apper_word_Inverse_num;
      this.txTrans_old_apper_symmetry_num = txTrans_old_apper_symmetry_num;
      this.istxPartAppearance_newNOTNull = istxPartAppearance_newNOTNull;
      this.isSymmetryWeld1 = isSymmetryWeld1;
    }
  }
}
