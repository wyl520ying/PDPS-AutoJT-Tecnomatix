





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "Create_MCM_LibraryTable", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class Create_MCM_LibraryTableRequest
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string tbName;

    public Create_MCM_LibraryTableRequest()
    {
    }

    public Create_MCM_LibraryTableRequest(string tbName) => this.tbName = tbName;
  }
}
