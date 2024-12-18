





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "AddinProject_Model2DB_MCM_new", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class AddinProject_Model2DB_MCM_newRequest
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string PROJECT;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string MODEL;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public string REGION;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 3)]
    public string uuid;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 4)]
    public string OPERATOR;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 5)]
    public string tbname;

    public AddinProject_Model2DB_MCM_newRequest()
    {
    }

    public AddinProject_Model2DB_MCM_newRequest(
      string PROJECT,
      string MODEL,
      string REGION,
      string uuid,
      string OPERATOR,
      string tbname)
    {
      this.PROJECT = PROJECT;
      this.MODEL = MODEL;
      this.REGION = REGION;
      this.uuid = uuid;
      this.OPERATOR = OPERATOR;
      this.tbname = tbname;
    }
  }
}
