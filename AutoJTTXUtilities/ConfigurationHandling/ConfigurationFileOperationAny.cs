






namespace AutoJTTXUtilities.ConfigurationHandling
{
  public class ConfigurationFileOperationAny : ConfigurationFileOperation
  {
    public ConfigurationFileOperationAny(string installPath)
      : base(installPath)
    {
      this.m_installPath = installPath;
    }
  }
}
