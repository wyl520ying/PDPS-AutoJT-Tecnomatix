





using EMPTYPELIBRARYLib;
using System.Collections.Generic;


namespace AutoJTTXCoreUtilities
{
  public class ResourceLibModel
  {
    private string name;

    public EmpObjectKey LibEmpObjKey { get; set; }

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public List<ResourceLibModel> ChildLib { get; set; }
  }
}
