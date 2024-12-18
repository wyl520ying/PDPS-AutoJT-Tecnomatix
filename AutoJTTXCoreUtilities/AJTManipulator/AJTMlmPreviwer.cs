using Tecnomatix.Engineering;

namespace AutoJTTXCoreUtilities.AJTManipulator
{
    public class AJTMlmPreviwer
    {
        private TxTransformation _Mlm;

        private AJTMultiMlmManipulator _MlmManipulator = new AJTMultiMlmManipulator();
        //是否已经销毁,初始值是true
        private bool _isDestoyed = true;

        //销毁
        public void Destroy()
        {
            if (!this._isDestoyed)
            {
                this._isDestoyed = true;
                this._MlmManipulator.DestroyManipulator();
            }
        }

        //创建
        public void SetOperationIndication(TxTransformation mlm)
        {
            this._Mlm = mlm;
            //销毁
            Destroy();
            this._MlmManipulator.CreateManipulator(_Mlm, 1);
            TxApplication.RefreshDisplay();
            //没有销毁
            _isDestoyed = false;
        }
    }
}
