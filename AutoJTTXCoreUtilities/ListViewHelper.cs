using System.Collections;
using System.Windows.Forms;


namespace AutoJTTXCoreUtilities
{
    public class ListViewItemComparer : IComparer
    {
        public ListViewItemComparer()
        {
            this.col = 0;
            this.order = SortOrder.Ascending;
        }

        public ListViewItemComparer(int column, SortOrder order)
        {
            this.col = column;
            this.order = order;
        }

        public int Compare(object x, object y)
        {
            int returnVal = string.Compare(((ListViewItem)x).SubItems[this.col].Text, ((ListViewItem)y).SubItems[this.col].Text);
            bool flag = this.order == SortOrder.Descending;
            checked
            {
                if (flag)
                {
                    returnVal *= -1;
                }
                return returnVal;
            }
        }

        private int col;

        private SortOrder order;
    }

    /// <summary>
    /// ListViewSorter	
    /// </summary>
    public class ListViewSorter : IComparer
    {
        /*
		protected override void OnColumnClick(ColumnClickEventArgs e)
		{
			if (this.EnableColumnSort)
			{
				ListViewSorter listViewSorter = new ListViewSorter();
				base.ListViewItemSorter = listViewSorter;
				if (!(base.ListViewItemSorter is ListViewSorter))
				{
					return;
				}
				if (ListViewSorter.LastSort == e.Column)
				{
					if (base.Sorting == SortOrder.Ascending)
					{
						base.Sorting = SortOrder.Descending;
						ListViewEx.SetColumnHeaderArrow(this, e.Column, ListViewSorter.LastSort, true);
					}
					else
					{
						base.Sorting = SortOrder.Ascending;
						ListViewEx.SetColumnHeaderArrow(this, e.Column, ListViewSorter.LastSort, false);
					}
				}
				else
				{
					base.Sorting = SortOrder.Descending;
					ListViewEx.SetColumnHeaderArrow(this, e.Column, ListViewSorter.LastSort, true);
				}
				listViewSorter.ByColumn = e.Column;
				ListViewSorter.LastSort = e.Column;
				base.Sort();
			}
		}
		*/
        public int Compare(object o1, object o2)
        {
            if (!(o1 is ListViewItem))
            {
                return 0;
            }
            if (!(o2 is ListViewItem))
            {
                return 0;
            }
            ListViewItem listViewItem = (ListViewItem)o2;
            string text = listViewItem.SubItems[this.ByColumn].Text;
            string text2 = ((ListViewItem)o1).SubItems[this.ByColumn].Text;
            int result;
            if (listViewItem.ListView.Sorting == SortOrder.Ascending)
            {
                result = string.Compare(text, text2);
            }
            else
            {
                result = string.Compare(text2, text);
            }
            return result;
        }

        public int ByColumn
        {
            get
            {
                return this.Column;
            }
            set
            {
                this.Column = value;
            }
        }

        public static int LastSort { get; set; }

        private int Column;
    }

}
