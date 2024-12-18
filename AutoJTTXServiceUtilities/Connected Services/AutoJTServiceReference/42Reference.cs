





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "AddinProject_Model2DB_MCMResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class AddinProject_Model2DB_MCMResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public bool AddinProject_Model2DB_MCMResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string err;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public string info;

    public AddinProject_Model2DB_MCMResponse()
    {
    }

    public AddinProject_Model2DB_MCMResponse(
      bool AddinProject_Model2DB_MCMResult,
      string err,
      string info)
    {
      this.AddinProject_Model2DB_MCMResult = AddinProject_Model2DB_MCMResult;
      this.err = err;
      this.info = info;
    }
  }
}
