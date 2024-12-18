





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "GetUserFromDatabase", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class GetUserFromDatabaseRequest
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string dataBaseTable;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string pass;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public string username;

    public GetUserFromDatabaseRequest()
    {
    }

    public GetUserFromDatabaseRequest(string dataBaseTable, string pass, string username)
    {
      this.dataBaseTable = dataBaseTable;
      this.pass = pass;
      this.username = username;
    }
  }
}
