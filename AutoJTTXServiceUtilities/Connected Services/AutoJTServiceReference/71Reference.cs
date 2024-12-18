





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "Query_UserNameByOpenID", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class Query_UserNameByOpenIDRequest
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string openID;

    public Query_UserNameByOpenIDRequest()
    {
    }

    public Query_UserNameByOpenIDRequest(string openID) => this.openID = openID;
  }
}
