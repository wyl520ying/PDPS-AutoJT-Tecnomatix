





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "GetUserFromDatabase2", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class GetUserFromDatabase2Request
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string category;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string userlnfos;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public string clientVersion;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 3)]
    public string dataBaseTable;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 4)]
    public string pass;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 5)]
    public string username;

    public GetUserFromDatabase2Request()
    {
    }

    public GetUserFromDatabase2Request(
      string category,
      string userlnfos,
      string clientVersion,
      string dataBaseTable,
      string pass,
      string username)
    {
      this.category = category;
      this.userlnfos = userlnfos;
      this.clientVersion = clientVersion;
      this.dataBaseTable = dataBaseTable;
      this.pass = pass;
      this.username = username;
    }
  }
}
