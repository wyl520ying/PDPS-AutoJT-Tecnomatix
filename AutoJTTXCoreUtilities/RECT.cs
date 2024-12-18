using System;

namespace AutoJTTXCoreUtilities
{
    internal struct RECT
    {
        public POINT Location
        {
            get
            {
                return new POINT(this.left, this.top);
            }
            set
            {
                this.right -= this.left - value.x;
                this.bottom -= this.bottom - value.y;
                this.left = value.x;
                this.top = value.y;
            }
        }

        internal uint Width
        {
            get
            {
                return (uint)Math.Abs(this.right - this.left);
            }
            set
            {
                this.right = this.left + (int)value;
            }
        }

        internal uint Height
        {
            get
            {
                return (uint)Math.Abs(this.bottom - this.top);
            }
            set
            {
                this.bottom = this.top + (int)value;
            }
        }

        public override string ToString()
        {
            return string.Concat(new object[]
            {
                this.left,
                ":",
                this.top,
                ":",
                this.right,
                ":",
                this.bottom
            });
        }

        public int left;

        public int top;

        public int right;

        public int bottom;
    }

}
