using EngineeringInternalExtension;
using System;
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

    public abstract class AJTDimMeasurementManipulator
    {
        protected IAJTDimMeasurementManipulatorManager m_manager;

        bool m_showXYZDelta;

        bool m_showXYZDeltaValues;

        protected bool m_hasChanged;

        ITxLocatableObject m_firstObject;

        ITxLocatableObject m_secondObject;

        public AJTDimMeasurementManipulator(IAJTDimMeasurementManipulatorManager manager)
        {
            this.m_manager = manager;
            this.FirstPoint = new TxVector();
            this.SecondPoint = new TxVector();
            this.m_showXYZDelta = false;
            this.m_showXYZDeltaValues = false;
            this.MeasureValueCalculated = false;
        }

        public ITxLocatableObject FirstObject
        {
            get
            {
                return this.m_firstObject;
            }
            set
            {
                if (this.m_firstObject != value)
                {
                    this.UnRegisterObjectEvents(this.m_firstObject);
                    this.m_firstObject = value;
                    this.m_hasChanged = true;
                    this.RegisterObjectEvents(this.m_firstObject);
                }
            }
        }

        public ITxLocatableObject SecondObject
        {
            get
            {
                return this.m_secondObject;
            }
            set
            {
                if (this.m_secondObject != value)
                {
                    this.UnRegisterObjectEvents(this.m_secondObject);
                    this.m_secondObject = value;
                    this.m_hasChanged = true;
                    this.RegisterObjectEvents(this.m_secondObject);
                }
            }
        }

        public TxVector FirstPoint { get; protected set; }

        public TxVector SecondPoint { get; protected set; }

        public double MeasureValue { get; protected set; }

        protected bool MeasureValueCalculated { get; set; }

        public virtual double DeltaXValue
        {
            get
            {
                return Math.Abs(this.FirstPoint.X - this.SecondPoint.X);
            }
        }

        public virtual double DeltaYValue
        {
            get
            {
                return Math.Abs(this.FirstPoint.Y - this.SecondPoint.Y);
            }
        }

        public virtual double DeltaZValue
        {
            get
            {
                return Math.Abs(this.FirstPoint.Z - this.SecondPoint.Z);
            }
        }

        //显示x y z三角洲元素和值
        public void ShowXYZDeltaElementsAndValue(bool showElements, bool showValues)
        {
            if (this.m_showXYZDelta != showElements)
            {
                this.m_showXYZDelta = showElements;
                this.m_showXYZDeltaValues = showValues;
                this.CreateManipulatorElements();
                return;
            }
            if (this.m_showXYZDeltaValues != showValues)
            {
                this.m_showXYZDeltaValues = showValues;
                this.CreateManipulatorElements();
            }
        }
        //刷新
        public void Refresh()
        {
            if (this.m_hasChanged)
            {
                this.CalculateMeasureValue();
                this.CreateManipulatorElements();
                this.m_hasChanged = false;
            }
        }
        //注册对象的事件
        protected virtual void RegisterObjectEvents(ITxLocatableObject obj)
        {
            if (obj != null)
            {
                obj.AbsoluteLocationChanged += new TxLocatableObject_AbsoluteLocationChangedEventHandler(this.ObjectAbsoluteLocationChanged);
                obj.Deleted += new TxObject_DeletedEventHandler(this.ObjectDeleted);
            }
        }
        //取消登记对象事件
        protected virtual void UnRegisterObjectEvents(ITxLocatableObject obj)
        {
            if (obj != null)
            {
                obj.AbsoluteLocationChanged -= new TxLocatableObject_AbsoluteLocationChangedEventHandler(this.ObjectAbsoluteLocationChanged);
                obj.Deleted -= new TxObject_DeletedEventHandler(this.ObjectDeleted);
            }
        }
        //对象绝对位置改变了
        protected virtual void ObjectAbsoluteLocationChanged(object sender, TxLocatableObject_AbsoluteLocationChangedEventArgs args)
        {
            this.m_hasChanged = true;
            this.m_manager.Changed();
        }
        //对象删除
        void ObjectDeleted(object sender, TxObject_DeletedEventArgs args)
        {
            this.UnRegisterObjectEvents(sender as ITxLocatableObject);
        }
        //计算测量值
        protected abstract void CalculateMeasureValue();
        //得到父组件
        protected ITxComponent GetParentComponent(ITxObject obj)
        {
            ITxComponent txComponent = null;
            if (obj is ITxGeometry || obj is TxFrame)
            {
                ITxObjectCollection txObjectCollection = obj.Collection;
                while (txObjectCollection != null && !(txObjectCollection is ITxComponent))
                {
                    txObjectCollection = txObjectCollection.Collection;
                }
                txComponent = txObjectCollection as ITxComponent;
            }
            return txComponent;
        }
        //添加默认值元素
        protected void AddDeltaValueElement(TxVector startPoint, TxVector endPoint, TxColor color, double value)
        {
            if (value != 0.0)
            {
                TxVector txVector = (startPoint + endPoint) / 2.0;
                TxTransformation txTransformation = new TxTransformation(txVector, TxTransformation.TxRotationType.RPY_XYZ);
                this.AddTextElement(txTransformation, this.m_manager.FormatValue(value), color);
            }
        }
        //添加线元素
        protected void AddLineElement(TxVector startPoint, TxVector endPoint, TxColor color)
        {
            TxManipulatorLineElementData txManipulatorLineElementData = new TxManipulatorLineElementData(new TxTransformation(), startPoint, endPoint);
            txManipulatorLineElementData.Color = color;
            txManipulatorLineElementData.AlwaysOnTop = false;
            txManipulatorLineElementData.Pickable = false;
            this.m_manager.Manipulator.AddElement(txManipulatorLineElementData);
        }
        //添加锥元素
        protected void AddConeElement(TxVector startPoint, TxVector endPoint, TxColor color)
        {
            int num = 15;
            int num2 = 7;
            TxManipulatorConstantConeElementData txManipulatorConstantConeElementData = new TxManipulatorConstantConeElementData(new TxTransformation(), startPoint, endPoint, (double)num2, (double)num);
            txManipulatorConstantConeElementData.Color = color;
            txManipulatorConstantConeElementData.AlwaysOnTop = false;
            txManipulatorConstantConeElementData.Pickable = false;
            this.m_manager.Manipulator.AddElement(txManipulatorConstantConeElementData);
        }
        //添加文本元素
        protected void AddTextElement(TxTransformation textLocation, string text, TxColor color)
        {
            TxManipulatorTextElementData txManipulatorTextElementData = new TxManipulatorTextElementData(textLocation, text);
            txManipulatorTextElementData.Color = color;
            txManipulatorTextElementData.Pickable = false;
            this.m_manager.Manipulator.AddElement(txManipulatorTextElementData);
        }
        //创建操纵元素
        public virtual void CreateManipulatorElements()
        {
            this.RemoveManipulatorElements();
            if (this.MeasureValueCalculated)
            {
                this.AddLineElement(this.FirstPoint, this.SecondPoint, this.m_manager.ElementColor);
                TxVector txVector = this.SecondPoint - this.FirstPoint;
                this.AddConeElement(this.FirstPoint, txVector, this.m_manager.ElementColor);
                this.AddConeElement(this.SecondPoint, -txVector, this.m_manager.ElementColor);
                TxVector txVector2 = (this.FirstPoint + this.SecondPoint) / 2.0;
                TxTransformation txTransformation = new TxTransformation(txVector2, TxTransformation.TxRotationType.RPY_XYZ);
                this.AddTextElement(txTransformation, this.m_manager.FormatValue(this.MeasureValue), this.m_manager.ElementColor);
                if (this.m_showXYZDelta)
                {
                    TxVector txVector3;
                    TxVector txVector4;
                    this.CalculateXYDeltaVectors(out txVector3, out txVector4);
                    this.AddLineElement(this.FirstPoint, txVector3, TxColor.TxColorRed);
                    this.AddLineElement(txVector3, txVector4, new TxColor(0, 0, byte.MaxValue));
                    this.AddLineElement(txVector4, this.SecondPoint, new TxColor(0, byte.MaxValue, 0));
                    if (this.m_showXYZDeltaValues)
                    {
                        this.AddDeltaValueElement(this.FirstPoint, txVector3, TxColor.TxColorRed, this.DeltaXValue);
                        this.AddDeltaValueElement(txVector3, txVector4, new TxColor(0, 0, byte.MaxValue), this.DeltaZValue);
                        this.AddDeltaValueElement(txVector4, this.SecondPoint, new TxColor(0, byte.MaxValue, 0), this.DeltaYValue);
                    }
                }
            }
        }
        //删除操纵元素
        protected void RemoveManipulatorElements()
        {
            this.m_manager.Manipulator.RemoveAllElements();
        }
        //计算x y默认向量
        void CalculateXYDeltaVectors(out TxVector vectorX, out TxVector vectorY)
        {
            TxTransformation workingFrame = TxApplication.ActiveDocument.WorkingFrame;
            TxTransformation txTransformation = new TxTransformation(this.FirstPoint, workingFrame.RotationRPY_XYZ, TxTransformation.TxRotationType.RPY_XYZ);
            TxTransformation txTransformation2 = new TxTransformation(this.SecondPoint, workingFrame.RotationRPY_XYZ, TxTransformation.TxRotationType.RPY_XYZ);
            TxTransformation txTransformation3 = TxTransformationEx.Delta(txTransformation, txTransformation2);
            TxVector axis = TxTransformationEx.GetAxis(txTransformation, TxTransformationEx.TxAxisEx.X);
            vectorX = this.FirstPoint + axis * txTransformation3.Translation.X;
            TxTransformation txTransformation4 = new TxTransformation(vectorX, workingFrame.RotationRPY_XYZ, TxTransformation.TxRotationType.RPY_XYZ);
            TxVector axis2 = TxTransformationEx.GetAxis(txTransformation4, TxTransformationEx.TxAxisEx.Z);
            vectorY = vectorX + axis2 * txTransformation3.Translation.Z;
        }
    }

    public class AJTDimCurveLengthMeasurementManipulator : AJTDimMeasurementManipulator
    {
        TxTransformation m_firstObjectLocation;

        TxTransformation m_firstObjectLeadingPointRelativeLocation;

        public AJTDimCurveLengthMeasurementManipulator(IAJTDimMeasurementManipulatorManager owner)
            : base(owner)
        {
            this.FirstObject = null;
            this.FirstObjectLocation = null;
        }

        public TxTransformation FirstObjectLocation
        {
            get
            {
                return this.m_firstObjectLocation;
            }
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
            get
            {
                return base.FirstObject;
            }
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
                if (this.FirstObjectLocation == null)
                {
                    return new TxTransformation(this.GetFirstObjectLeadingPoint(), TxTransformation.TxRotationType.RPY_XYZ);
                }
                return this.GetFirstObjectLeadingPointAbsoluteLocation();
            }
        }

        protected ITx1Dimensional FirstObjectCurve
        {
            get
            {
                return this.FirstObject as ITx1Dimensional;
            }
        }
        //得到第一个物体长度
        protected double GetFirstObjectLength()
        {
            if (this.FirstObjectCurve != null)
            {
                return this.FirstObjectCurve.Length();
            }
            ITx1DimensionalGeometry tx1DimensionalGeometry = this.FirstObject as ITx1DimensionalGeometry;
            if (tx1DimensionalGeometry != null)
            {
                return tx1DimensionalGeometry.ApproximatedLength();
            }
            return 0.0;
        }
        //获得第一个对象主要点
        protected TxVector GetFirstObjectLeadingPoint()
        {
            return GetCurveMiddlePoint(this.FirstObject);
        }

        //得到曲线中点
        TxVector GetCurveMiddlePoint(ITxLocatableObject curve)
        {
            TxVector txVector = new TxVector();
            if (curve is ITx1Dimensional)
            {
                ITx1Dimensional tx1Dimensional = curve as ITx1Dimensional;
                TxVector approximatedStartPoint;
                TxVector txVector2;
                tx1Dimensional.GetStartPointAndTangent(out approximatedStartPoint, out txVector2);
                txVector = tx1Dimensional.GetPointByLengthAlongTheCurve(approximatedStartPoint, tx1Dimensional.Length() / 2.0);
            }
            else
            {
                ITx1DimensionalGeometry tx1DimensionalGeometry = curve as ITx1DimensionalGeometry;
                if (tx1DimensionalGeometry != null)
                {
                    TxVector approximatedStartPoint = tx1DimensionalGeometry.ApproximatedStartPoint;
                    TxVector approximatedEndPoint = tx1DimensionalGeometry.ApproximatedEndPoint;
                    txVector = tx1DimensionalGeometry.GetPointOnCurveByRatio(approximatedStartPoint, approximatedEndPoint, 0.5);
                }
            }
            return txVector;
        }
        //计算测量值
        protected override void CalculateMeasureValue()
        {
            base.FirstPoint = new TxVector();
            base.MeasureValue = 0.0;
            base.MeasureValueCalculated = false;
            if (this.FirstObject != null)
            {
                try
                {
                    base.MeasureValue = this.GetFirstObjectLength();
                    base.MeasureValueCalculated = true;
                }
                catch (Exception ex)
                {
                    TxApplication.LogWriter.WriteErrorLine("Failed to calcualted curve object: " + this.FirstObject.Name + "length.");
                    TxApplication.LogWriter.WriteExceptionLine(ex);
                }
            }
        }
        //添加圆元素
        protected void AddCircleElement(TxTransformation startPoint, double radius, TxColor color)
        {
            TxManipulatorArcElementData txManipulatorArcElementData = new TxManipulatorArcElementData(startPoint, radius, radius, 0.0, 6.283185307179586);
            txManipulatorArcElementData.Color = color;
            txManipulatorArcElementData.AlwaysOnTop = false;
            txManipulatorArcElementData.Pickable = false;
            txManipulatorArcElementData.AntiAlias = true;
            this.m_manager.Manipulator.AddElement(txManipulatorArcElementData);
        }
        //创建操纵元素
        public override void CreateManipulatorElements()
        {
            base.RemoveManipulatorElements();
            if (base.MeasureValueCalculated)
            {
                double measureValue = base.MeasureValue;
                TxTransformation curveLengthTextPosition = GetCurveLengthTextPosition(this.FirstObjectLeadingPointLocation, measureValue);
                double num = this.CalculateCircleRadius(measureValue);
                this.AddCircleElement(this.FirstObjectLeadingPointLocation, num, this.m_manager.ElementColor);
                base.AddLineElement(this.FirstObjectLeadingPointLocation.Translation, curveLengthTextPosition.Translation, this.m_manager.ElementColor);
                base.AddTextElement(curveLengthTextPosition, this.m_manager.FormatValue(base.MeasureValue), this.m_manager.ElementColor);
            }
        }
        //得到的曲线长度文本的位置
        TxTransformation GetCurveLengthTextPosition(TxTransformation locationOnObject, double curveLength)
        {
            double num = CalculateCurveLengthMeasurementLineLength(curveLength);
            TxVector txVector = new TxVector(0.0, 0.0, num);
            TxTransformation txTransformation = new TxTransformation(txVector, TxTransformation.TxRotationType.RPY_XYZ);
            return locationOnObject * txTransformation;
        }
        //计算曲线长度测量线的长度
        double CalculateCurveLengthMeasurementLineLength(double curveLength)
        {
            double num = 100.0;
            if (curveLength < 200.0)
            {
                num = curveLength * 0.5;
            }
            return num;
        }
        //设置第一个对象主要点的相对位置
        void SetFirstObjectLeadingPointRelativeLocation(ITxLocatableObject obj, TxTransformation location)
        {
            if (obj != null && location != null)
            {
                this.m_firstObjectLeadingPointRelativeLocation = obj.AbsoluteLocation.Inverse * location;
            }
        }
        //获得第一个对象点绝对位置
        TxTransformation GetFirstObjectLeadingPointAbsoluteLocation()
        {
            TxTransformation txTransformation = null;
            if (this.FirstObject != null && this.m_firstObjectLeadingPointRelativeLocation != null)
            {
                txTransformation = this.FirstObject.AbsoluteLocation * this.m_firstObjectLeadingPointRelativeLocation;
            }
            return txTransformation;
        }
        //计算圆的半径
        double CalculateCircleRadius(double curveLength)
        {
            double num = 6.0;
            if (curveLength <= 200.0 && curveLength > 10.0)
            {
                num = 3.0;
            }
            else if (curveLength <= 10.0)
            {
                num = curveLength / 4.0;
            }
            return num;
        }
    }
}