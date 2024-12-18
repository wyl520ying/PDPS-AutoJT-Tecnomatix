using EngineeringInternalExtension;
using System;
using Tecnomatix.Engineering;

namespace AutoJTTXCoreUtilities.AJTManipulator
{
    internal class AJTMlmManipulator
    {
        //长度
        private double MANIPULATOR_LENGTH = 600.0;
        //字体
        private double TEXT_OFFSET_FROM_AXIS = 50.0;
        //locatable
        private TxTransformation _locatable;
        //data
        private TxManipulatorCreationData _data;
        //TxManipulator
        private TxManipulator _manipulator;
        //data
        public TxManipulatorCreationData Data
        {
            get
            {
                return this._data;
            }
        }

        //构造
        public AJTMlmManipulator(TxTransformation locatable)
        {
            this._locatable = locatable;
            this.CreateManipulatorData();
        }

        //创建data
        private void CreateManipulatorData()
        {
            this.MANIPULATOR_LENGTH = (double)(TxApplication.ViewersManager.GraphicViewer.ViewRectangle.Width / 12f);
            this.TEXT_OFFSET_FROM_AXIS = this.MANIPULATOR_LENGTH / 12.0;
            if (this._locatable != null)
            {
                this._data = new TxManipulatorCreationData();
                this._data.Name = "AJTKinematicsManipulator";
                this._data.AbsoluteLocation = this._locatable;


                #region line

                TxTransformation txTransformation = new TxTransformation();
                TxVector txVector = new TxVector(0.0, 0.0, 0.0);
                TxVector txVector2 = new TxVector(this.MANIPULATOR_LENGTH, 0.0, 0.0);
                TxManipulatorLineElementData txManipulatorLineElementData = new TxManipulatorLineElementData(txTransformation, txVector, txVector2);
                txManipulatorLineElementData.Color = TxColor.TxColorRed;
                txManipulatorLineElementData.Pickable = false;
                txManipulatorLineElementData.AlwaysOnTop = true;
                txManipulatorLineElementData.Unzoomable = false;

                TxTransformation txTransformation2 = new TxTransformation();
                TxVector txVector3 = new TxVector(0.0, 0.0, 0.0);
                TxVector txVector4 = new TxVector(0.0, this.MANIPULATOR_LENGTH, 0.0);
                TxManipulatorLineElementData txManipulatorLineElementData2 = new TxManipulatorLineElementData(txTransformation2, txVector3, txVector4);
                txManipulatorLineElementData2.Color = new TxColor(0, byte.MaxValue, 0);
                txManipulatorLineElementData2.Pickable = false;
                txManipulatorLineElementData2.AlwaysOnTop = true;
                txManipulatorLineElementData2.Unzoomable = false;

                TxTransformation txTransformation3 = new TxTransformation();
                TxVector txVector5 = new TxVector(0.0, 0.0, 0.0);
                TxVector txVector6 = new TxVector(0.0, 0.0, this.MANIPULATOR_LENGTH);
                TxManipulatorLineElementData txManipulatorLineElementData3 = new TxManipulatorLineElementData(txTransformation3, txVector5, txVector6);
                txManipulatorLineElementData3.Color = new TxColor(0, 0, byte.MaxValue);
                txManipulatorLineElementData3.Pickable = false;
                txManipulatorLineElementData3.AlwaysOnTop = true;
                txManipulatorLineElementData3.Unzoomable = false;

                #endregion

                #region cone

                TxTransformation txTransformation110 = new TxTransformation();
                TxVector txvector111 = txVector2;
                TxVector txvector112 = new TxVector(-this.MANIPULATOR_LENGTH, 0.0, 0.0);
                TxManipulatorConstantConeElementData txManipulatorConstantConeElementData = new TxManipulatorConstantConeElementData(txTransformation110, txvector111, txvector112, 6, 20);
                txManipulatorConstantConeElementData.Color = TxColor.TxColorRed;
                txManipulatorConstantConeElementData.Pickable = false;
                txManipulatorConstantConeElementData.AlwaysOnTop = true;
                txManipulatorConstantConeElementData.Unzoomable = false;

                TxTransformation txTransformation111 = new TxTransformation();
                TxVector txvector113 = txVector4;
                TxVector txvector114 = new TxVector(0.0, -this.MANIPULATOR_LENGTH, 0.0);
                TxManipulatorConstantConeElementData txManipulatorConstantConeElementDataY = new TxManipulatorConstantConeElementData(txTransformation111, txvector113, txvector114, 6, 20);
                txManipulatorConstantConeElementDataY.Color = new TxColor(0, byte.MaxValue, 0);
                txManipulatorConstantConeElementDataY.Pickable = false;
                txManipulatorConstantConeElementDataY.AlwaysOnTop = true;
                txManipulatorConstantConeElementDataY.Unzoomable = false;

                TxTransformation txTransformation113 = new TxTransformation();
                TxVector txvector115 = txVector6;
                TxVector txvector116 = new TxVector(0.0, 0.0, -this.MANIPULATOR_LENGTH);
                TxManipulatorConstantConeElementData txManipulatorConstantConeElementDataZ = new TxManipulatorConstantConeElementData(txTransformation113, txvector115, txvector116, 6, 20);
                txManipulatorConstantConeElementDataZ.Color = new TxColor(0, 0, byte.MaxValue);
                txManipulatorConstantConeElementDataZ.Pickable = false;
                txManipulatorConstantConeElementDataZ.AlwaysOnTop = true;
                txManipulatorConstantConeElementDataZ.Unzoomable = false;

                #endregion

                #region arc
                /*
                TxTransformation txTransformation4 = new TxTransformation(new TxVector(0.0, -1.5707963267948966, 0.0), (TxTransformation.TxRotationType)1);
                double manipulator_LENGTH = this.MANIPULATOR_LENGTH;
                double manipulator_LENGTH2 = this.MANIPULATOR_LENGTH;
                double num = 0.0;
                double num2 = 1.5707963267948966;
                TxManipulatorArcElementData txManipulatorArcElementData = new TxManipulatorArcElementData(txTransformation4, manipulator_LENGTH, manipulator_LENGTH2, num, num2);
                txManipulatorArcElementData.Color = TxColor.TxColorRed;
                txManipulatorArcElementData.Pickable = true;
                txManipulatorArcElementData.AlwaysOnTop = true;
                txManipulatorArcElementData.Unzoomable = true;

                TxTransformation txTransformation5 = new TxTransformation(new TxVector(1.5707963267948966, 0.0, 0.0), (TxTransformation.TxRotationType)1);
                double manipulator_LENGTH3 = this.MANIPULATOR_LENGTH;
                double manipulator_LENGTH4 = this.MANIPULATOR_LENGTH;
                double num3 = 0.0;
                double num4 = 1.5707963267948966;
                TxManipulatorArcElementData txManipulatorArcElementData2 = new TxManipulatorArcElementData(txTransformation5, manipulator_LENGTH3, manipulator_LENGTH4, num3, num4);
                txManipulatorArcElementData2.Color = new TxColor(0, byte.MaxValue, 0);
                txManipulatorArcElementData2.Pickable = true;
                txManipulatorArcElementData2.AlwaysOnTop = true;
                txManipulatorArcElementData2.Unzoomable = true;

                TxTransformation txTransformation6 = new TxTransformation();
                double manipulator_LENGTH5 = this.MANIPULATOR_LENGTH;
                double manipulator_LENGTH6 = this.MANIPULATOR_LENGTH;
                double num5 = 0.0;
                double num6 = 1.5707963267948966;
                TxManipulatorArcElementData txManipulatorArcElementData3 = new TxManipulatorArcElementData(txTransformation6, manipulator_LENGTH5, manipulator_LENGTH6, num5, num6);
                txManipulatorArcElementData3.Color = new TxColor(byte.MaxValue, byte.MaxValue, 0);
                txManipulatorArcElementData3.Pickable = true;
                txManipulatorArcElementData3.AlwaysOnTop = true;
                txManipulatorArcElementData3.Unzoomable = true;
                */
                #endregion

                #region text

                TxFont txFont = new TxFont();
                txFont.SizeInPoints = 18;//25磅

                TxTransformation txTransformation7 = new TxTransformation(1.0, 0.0, 0.0, this.MANIPULATOR_LENGTH + this.TEXT_OFFSET_FROM_AXIS + 5, 1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0);
                TxManipulatorTextElementData txManipulatorTextElementData = new TxManipulatorTextElementData(txTransformation7, "X");
                txManipulatorTextElementData.Color = TxColor.TxColorRed;
                txManipulatorTextElementData.Pickable = false;
                txManipulatorTextElementData.AlwaysOnTop = true;
                txManipulatorTextElementData.Unzoomable = false;
                txManipulatorTextElementData.Font = txFont;

                TxTransformation txTransformation8 = new TxTransformation(1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, this.MANIPULATOR_LENGTH + this.TEXT_OFFSET_FROM_AXIS + 5, 1.0, 0.0, 0.0, 0.0);
                TxManipulatorTextElementData txManipulatorTextElementData2 = new TxManipulatorTextElementData(txTransformation8, "Y");
                txManipulatorTextElementData2.Color = new TxColor(0, byte.MaxValue, 0);
                txManipulatorTextElementData2.Pickable = false;
                txManipulatorTextElementData2.AlwaysOnTop = true;
                txManipulatorTextElementData2.Unzoomable = false;
                txManipulatorTextElementData2.Font = txFont;

                TxTransformation txTransformation9 = new TxTransformation(1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, this.MANIPULATOR_LENGTH + this.TEXT_OFFSET_FROM_AXIS + 5);
                TxManipulatorTextElementData txManipulatorTextElementData3 = new TxManipulatorTextElementData(txTransformation9, "Z");
                txManipulatorTextElementData3.Color = new TxColor(0, 0, byte.MaxValue);
                txManipulatorTextElementData3.Pickable = false;
                txManipulatorTextElementData3.AlwaysOnTop = true;
                txManipulatorTextElementData3.Unzoomable = false;
                txManipulatorTextElementData3.Font = txFont;

                #endregion

                //line
                this._data.InsertElementData(txManipulatorLineElementData);
                this._data.InsertElementData(txManipulatorLineElementData2);
                this._data.InsertElementData(txManipulatorLineElementData3);

                //arc
                //his._data.InsertElementData(txManipulatorArcElementData);
                //this._data.InsertElementData(txManipulatorArcElementData2);
                //this._data.InsertElementData(txManipulatorArcElementData3);

                //text
                this._data.InsertElementData(txManipulatorTextElementData);
                this._data.InsertElementData(txManipulatorTextElementData2);
                this._data.InsertElementData(txManipulatorTextElementData3);

                //cone
                this._data.InsertElementData(txManipulatorConstantConeElementData);
                this._data.InsertElementData(txManipulatorConstantConeElementDataY);
                this._data.InsertElementData(txManipulatorConstantConeElementDataZ);

                return;
            }
            throw new NullReferenceException("The locatable object is null.");
        }


        //静态方法显示
        public static void CreateManipulator_1(TxManipulatorCreationData data)
        {
            TxManipulator _manipulator = TxApplication.ActiveDocument.PhysicalRoot.CreateManipulator(data);
            _manipulator.Display();

        }

        //销毁
        public void DestroyManipulator()
        {
            if (this._manipulator != null)
            {
                this._manipulator.RemoveAllElements();
                TxObjectEx.ForceDeletion(this._manipulator);
                this._manipulator = null;
            }
        }

        public void CreateManipulator()
        {
            //强行销毁
            this.DestroyManipulator();

            if (this._data == null)
            {
                return;
            }

            this._manipulator = TxApplication.ActiveDocument.PhysicalRoot.CreateManipulator(this._data);
            this._manipulator.Display();
        }
    }
}
