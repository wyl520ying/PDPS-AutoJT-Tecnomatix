





using System.CodeDom.Compiler;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "Tx_Refresh_MCM_new1Response", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class Tx_Refresh_MCM_new1Response
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public DataTable dataTable_Library;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public DataTable[] dataTable_AllUsers;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public DataTable NickNameOpenidDic;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 3)]
    public string infos;

    public Tx_Refresh_MCM_new1Response()
    {
    }

    public Tx_Refresh_MCM_new1Response(
      DataTable dataTable_Library,
      DataTable[] dataTable_AllUsers,
      DataTable NickNameOpenidDic,
      string infos)
    {
      this.dataTable_Library = dataTable_Library;
      this.dataTable_AllUsers = dataTable_AllUsers;
      this.NickNameOpenidDic = NickNameOpenidDic;
      this.infos = infos;
    }
  }
}
