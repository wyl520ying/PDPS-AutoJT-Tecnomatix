





using System.CodeDom.Compiler;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "GetUserFromDatabase2Response", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class GetUserFromDatabase2Response
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public DataSet GetUserFromDatabase2Result;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string resultstatus;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public bool resultstatus2;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 3)]
    public int resultstatus3;

    public GetUserFromDatabase2Response()
    {
    }

    public GetUserFromDatabase2Response(
      DataSet GetUserFromDatabase2Result,
      string resultstatus,
      bool resultstatus2,
      int resultstatus3)
    {
      this.GetUserFromDatabase2Result = GetUserFromDatabase2Result;
      this.resultstatus = resultstatus;
      this.resultstatus2 = resultstatus2;
      this.resultstatus3 = resultstatus3;
    }
  }
}
