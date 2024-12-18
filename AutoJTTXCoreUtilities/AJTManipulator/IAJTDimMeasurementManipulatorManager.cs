





using Tecnomatix.Engineering;


namespace AutoJTTXCoreUtilities.AJTManipulator
{
  public interface IAJTDimMeasurementManipulatorManager
  {
    TxManipulator Manipulator { get; }

    TxColor ElementColor { get; }

    void Changed();

    string FormatValue(double val);
  }
}
