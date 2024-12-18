





using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;


namespace AutoJTMessageUtilities.AutoJTMessageService
{
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [ServiceContract(ConfigurationName = "AutoJTMessageService.IMessageService", CallbackContract = typeof (IMessageServiceCallback), SessionMode = SessionMode.Required)]
  public interface IMessageService
  {
    [OperationContract(IsOneWay = true, Action = "http://tempuri.org/IMessageService/Register")]
    void Register(string username, string cilentInfos, string openID, string regid);

    [OperationContract(IsOneWay = true, Action = "http://tempuri.org/IMessageService/Register")]
    Task RegisterAsync(string username, string cilentInfos, string openID, string regid);

    [OperationContract(IsOneWay = true, Action = "http://tempuri.org/IMessageService/ClientSendMessage")]
    void ClientSendMessage(string message, string username);

    [OperationContract(IsOneWay = true, Action = "http://tempuri.org/IMessageService/ClientSendMessage")]
    Task ClientSendMessageAsync(string message, string username);
  }
}
