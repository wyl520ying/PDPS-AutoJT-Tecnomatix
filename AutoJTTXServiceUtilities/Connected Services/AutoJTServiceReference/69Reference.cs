





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "Query_UsersTakeProjectsOpenID", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class Query_UsersTakeProjectsOpenIDRequest
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string openID;

    public Query_UsersTakeProjectsOpenIDRequest()
    {
    }

    public Query_UsersTakeProjectsOpenIDRequest(string openID) => this.openID = openID;
  }
}
