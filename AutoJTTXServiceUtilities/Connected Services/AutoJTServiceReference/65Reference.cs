





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "Query_ProExists_uuid_MCM", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class Query_ProExists_uuid_MCMRequest
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string uuid;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string openid;

    public Query_ProExists_uuid_MCMRequest()
    {
    }

    public Query_ProExists_uuid_MCMRequest(string uuid, string openid)
    {
      this.uuid = uuid;
      this.openid = openid;
    }
  }
}
