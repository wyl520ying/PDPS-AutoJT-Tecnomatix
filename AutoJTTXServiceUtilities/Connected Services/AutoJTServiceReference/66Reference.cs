





using System.CodeDom.Compiler;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "Query_ProExists_uuid_MCMResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class Query_ProExists_uuid_MCMResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public DataTable Query_ProExists_uuid_MCMResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public int status;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public string infos;

    public Query_ProExists_uuid_MCMResponse()
    {
    }

    public Query_ProExists_uuid_MCMResponse(
      DataTable Query_ProExists_uuid_MCMResult,
      int status,
      string infos)
    {
      this.Query_ProExists_uuid_MCMResult = Query_ProExists_uuid_MCMResult;
      this.status = status;
      this.infos = infos;
    }
  }
}
