





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "AddinTx_Project_Model_Lib_MCMResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class AddinTx_Project_Model_Lib_MCMResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public bool AddinTx_Project_Model_Lib_MCMResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string err;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public string info;

    public AddinTx_Project_Model_Lib_MCMResponse()
    {
    }

    public AddinTx_Project_Model_Lib_MCMResponse(
      bool AddinTx_Project_Model_Lib_MCMResult,
      string err,
      string info)
    {
      this.AddinTx_Project_Model_Lib_MCMResult = AddinTx_Project_Model_Lib_MCMResult;
      this.err = err;
      this.info = info;
    }
  }
}
