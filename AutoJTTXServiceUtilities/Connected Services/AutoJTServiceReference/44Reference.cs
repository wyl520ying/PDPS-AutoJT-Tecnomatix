





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "AddinProject_Model2DB_MCM_newResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class AddinProject_Model2DB_MCM_newResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public bool AddinProject_Model2DB_MCM_newResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string infos;

    public AddinProject_Model2DB_MCM_newResponse()
    {
    }

    public AddinProject_Model2DB_MCM_newResponse(
      bool AddinProject_Model2DB_MCM_newResult,
      string infos)
    {
      this.AddinProject_Model2DB_MCM_newResult = AddinProject_Model2DB_MCM_newResult;
      this.infos = infos;
    }
  }
}
