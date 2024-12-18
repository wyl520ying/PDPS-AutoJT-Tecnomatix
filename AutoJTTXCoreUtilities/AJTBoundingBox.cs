using System.Collections;
using Tecnomatix.Engineering;

namespace AutoJTTXCoreUtilities
{
    public class AJTBoundingBox
    {
        //TxManipulator对象
        private TxManipulator m_manip;
        private TxTransformation m_transformation;


        private TxVector m_lowerLeft;

        private TxVector m_upperRight;


        private TxVector m_bottomLeftLower;

        private TxVector m_bottomLeftUpper;

        private TxVector m_bottomRightLower;

        private TxVector m_bottomRightUpper;

        private TxVector m_topLeftLower;

        private TxVector m_topLeftUpper;

        private TxVector m_topRightLower;

        private TxVector m_topRightUpper;

        //是否计算8个顶点
        private bool m_IsCalcEightBox;
        //是否绘制原点线
        private bool m_IsCrateBetweenLin;
        //Lower
        public TxVector LowerLeft
        {
            get
            {
                return this.m_lowerLeft;
            }
            set
            {
                this.m_lowerLeft = value;
            }
        }

        //Higher
        public TxVector UpperRight
        {
            get
            {
                return this.m_upperRight;
            }
            set
            {
                this.m_upperRight = value;
            }
        }

        public AJTBoundingBox(bool isCalcEightBox = true, bool isCrateBetweenLin = true)
        {
            m_IsCalcEightBox = isCalcEightBox;
            m_IsCrateBetweenLin = isCrateBetweenLin;
        }

        public AJTBoundingBox(TxVector bottomLeftLower, TxVector bottomLeftUpper, TxVector bottomRightLower, TxVector bottomRightUpper,
                              TxVector topLeftLower, TxVector topLeftUpper, TxVector topRightLower, TxVector topRightUpper, bool isCalcEightBox = false, bool isCrateBetweenLin = false)
        {
            TxPhysicalRoot physicalRoot = TxApplication.ActiveDocument.PhysicalRoot;

            //Manipulator对象的创建数据。
            TxManipulatorCreationData txManipulatorCreationData = new TxManipulatorCreationData();
            //创建TxManipulator
            this.m_manip = physicalRoot.CreateManipulator(txManipulatorCreationData);

            //是否计算8个顶点
            m_IsCalcEightBox = isCalcEightBox;
            //是否绘制原点线
            m_IsCrateBetweenLin = isCrateBetweenLin;

            m_bottomLeftLower = bottomLeftLower;
            m_bottomLeftUpper = bottomLeftUpper;
            m_bottomRightLower = bottomRightLower;
            m_bottomRightUpper = bottomRightUpper;
            m_topLeftLower = topLeftLower;
            m_topLeftUpper = topLeftUpper;
            m_topRightLower = topRightLower;
            m_topRightUpper = topRightUpper;
        }

        //创建boundingBox
        public void CreateBoundingBox(TxVector lowerLeft, TxVector upperRight, TxTransformation transformation)
        {
            this.m_transformation = transformation;
            TxVector txVector = TxVector.Subtract(upperRight, lowerLeft);
            if (this.m_IsCalcEightBox)
            {
                this.CalculateEightBoxCorners(lowerLeft, upperRight);
            }
            this.DeleteAllLines();
            this.AddAllLines();
            this.CreateCenterSymbol();
        }


        //销毁boundingBox
        public void DestroyBoundingBox()
        {
            this.DeleteAllLines();
            if (this.m_manip != null)
            {
                this.m_manip.Delete();
                this.m_manip = null;
            }
        }

        //计算8个顶点
        private void CalculateEightBoxCorners(TxVector leftLower, TxVector rightUpper)
        {
            this.m_bottomLeftLower = new TxVector(leftLower);
            this.m_topRightUpper = new TxVector(rightUpper);
            this.m_bottomLeftUpper = new TxVector(leftLower.X, leftLower.Y, rightUpper.Z);
            this.m_bottomRightLower = new TxVector(rightUpper.X, leftLower.Y, leftLower.Z);
            this.m_bottomRightUpper = new TxVector(rightUpper.X, leftLower.Y, rightUpper.Z);
            this.m_topLeftLower = new TxVector(leftLower.X, rightUpper.Y, leftLower.Z);
            this.m_topLeftUpper = new TxVector(leftLower.X, rightUpper.Y, rightUpper.Z);
            this.m_topRightLower = new TxVector(rightUpper.X, rightUpper.Y, leftLower.Z);
        }

        //删除所有的线条
        private void DeleteAllLines()
        {
            if (this.m_manip != null)
            {
                try
                {
                    this.m_manip.RemoveAllElements();
                }
                catch (TxException ex)
                {
                }
            }
        }

