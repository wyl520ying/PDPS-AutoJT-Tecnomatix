





using System.CodeDom.Compiler;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "AddinProject_Model2DB_MCM", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class AddinProject_Model2DB_MCMRequest
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public DataTable readDataTable;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string currentTime;

    public AddinProject_Model2DB_MCMRequest()
    {
    }

    public AddinProject_Model2DB_MCMRequest(DataTable readDataTable, string currentTime)
    {
      this.readDataTable = readDataTable;
      this.currentTime = currentTime;
    }
  }
}
