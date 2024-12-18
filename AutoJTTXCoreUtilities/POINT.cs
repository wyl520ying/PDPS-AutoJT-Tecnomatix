using System.Drawing;

namespace AutoJTTXCoreUtilities
{
    internal struct POINT
    {
        public POINT(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public POINT(Point point)
        {
            this.x = point.X;
            this.y = point.Y;
        }

        public int x;

        public int y;
    }
}