        //添加12线条
        private void AddAllLines()
        {
            TxColor txColorYellow = TxColor.TxColorYellow;
            TxColor txColorRed = TxColor.TxColorRed;
            TxColor txColorGreen = TxColor.TxColorGreen;
            if (this.m_manip != null)
            {
                try
                {
                    int num = default(int);
                    this.CreateLineBetweenPoints(ref num, this.m_bottomLeftUpper, this.m_bottomRightUpper, txColorYellow);
                    this.CreateLineBetweenPoints(ref num, this.m_bottomLeftLower, this.m_bottomRightLower, txColorYellow);
                    this.CreateLineBetweenPoints(ref num, this.m_bottomRightLower, this.m_bottomRightUpper, txColorYellow);
                    this.CreateLineBetweenPoints(ref num, this.m_bottomLeftLower, this.m_bottomLeftUpper, txColorYellow);
                    this.CreateLineBetweenPoints(ref num, this.m_topLeftUpper, this.m_topRightUpper, txColorYellow);
                    this.CreateLineBetweenPoints(ref num, this.m_topLeftLower, this.m_topRightLower, txColorYellow);
                    this.CreateLineBetweenPoints(ref num, this.m_topRightLower, this.m_topRightUpper, txColorYellow);
                    this.CreateLineBetweenPoints(ref num, this.m_topLeftLower, this.m_topLeftUpper, txColorYellow);
                    this.CreateLineBetweenPoints(ref num, this.m_bottomLeftUpper, this.m_topLeftUpper, txColorYellow);
                    this.CreateLineBetweenPoints(ref num, this.m_bottomLeftLower, this.m_topLeftLower, txColorYellow);
                    this.CreateLineBetweenPoints(ref num, this.m_bottomRightLower, this.m_topRightLower, txColorYellow);
                    this.CreateLineBetweenPoints(ref num, this.m_bottomRightUpper, this.m_topRightUpper, txColorYellow);

                    if (this.m_IsCrateBetweenLin)
                    {
                        double num2 = (this.m_bottomRightLower.X - this.m_bottomLeftLower.X) / 3.0;
                        this.m_bottomRightLower.X = this.m_bottomLeftLower.X + num2;
                        this.CreateOrientaionLineBetweenPoints(ref num, this.m_bottomLeftLower, this.m_bottomRightLower, txColorRed);

                        num2 = (this.m_topLeftLower.Y - this.m_bottomLeftLower.Y) / 3.0;
                        this.m_topLeftLower.Y = this.m_bottomLeftLower.Y + num2;
                        this.CreateOrientaionLineBetweenPoints(ref num, this.m_bottomLeftLower, this.m_topLeftLower, txColorGreen);
                    }
                }
                catch (TxException ex)
                {
                }
            }
        }

        //添加线条
        private void CreateLineBetweenPoints(ref int elementId, TxVector beginPoint, TxVector endPoint, TxColor color)
        {
            TxManipulatorElementData txManipulatorElementData = new TxManipulatorLineElementData(this.m_transformation, beginPoint, endPoint);
            txManipulatorElementData.Color = color;
            txManipulatorElementData.AlwaysOnTop = false;
            txManipulatorElementData.Pickable = false;
            elementId = this.m_manip.AddElement(txManipulatorElementData);
        }

        private void CreateOrientaionLineBetweenPoints(ref int elementId, TxVector beginPoint, TxVector endPoint, TxColor color)
        {
            TxManipulatorElementData txManipulatorElementData = new TxManipulatorLineElementData(this.m_transformation, beginPoint, endPoint)
            {
                LineWidth = 4.0
            };
            txManipulatorElementData.Color = color;
            txManipulatorElementData.AlwaysOnTop = false;
            txManipulatorElementData.Pickable = false;
            elementId = this.m_manip.AddElement(txManipulatorElementData);
        }

        //创建中心符号
        private void CreateCenterSymbol()
        {
            if (this.m_manip != null)
            {
                int num = 8;
                ArrayList arrayList = new ArrayList(num);
                byte b = 24;
                byte maxValue = byte.MaxValue;
                arrayList.Add(b);
                arrayList.Add(b);
                arrayList.Add(b);
                arrayList.Add(maxValue);
                arrayList.Add(maxValue);
                arrayList.Add(b);
                arrayList.Add(b);
                arrayList.Add(b);
                TxManipulatorSymbolElementData txManipulatorSymbolElementData = new TxManipulatorSymbolElementData(this.m_transformation, num, num, arrayList);
                txManipulatorSymbolElementData.Color = TxColor.TxColorYellow;
                txManipulatorSymbolElementData.Pickable = false;
                txManipulatorSymbolElementData.AlwaysOnTop = false;
                this.m_manip.AddElement(txManipulatorSymbolElementData);
            }
        }

        //显示BoundingBox
        public void ShowBoundingBox(bool show)
        {
            if (show)
            {
                this.m_manip.Display();
            }
            else
            {
                this.m_manip.Blank();
            }
        }
    }
}