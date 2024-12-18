using System.Collections.Generic;
using Tecnomatix.Engineering;

namespace AutoJTTXCoreUtilities.AJTManipulator
{
    internal class AJTMultiMlmManipulator
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

        //创建
        public void CreateManipulator(TxTransformation location, int count)
        {
            this.DestroyManipulator();

            if (location == null || count < 1)
            {
                return;
            }

            this._manipulators = new List<AJTMlmManipulator>(count);
            for (int i = 0; i < count; i++)
            {
                AJTMlmManipulator cap_Mlm_Manipulator = new AJTMlmManipulator(location);
                cap_Mlm_Manipulator.CreateManipulator();
                this._manipulators.Add(cap_Mlm_Manipulator);
            }
        }

        //销毁
        public void DestroyManipulator()
        {
            if (this._manipulators == null)
            {
                return;
            }
            foreach (AJTMlmManipulator capMlmManipulator in this._manipulators)
            {
                capMlmManipulator.DestroyManipulator();
            }
        }

        private List<AJTMlmManipulator> _manipulators;

        private TxColor _manipulatorColor = TxColor.TxColorBlue;


    }

}
