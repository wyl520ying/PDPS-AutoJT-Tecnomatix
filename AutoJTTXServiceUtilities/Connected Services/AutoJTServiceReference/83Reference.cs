





using System.CodeDom.Compiler;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "CalcNoteQuadrant", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class CalcNoteQuadrantRequest
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public DataTable dataTable1;

    public CalcNoteQuadrantRequest()
    {
    }

    public CalcNoteQuadrantRequest(DataTable dataTable1) => this.dataTable1 = dataTable1;
  }
}
