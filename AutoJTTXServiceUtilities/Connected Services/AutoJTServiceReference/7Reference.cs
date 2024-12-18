





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "QueryAllComboMealInfo", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class QueryAllComboMealInfoRequest
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string unionID;

    public QueryAllComboMealInfoRequest()
    {
    }

    public QueryAllComboMealInfoRequest(string unionID) => this.unionID = unionID;
  }
}
