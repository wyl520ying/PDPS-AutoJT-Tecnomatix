





using System.CodeDom.Compiler;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "Tx_Refresh_MCMResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class Tx_Refresh_MCMResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public DataTable dataTable_LIB;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string result_lib;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public DataTable[] dataTable_users_MCMS;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 3)]
    public string result_user;

    public Tx_Refresh_MCMResponse()
    {
    }

    public Tx_Refresh_MCMResponse(
      DataTable dataTable_LIB,
      string result_lib,
      DataTable[] dataTable_users_MCMS,
      string result_user)
    {
      this.dataTable_LIB = dataTable_LIB;
      this.result_lib = result_lib;
      this.dataTable_users_MCMS = dataTable_users_MCMS;
      this.result_user = result_user;
    }
  }
}
