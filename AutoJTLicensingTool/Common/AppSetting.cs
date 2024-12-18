using AutoJTL.SDK.Strandard;
#if PSV
using AutoJTTXServiceUtilities;
#endif 
namespace AutoJTLicensingTool.Common
{
    internal static class AppSetting
    {
        //服务器地址
        public static string TestNetworkIP
        {
            get
            {
#if PSV
                return AJTDatabaseOperation.PSVAddress;
#else
                return "tx.autojt.com";
#endif                
            }
        }

        //终结点地址
        public static string Endpoint => "http://tx.autojt.com";
        //public static string Endpoint => Debug ? "http://localhost" : "http://tx.autojt.com";

        //hub终结点地址
        public static string HubEndpoint => $"{Endpoint}/signalr/chatHub";

        //client
        public static IAutoJTLClient AutoJTLClient = new AutoJTLClient(new AutoJTLOptions()
        {
            Endpoint = Endpoint,
        });

        //hubName
        public static string HubName => "chathub";
    }
}
