





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "GetAutoJTTasksVersionCodeResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class GetAutoJTTasksVersionCodeResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string GetAutoJTTasksVersionCodeResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string versionAndContents;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public string FORCEDUPDATE;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 3)]
    public string downloadLink;

    public GetAutoJTTasksVersionCodeResponse()
    {
    }

    public GetAutoJTTasksVersionCodeResponse(
      string GetAutoJTTasksVersionCodeResult,
      string versionAndContents,
      string FORCEDUPDATE,
      string downloadLink)
    {
      this.GetAutoJTTasksVersionCodeResult = GetAutoJTTasksVersionCodeResult;
      this.versionAndContents = versionAndContents;
      this.FORCEDUPDATE = FORCEDUPDATE;
      this.downloadLink = downloadLink;
    }
  }
}
