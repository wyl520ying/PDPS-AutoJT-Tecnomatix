





using System.CodeDom.Compiler;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "ImportTableToDB2", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class ImportTableToDB2Request
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public DataTable dtImp;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string SqlTableName;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public string currentTime;

    public ImportTableToDB2Request()
    {
    }

    public ImportTableToDB2Request(DataTable dtImp, string SqlTableName, string currentTime)
    {
      this.dtImp = dtImp;
      this.SqlTableName = SqlTableName;
      this.currentTime = currentTime;
    }
  }
}
