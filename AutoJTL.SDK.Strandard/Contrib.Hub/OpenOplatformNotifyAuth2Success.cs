





using System;


namespace Contrib.Hub
{
  [Serializable]
  public class OpenOplatformNotifyAuth2Success
  {
    public const string MethodName = "OpenOplatformNotifyAuth2Success";

    public string OpenId { get; set; }

    public string NickName { get; set; }

    public string UnionId { get; set; }

    public bool Subscribe { get; set; }
  }
}
