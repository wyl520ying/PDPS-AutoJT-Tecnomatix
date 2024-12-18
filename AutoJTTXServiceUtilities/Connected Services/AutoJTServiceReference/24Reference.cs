





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "UpdateWchatNiceName_2Net6Response", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class UpdateWchatNiceName_2Net6Response
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string UpdateWchatNiceName_2Net6Result;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public bool m_validity_status;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public string remarkName;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 3)]
    public bool isJT;

    public UpdateWchatNiceName_2Net6Response()
    {
    }

    public UpdateWchatNiceName_2Net6Response(
      string UpdateWchatNiceName_2Net6Result,
      bool m_validity_status,
      string remarkName,
      bool isJT)
    {
      this.UpdateWchatNiceName_2Net6Result = UpdateWchatNiceName_2Net6Result;
      this.m_validity_status = m_validity_status;
      this.remarkName = remarkName;
      this.isJT = isJT;
    }
  }
}
