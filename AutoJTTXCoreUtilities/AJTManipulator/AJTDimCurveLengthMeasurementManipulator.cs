





using System;
using Tecnomatix.Engineering;


namespace AutoJTTXCoreUtilities.AJTManipulator
{
  public class AJTDimCurveLengthMeasurementManipulator : AJTDimMeasurementManipulator
  {
    private TxTransformation m_firstObjectLocation;
    private TxTransformation m_firstObjectLeadingPointRelativeLocation;

    public AJTDimCurveLengthMeasurementManipulator(IAJTDimMeasurementManipulatorManager owner)
      : base(owner)
    {
      this.FirstObject = null;
      this.FirstObjectLocation = null;
    }

    public TxTransformation FirstObjectLocation
    {
      get => this.m_firstObjectLocation;
      set
      {
        if (this.m_firstObjectLocation != value)
        {
          this.m_firstObjectLocation = value;
          this.m_hasChanged = true;
        }
        this.SetFirstObjectLeadingPointRelativeLocation(this.FirstObject, value);
      }
    }

    public new ITxLocatableObject FirstObject
    {
      get => base.FirstObject;
      set
      {
        this.SetFirstObjectLeadingPointRelativeLocation(value, this.FirstObjectLocation);
        base.FirstObject = value;
      }
    }

    public TxTransformation FirstObjectLeadingPointLocation
    {
      get
      {
        return this.FirstObjectLocation == null ? new TxTransformation(this.GetFirstObjectLeadingPoint(), TxTransformation.TxRotationType.RPY_XYZ) : this.GetFirstObjectLeadingPointAbsoluteLocation();
      }
    }

    protected ITx1Dimensional FirstObjectCurve => this.FirstObject as ITx1Dimensional;

    protected double GetFirstObjectLength()
    {
      if (this.FirstObjectCurve != null)
        return this.FirstObjectCurve.Length();
      return this.FirstObject is ITx1DimensionalGeometry firstObject ? firstObject.ApproximatedLength() : 0.0;
    }

    protected TxVector GetFirstObjectLeadingPoint() => this.GetCurveMiddlePoint(this.FirstObject);

    private TxVector GetCurveMiddlePoint(ITxLocatableObject curve)
    {
      TxVector curveMiddlePoint = new TxVector();
      if (curve is ITx1Dimensional)
      {
        ITx1Dimensional tx1Dimensional = curve as ITx1Dimensional;
        TxVector startPoint;
        tx1Dimensional.GetStartPointAndTangent(out startPoint, out TxVector _);
        curveMiddlePoint = tx1Dimensional.GetPointByLengthAlongTheCurve(startPoint, tx1Dimensional.Length() / 2.0);
      }
      else if (curve is ITx1DimensionalGeometry dimensionalGeometry)
      {
        TxVector approximatedStartPoint = dimensionalGeometry.ApproximatedStartPoint;
        TxVector approximatedEndPoint = dimensionalGeometry.ApproximatedEndPoint;
        curveMiddlePoint = dimensionalGeometry.GetPointOnCurveByRatio(approximatedStartPoint, approximatedEndPoint, 0.5);
      }
      return curveMiddlePoint;
    }

    protected override void CalculateMeasureValue()
    {
      this.FirstPoint = new TxVector();
      this.MeasureValue = 0.0;
      this.MeasureValueCalculated = false;
      if (this.FirstObject == null)
        return;
      try
      {
        this.MeasureValue = this.GetFirstObjectLength();
        this.MeasureValueCalculated = true;
      }
      catch (Exception ex)
      {
        TxApplication.LogWriter.WriteErrorLine("Failed to calcualted curve object: " + this.FirstObject.Name + "length.");
        TxApplication.LogWriter.WriteExceptionLine(ex);
      }
    }

    protected void AddCircleElement(TxTransformation startPoint, double radius, TxColor color)
    {
      TxManipulatorArcElementData elementData = new TxManipulatorArcElementData(startPoint, radius, radius, 0.0, 2.0 * Math.PI);
      elementData.Color = color;
      elementData.AlwaysOnTop = false;
      elementData.Pickable = false;
      elementData.AntiAlias = true;
      this.m_manager.Manipulator.AddElement(elementData);
    }

    public override void CreateManipulatorElements()
    {
      this.RemoveManipulatorElements();
      if (!this.MeasureValueCalculated)
        return;
      double measureValue = this.MeasureValue;
      TxTransformation lengthTextPosition = this.GetCurveLengthTextPosition(this.FirstObjectLeadingPointLocation, measureValue);
      this.AddCircleElement(this.FirstObjectLeadingPointLocation, this.CalculateCircleRadius(measureValue), this.m_manager.ElementColor);
      this.AddLineElement(this.FirstObjectLeadingPointLocation.Translation, lengthTextPosition.Translation, this.m_manager.ElementColor);
      this.AddTextElement(lengthTextPosition, this.m_manager.FormatValue(this.MeasureValue), this.m_manager.ElementColor);
    }

    private TxTransformation GetCurveLengthTextPosition(
      TxTransformation locationOnObject,
      double curveLength)
    {
      TxTransformation txTransformation = new TxTransformation(new TxVector(0.0, 0.0, this.CalculateCurveLengthMeasurementLineLength(curveLength)), TxTransformation.TxRotationType.RPY_XYZ);
      return locationOnObject * txTransformation;
    }

    private double CalculateCurveLengthMeasurementLineLength(double curveLength)
    {
      double measurementLineLength = 100.0;
      if (curveLength < 200.0)
        measurementLineLength = curveLength * 0.5;
      return measurementLineLength;
    }

    private void SetFirstObjectLeadingPointRelativeLocation(
      ITxLocatableObject obj,
      TxTransformation location)
    {
      if (obj == null || !(location != null))
        return;
      this.m_firstObjectLeadingPointRelativeLocation = obj.AbsoluteLocation.Inverse * location;
    }

    private TxTransformation GetFirstObjectLeadingPointAbsoluteLocation()
    {
      TxTransformation absoluteLocation = null;
      if (this.FirstObject != null && this.m_firstObjectLeadingPointRelativeLocation != null)
        absoluteLocation = this.FirstObject.AbsoluteLocation * this.m_firstObjectLeadingPointRelativeLocation;
      return absoluteLocation;
    }

    private double CalculateCircleRadius(double curveLength)
    {
      double circleRadius = 6.0;
      if (curveLength <= 200.0 && curveLength > 10.0)
        circleRadius = 3.0;
      else if (curveLength <= 10.0)
        circleRadius = curveLength / 4.0;
      return circleRadius;
    }
  }
}
