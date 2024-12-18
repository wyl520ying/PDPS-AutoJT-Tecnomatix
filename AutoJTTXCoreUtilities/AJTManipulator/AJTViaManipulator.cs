using EngineeringInternalExtension;
using Tecnomatix.Engineering;

namespace AutoJTTXCoreUtilities.AJTManipulator
{
    internal class AJTViaManipulator
    {
        public TxColor ManipulatorColor
        {
            get
            {
                return this._manipulatorColor;
            }
            set
            {
                this._manipulatorColor = value;
            }
        }

        public void CreateManipulator(TxTransformation point)
        {
            this.DestroyManipulator();
            if (point == null)
            {
                return;
            }
            float num = TxApplication.ViewersManager.GraphicViewer.ViewRectangle.Width / 40f;
            TxManipulatorLineElementData txManipulatorLineElementData = new TxManipulatorLineElementData(new TxTransformation(), new TxVector(), new TxVector((double)num, 0.0, 0.0))
            {
                Color = this.ManipulatorColor,
                LineWidth = 2.0,
                Pickable = false,
                AlwaysOnTop = true,
                Unzoomable = true,
                AntiAlias = true
            };
            TxManipulatorLineElementData txManipulatorLineElementData2 = new TxManipulatorLineElementData(new TxTransformation(), new TxVector(0.0, -0.125 * (double)num, 0.0), new TxVector(0.0, 0.375 * (double)num, 0.0))
            {
                Color = this.ManipulatorColor,
                LineWidth = 2.0,
                Pickable = false,
                AlwaysOnTop = true,
                Unzoomable = true,
                AntiAlias = true
            };
            TxManipulatorLineElementData txManipulatorLineElementData3 = new TxManipulatorLineElementData(new TxTransformation(), new TxVector(), new TxVector(0.0, 0.0, (double)num))
            {
                Color = this.ManipulatorColor,
                LineWidth = 1.0,
                Pickable = false,
                AlwaysOnTop = true,
                Unzoomable = true,
                AntiAlias = true
            };
            TxManipulatorCreationData txManipulatorCreationData = new TxManipulatorCreationData
            {
                AbsoluteLocation = point
            };
            this._manipulator = TxApplication.ActiveDocument.PhysicalRoot.CreateManipulator(txManipulatorCreationData);
            this._manipulator.AddElement(txManipulatorLineElementData);
            this._manipulator.AddElement(txManipulatorLineElementData2);
            this._manipulator.AddElement(txManipulatorLineElementData3);
            this._manipulator.Display();
        }

        public void DestroyManipulator()
        {
            if (this._manipulator != null)
            {
                this._manipulator.RemoveAllElements();
                TxObjectEx.ForceDeletion(this._manipulator);
                this._manipulator = null;
            }
        }

        private const int AxisLength = 40;

        private TxManipulator _manipulator;

        private TxColor _manipulatorColor = TxColor.TxColorBlue;
    }
}
