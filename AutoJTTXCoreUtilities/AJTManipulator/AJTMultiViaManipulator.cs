using System.Collections.Generic;
using Tecnomatix.Engineering;

namespace AutoJTTXCoreUtilities.AJTManipulator
{
    internal class AJTMultiViaManipulator
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

        public void CreateManipulator(TxTransformation location, TxTransformation delta, int count)
        {
            this.DestroyManipulator();
            if (location == null || count < 1)
            {
                return;
            }
            this._manipulators = new List<AJTViaManipulator>(count);
            TxTransformation txTransformation = location;
            for (int i = 0; i < count; i++)
            {
                AJTViaManipulator capViaManipulator = new AJTViaManipulator();
                capViaManipulator.CreateManipulator(txTransformation);
                this._manipulators.Add(capViaManipulator);
                txTransformation = delta * txTransformation;
            }
        }

        public void DestroyManipulator()
        {
            if (this._manipulators == null)
            {
                return;
            }
            foreach (AJTViaManipulator capViaManipulator in this._manipulators)
            {
                capViaManipulator.DestroyManipulator();
            }
        }

        private List<AJTViaManipulator> _manipulators;

        private TxColor _manipulatorColor = TxColor.TxColorBlue;
    }
}
