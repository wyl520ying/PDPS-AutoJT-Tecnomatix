





using System.CodeDom.Compiler;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "Query_AllPro4OpenIDResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class Query_AllPro4OpenIDResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public DataTable Query_AllPro4OpenIDResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string infos;

    public Query_AllPro4OpenIDResponse()
    {
    }

    public Query_AllPro4OpenIDResponse(DataTable Query_AllPro4OpenIDResult, string infos)
    {
      this.Query_AllPro4OpenIDResult = Query_AllPro4OpenIDResult;
      this.infos = infos;
    }
  }
}
