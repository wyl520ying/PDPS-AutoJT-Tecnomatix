





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "DeleteProject_1Response", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class DeleteProject_1Response
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string infos_1;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string infos_2;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public string infos_3;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 3)]
    public int icount_1;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 4)]
    public int icount_2;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 5)]
    public int icount_3;

    public DeleteProject_1Response()
    {
    }

    public DeleteProject_1Response(
      string infos_1,
      string infos_2,
      string infos_3,
      int icount_1,
      int icount_2,
      int icount_3)
    {
      this.infos_1 = infos_1;
      this.infos_2 = infos_2;
      this.infos_3 = infos_3;
      this.icount_1 = icount_1;
      this.icount_2 = icount_2;
      this.icount_3 = icount_3;
    }
  }
}
