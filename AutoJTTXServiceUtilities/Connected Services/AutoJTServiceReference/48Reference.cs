





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "Create_MCM_LibraryTableResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class Create_MCM_LibraryTableResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public bool Create_MCM_LibraryTableResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string infos;

    public Create_MCM_LibraryTableResponse()
    {
    }

    public Create_MCM_LibraryTableResponse(bool Create_MCM_LibraryTableResult, string infos)
    {
      this.Create_MCM_LibraryTableResult = Create_MCM_LibraryTableResult;
      this.infos = infos;
    }
  }
}
