





using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.ServiceModel.Channels;


namespace AutoJTMessageUtilities.AutoJTMessageService
{
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  public interface IMessageServiceChannel : 
    IMessageService,
    IClientChannel,
    IContextChannel,
    IChannel,
    ICommunicationObject,
    IExtensibleObject<IContextChannel>,
    IDisposable
  {
  }
}
