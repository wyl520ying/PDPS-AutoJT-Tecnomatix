





using System.CodeDom.Compiler;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "ImportTableToDB1", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class ImportTableToDB1Request
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public DataTable dtImp;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string SqlTableName;

    public ImportTableToDB1Request()
    {
    }

    public ImportTableToDB1Request(DataTable dtImp, string SqlTableName)
    {
      this.dtImp = dtImp;
      this.SqlTableName = SqlTableName;
    }
  }
}
