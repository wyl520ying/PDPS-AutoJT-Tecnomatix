





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "UpdateUserPaymentData", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class UpdateUserPaymentDataRequest
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string unionID;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string order_number;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public string amount;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 3)]
    public string combo_meal_code;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 4)]
    public string category;

    public UpdateUserPaymentDataRequest()
    {
    }

    public UpdateUserPaymentDataRequest(
      string unionID,
      string order_number,
      string amount,
      string combo_meal_code,
      string category)
    {
      this.unionID = unionID;
      this.order_number = order_number;
      this.amount = amount;
      this.combo_meal_code = combo_meal_code;
      this.category = category;
    }
  }
}
