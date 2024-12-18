





using System.Net.NetworkInformation;


namespace AutoJTTXCoreUtilities
{
  public class NetworkUtil
  {
    public bool NetworkConnection(string targetIP)
    {
      Ping ping = new Ping();
      PingReply pingReply;
      try
      {
        pingReply = ping.Send(targetIP, 120);
      }
      catch
      {
        return false;
      }
      return pingReply.Status == IPStatus.Success;
    }
  }
}
