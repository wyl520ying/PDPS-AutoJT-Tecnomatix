using Tecnomatix.Engineering;

namespace AutoJTTXCoreUtilities.AJTManipulator
{
    internal class AJTViaPreviwer
    {
        public int Count
        {
            get
            {
                return this._count;
            }
            set
            {
                if (this._count != value)
                {
                    this._count = value;
                    this.SetOperationIndication();
                }
            }
        }

        public TxTransformation Delta
        {
            get
            {
                return this._delta;
            }
            set
            {
                if (this._delta != value)
                {
                    this._delta = value;
                    this.SetOperationIndication();
                }
            }
        }

        public AJTViaPreviwer(TxTransformation via)
        {
            this._via = via;
        }

        public void Destroy()
        {
            if (!this._isDestoyed)
            {
                this._isDestoyed = true;
                this._viaManipulator.DestroyManipulator();
            }
        }

        private void SetOperationIndication()
        {
            if (!this._isDestoyed)
            {
                this.SetLocation();
                this._viaManipulator.CreateManipulator(this._location, this.Delta, this.Count);
            }
        }

        private void SetLocation()
        {
            this._location = null;
            if (this.Delta != null)
            {
                this._location = this.Delta * this._via;
            }
        }

        private readonly TxTransformation _via;

        private TxTransformation _location;

        private int _count;

        private TxTransformation _delta;

        private readonly AJTMultiViaManipulator _viaManipulator = new AJTMultiViaManipulator();

        private bool _isDestoyed;
    }

}
