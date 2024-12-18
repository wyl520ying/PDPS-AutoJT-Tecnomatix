





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "Query_UserNameByOpenIDResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class Query_UserNameByOpenIDResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string Query_UserNameByOpenIDResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string infos;

    public Query_UserNameByOpenIDResponse()
    {
    }

    public Query_UserNameByOpenIDResponse(string Query_UserNameByOpenIDResult, string infos)
    {
      this.Query_UserNameByOpenIDResult = Query_UserNameByOpenIDResult;
      this.infos = infos;
    }
  }
}
