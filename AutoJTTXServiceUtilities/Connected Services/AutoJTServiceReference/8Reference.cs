





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "QueryAllComboMealInfoResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class QueryAllComboMealInfoResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string QueryAllComboMealInfoResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string combo_dt;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public string userInfos;

    public QueryAllComboMealInfoResponse()
    {
    }

    public QueryAllComboMealInfoResponse(
      string QueryAllComboMealInfoResult,
      string combo_dt,
      string userInfos)
    {
      this.QueryAllComboMealInfoResult = QueryAllComboMealInfoResult;
      this.combo_dt = combo_dt;
      this.userInfos = userInfos;
    }
  }
}
