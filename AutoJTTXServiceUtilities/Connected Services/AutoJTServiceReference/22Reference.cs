





using System.CodeDom.Compiler;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "UpdateWchatNiceNameResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class UpdateWchatNiceNameResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string UpdateWchatNiceNameResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public DataSet m_DataSet;

    public UpdateWchatNiceNameResponse()
    {
    }

    public UpdateWchatNiceNameResponse(string UpdateWchatNiceNameResult, DataSet m_DataSet)
    {
      this.UpdateWchatNiceNameResult = UpdateWchatNiceNameResult;
      this.m_DataSet = m_DataSet;
    }
  }
}
