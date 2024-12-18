





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "UpdateUserPaymentDataResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class UpdateUserPaymentDataResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public bool UpdateUserPaymentDataResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string expiry_date;

    public UpdateUserPaymentDataResponse()
    {
    }

    public UpdateUserPaymentDataResponse(bool UpdateUserPaymentDataResult, string expiry_date)
    {
      this.UpdateUserPaymentDataResult = UpdateUserPaymentDataResult;
      this.expiry_date = expiry_date;
    }
  }
}
