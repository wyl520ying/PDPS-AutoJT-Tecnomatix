





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "ImportTableToDB2Response", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class ImportTableToDB2Response
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public bool ImportTableToDB2Result;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string err;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public string info;

    public ImportTableToDB2Response()
    {
    }

    public ImportTableToDB2Response(bool ImportTableToDB2Result, string err, string info)
    {
      this.ImportTableToDB2Result = ImportTableToDB2Result;
      this.err = err;
      this.info = info;
    }
  }
}
