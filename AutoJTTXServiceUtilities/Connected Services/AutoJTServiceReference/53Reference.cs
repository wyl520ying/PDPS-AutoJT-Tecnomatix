





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "DeleteProject_1", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class DeleteProject_1Request
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string uuid;

    public DeleteProject_1Request()
    {
    }

    public DeleteProject_1Request(string uuid) => this.uuid = uuid;
  }
}
