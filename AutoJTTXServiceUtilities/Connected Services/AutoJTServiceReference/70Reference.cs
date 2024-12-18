





using System.CodeDom.Compiler;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "Query_UsersTakeProjectsOpenIDResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class Query_UsersTakeProjectsOpenIDResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public DataTable Query_UsersTakeProjectsOpenIDResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string infos;

    public Query_UsersTakeProjectsOpenIDResponse()
    {
    }

    public Query_UsersTakeProjectsOpenIDResponse(
      DataTable Query_UsersTakeProjectsOpenIDResult,
      string infos)
    {
      this.Query_UsersTakeProjectsOpenIDResult = Query_UsersTakeProjectsOpenIDResult;
      this.infos = infos;
    }
  }
}
