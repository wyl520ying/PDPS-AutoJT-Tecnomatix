





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "GetAutoJTTecnomatixVersionCode2Response", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class GetAutoJTTecnomatixVersionCode2Response
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string GetAutoJTTecnomatixVersionCode2Result;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string versionAndContents;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public string FORCEDUPDATE;

    public GetAutoJTTecnomatixVersionCode2Response()
    {
    }

    public GetAutoJTTecnomatixVersionCode2Response(
      string GetAutoJTTecnomatixVersionCode2Result,
      string versionAndContents,
      string FORCEDUPDATE)
    {
      this.GetAutoJTTecnomatixVersionCode2Result = GetAutoJTTecnomatixVersionCode2Result;
      this.versionAndContents = versionAndContents;
      this.FORCEDUPDATE = FORCEDUPDATE;
    }
  }
}
