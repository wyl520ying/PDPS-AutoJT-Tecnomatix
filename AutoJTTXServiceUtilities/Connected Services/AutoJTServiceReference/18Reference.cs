





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "ImportTableToDB1Response", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class ImportTableToDB1Response
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public bool ImportTableToDB1Result;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string err;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public string info;

    public ImportTableToDB1Response()
    {
    }

    public ImportTableToDB1Response(bool ImportTableToDB1Result, string err, string info)
    {
      this.ImportTableToDB1Result = ImportTableToDB1Result;
      this.err = err;
      this.info = info;
    }
  }
}
