





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "Query_AllPro4OpenID", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class Query_AllPro4OpenIDRequest
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string openID;

    public Query_AllPro4OpenIDRequest()
    {
    }

    public Query_AllPro4OpenIDRequest(string openID) => this.openID = openID;
  }
}
