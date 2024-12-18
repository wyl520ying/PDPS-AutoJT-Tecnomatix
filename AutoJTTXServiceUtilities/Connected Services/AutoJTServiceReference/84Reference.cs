





using System.CodeDom.Compiler;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "CalcNoteQuadrantResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class CalcNoteQuadrantResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public DataTable CalcNoteQuadrantResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public object[] minXZ;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public object[] maxXZ;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 3)]
    public object[] centerXZ;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 4)]
    public string exInfos;

    public CalcNoteQuadrantResponse()
    {
    }

    public CalcNoteQuadrantResponse(
      DataTable CalcNoteQuadrantResult,
      object[] minXZ,
      object[] maxXZ,
      object[] centerXZ,
      string exInfos)
    {
      this.CalcNoteQuadrantResult = CalcNoteQuadrantResult;
      this.minXZ = minXZ;
      this.maxXZ = maxXZ;
      this.centerXZ = centerXZ;
      this.exInfos = exInfos;
    }
  }
}
