





using System;


namespace AutoJTTXCoreUtilities
{
  public class ChangesMonitorValue_bool
  {
    private bool monitorValue;

    public bool MonitorValue
    {
      get => this.monitorValue;
      set
      {
        if (value == this.monitorValue)
          return;
        this.monitorValue = value;
        this.WhenMyValueChange();
      }
    }

    public event MyValueChanged OnMyValueChanged;

    private void WhenMyValueChange()
    {
      if (this.OnMyValueChanged == null)
        return;
      this.OnMyValueChanged(this, null);
    }

    public delegate void MyValueChanged(object sender, EventArgs e);
  }
}
