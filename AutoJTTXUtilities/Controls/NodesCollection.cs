using System;
using System.Collections;
using System.Windows.Forms;

namespace AutoJTTXUtilities.Controls
{
    public class NodesCollection : CollectionBase
    {
        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal event TreeNodeEventHandler TreeNodeAdded;

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal event TreeNodeEventHandler TreeNodeRemoved;

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal event TreeNodeEventHandler TreeNodeInserted;

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal event EventHandler SelectedNodesCleared;

        public TreeNode this[int index]
        {
            get
            {
                return (TreeNode)base.List[index];
            }
        }

        public int Add(TreeNode treeNode)
        {
            if (this.TreeNodeAdded != null)
            {
                this.TreeNodeAdded(treeNode);
            }
            return base.List.Add(treeNode);
        }

        public void Insert(int index, TreeNode treeNode)
        {
            if (this.TreeNodeInserted != null)
            {
                this.TreeNodeInserted(treeNode);
            }
            base.List.Add(treeNode);
        }

        public void Remove(TreeNode treeNode)
        {
            if (this.TreeNodeRemoved != null)
            {
                this.TreeNodeRemoved(treeNode);
            }
            if (base.List.Contains(treeNode))
            {
                base.List.Remove(treeNode);
            }
        }

        public bool Contains(TreeNode treeNode)
        {
            return base.List.Contains(treeNode);
        }

        public int IndexOf(TreeNode treeNode)
        {
            return base.List.IndexOf(treeNode);
        }

        protected override void OnClear()
        {
            if (this.SelectedNodesCleared != null)
            {
                this.SelectedNodesCleared(this, EventArgs.Empty);
            }
            base.OnClear();
        }
    }

}
