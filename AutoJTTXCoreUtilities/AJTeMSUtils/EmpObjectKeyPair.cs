





using EMPTYPELIBRARYLib;
using System.Collections.Generic;


namespace AutoJTTXCoreUtilities
{
  public class EmpObjectKeyPair
  {
    private bool isChecked;
    private EmpObjectKey parentKey;
    private EmpObjectKey childKey;
    private string name;
    private string type;
    private string date;
    private string filename;

    public EmpObjectKey ParentKey
    {
      get => this.parentKey;
      set => this.parentKey = value;
    }

    public EmpObjectKey ChildKey
    {
      get => this.childKey;
      set => this.childKey = value;
    }

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public string Type
    {
      get => this.type;
      set => this.type = value;
    }

    public string Date
    {
      get => this.date;
      set => this.date = value;
    }

    public string FilePath
    {
      get => this.filename;
      set => this.filename = value;
    }

    public EmpObjectKey ParentLib { get; set; }

    public List<EmpObjectKey> ChildLib { get; set; }
  }
}
