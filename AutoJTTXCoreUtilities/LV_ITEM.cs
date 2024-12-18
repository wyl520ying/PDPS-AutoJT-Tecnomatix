using System;

namespace AutoJTTXCoreUtilities
{
    public struct LV_ITEM
    {
        public uint mask;

        public int iItem;

        public int iSubItem;

        public uint state;

        public uint stateMask;

        public string pszText;

        public int cchTextMax;

        public int iImage;

        public IntPtr lParam;
    }
}
