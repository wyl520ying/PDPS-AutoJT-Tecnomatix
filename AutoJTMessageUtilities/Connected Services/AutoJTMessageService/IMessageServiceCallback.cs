





using System.CodeDom.Compiler;
using System.ServiceModel;


namespace AutoJTMessageUtilities.AutoJTMessageService
{
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  public interface IMessageServiceCallback
  {
    [OperationContract(IsOneWay = true, Action = "http://tempuri.org/IMessageService/SendMessage")]
    void SendMessage(string message);
  }
}
