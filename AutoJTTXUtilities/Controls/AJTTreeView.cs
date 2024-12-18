using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace AutoJTTXUtilities.Controls
{
    public class AJTTreeView : TreeView
    {
        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public event TreeViewEventHandler AfterDeselect;

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public event TreeViewEventHandler BeforeDeselect;

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public event EventHandler SelectionsChanged;

        public new TreeNode SelectedNode
        {
            get
            {
                if (!this.blnInternalCall)
                {
                    throw new NotSupportedException("Use SelectedNodes instead of SelectedNode.");
                }
                return base.SelectedNode;
            }
            set
            {
                if (this.blnInternalCall)
                {
                    try
                    {
                        base.SelectedNode = value;
                        return;
                    }
                    catch
                    {
                        return;
                    }
                }
                throw new NotSupportedException("Use SelectedNodes instead of SelectedNode.");
            }
        }

        public TreeViewSelectionMode SelectionMode
        {
            get
            {
                return this.selectionMode;
            }
            set
            {
                this.selectionMode = value;
            }
        }

        public Color SelectionBackColor
        {
            get
            {
                return this.selectionBackColor;
            }
            set
            {
                this.selectionBackColor = value;
            }
        }

        public NodesCollection SelectedNodes
        {
            get
            {
                NodesCollection nodesCollection = new NodesCollection();
                foreach (object obj in this.htblSelectedNodes.Values)
                {
                    TreeNode treeNode = (TreeNode)obj;
                    if (treeNode.TreeView != null)
                    {
                        nodesCollection.Add(treeNode);
                    }
                }
                nodesCollection.TreeNodeAdded += this.SelectedNodes_TreeNodeAdded;
                nodesCollection.TreeNodeInserted += this.SelectedNodes_TreeNodeInserted;
                nodesCollection.TreeNodeRemoved += this.SelectedNodes_TreeNodeRemoved;
                nodesCollection.SelectedNodesCleared += this.SelectedNodes_SelectedNodesCleared;
                return nodesCollection;
            }
        }

        protected void OnAfterDeselect(TreeNode tn)
        {
            if (this.AfterDeselect != null)
            {
                this.AfterDeselect(this, new TreeViewEventArgs(tn));
            }
        }

        protected void OnBeforeDeselect(TreeNode tn)
        {
            if (this.BeforeDeselect != null)
            {
                this.BeforeDeselect(this, new TreeViewEventArgs(tn));
            }
        }

        protected void OnSelectionsChanged()
        {
            if (this.blnSelectionChanged && this.SelectionsChanged != null)
            {
                this.SelectionsChanged(this, new EventArgs());
            }
        }

        private void SelectedNodes_TreeNodeAdded(TreeNode tn)
        {
            this.blnSelectionChanged = false;
            this.SelectNode(tn, true, TreeViewAction.Unknown);
            this.OnSelectionsChanged();
        }

        private void SelectedNodes_TreeNodeInserted(TreeNode tn)
        {
            this.blnSelectionChanged = false;
            this.SelectNode(tn, true, TreeViewAction.Unknown);
            this.OnSelectionsChanged();
        }

        private void SelectedNodes_TreeNodeRemoved(TreeNode tn)
        {
            this.blnSelectionChanged = false;
            this.SelectNode(tn, false, TreeViewAction.Unknown);
            this.OnSelectionsChanged();
        }

        private void SelectedNodes_SelectedNodesCleared(object sender, EventArgs e)
        {
            this.blnSelectionChanged = false;
            this.UnselectAllNodes(TreeViewAction.Unknown);
            this.OnSelectionsChanged();
        }

        private void UnselectAllNodes(TreeViewAction tva)
        {
            this.UnselectAllNodesExceptNode(null, tva);
        }

        private void UnselectAllNodesNotBelongingToLevel(int level, TreeViewAction tva)
        {
            ArrayList arrayList = new ArrayList();
            foreach (object obj in this.htblSelectedNodes.Values)
            {
                TreeNode treeNode = (TreeNode)obj;
                if (this.GetNodeLevel(treeNode) != level)
                {
                    arrayList.Add(treeNode);
                }
            }
            foreach (object obj2 in arrayList)
            {
                TreeNode tn = (TreeNode)obj2;
                this.SelectNode(tn, false, tva);
            }
        }

        private void UnselectAllNodesNotBelongingDirectlyToParent(TreeNode parent, TreeViewAction tva)
        {
            ArrayList arrayList = new ArrayList();
            foreach (object obj in this.htblSelectedNodes.Values)
            {
                TreeNode treeNode = (TreeNode)obj;
                if (treeNode.Parent != parent)
                {
                    arrayList.Add(treeNode);
                }
            }
            foreach (object obj2 in arrayList)
            {
                TreeNode tn = (TreeNode)obj2;
                this.SelectNode(tn, false, tva);
            }
        }

        private void UnselectAllNodesNotBelongingToParent(TreeNode parent, TreeViewAction tva)
        {
            ArrayList arrayList = new ArrayList();
            foreach (object obj in this.htblSelectedNodes.Values)
            {
                TreeNode treeNode = (TreeNode)obj;
                if (!this.IsChildOf(treeNode, parent))
                {
                    arrayList.Add(treeNode);
                }
            }
            foreach (object obj2 in arrayList)
            {
                TreeNode tn = (TreeNode)obj2;
                this.SelectNode(tn, false, tva);
            }
        }

        private void UnselectAllNodesExceptNode(TreeNode nodeKeepSelected, TreeViewAction tva)
        {
            ArrayList arrayList = new ArrayList();
            foreach (object obj in this.htblSelectedNodes.Values)
            {
                TreeNode treeNode = (TreeNode)obj;
                if (nodeKeepSelected == null)
                {
                    arrayList.Add(treeNode);
                }
                else if (nodeKeepSelected != null && treeNode != nodeKeepSelected)
                {
                    arrayList.Add(treeNode);
                }
            }
            foreach (object obj2 in arrayList)
            {
                TreeNode tn = (TreeNode)obj2;
                this.SelectNode(tn, false, tva);
            }
        }

        protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }

        public void SetNodeForeColor(TreeNode tn, Color color)
        {
            if (tn != null)
            {
                if (!this.IsNodeSelected(tn))
                {
                    tn.ForeColor = color;
                }
                else
                {
                    Color[] array = (Color[])this.htblSelectedNodesOrigColors[tn.GetHashCode()];
                    array[1] = color;
                    this.htblSelectedNodesOrigColors[tn.GetHashCode()] = array;
                }
            }
        }

        public bool IsNodeSelected(TreeNode tn)
        {
            return tn != null && this.htblSelectedNodes.ContainsKey(tn.GetHashCode());
        }

        public bool IsAnyParentNodeSelected(TreeNode tn)
        {
            return tn != null && tn.Parent != null && (this.IsNodeSelected(tn.Parent) || this.IsAnyParentNodeSelected(tn.Parent));
        }

        private void PreserveNodeColors(TreeNode tn)
        {
            if (tn != null && !this.htblSelectedNodesOrigColors.ContainsKey(tn.GetHashCode()))
            {
                this.htblSelectedNodesOrigColors.Add(tn.GetHashCode(), new Color[]
                {
                    tn.BackColor,
                    tn.ForeColor
                });
            }
        }

        private bool SelectNode(TreeNode tn, bool select, TreeViewAction tva)
        {
            bool flag = false;
            bool result;
            if (tn == null)
            {
                result = false;
            }
            else
            {
                if (!select)
                {
                    if (this.IsNodeSelected(tn))
                    {
                        this.OnBeforeDeselect(tn);
                        Color[] array = (Color[])this.htblSelectedNodesOrigColors[tn.GetHashCode()];
                        if (array != null)
                        {
                            this.htblSelectedNodes.Remove(tn.GetHashCode());
                            this.blnSelectionChanged = true;
                            this.htblSelectedNodesOrigColors.Remove(tn.GetHashCode());
                            tn.BackColor = array[0];
                            tn.ForeColor = array[1];
                        }
                        this.OnAfterDeselect(tn);
                    }
                }
                else
                {
                    if (!this.IsNodeSelected(tn))
                    {
                        TreeViewCancelEventArgs treeViewCancelEventArgs = new TreeViewCancelEventArgs(tn, false, tva);
                        base.OnBeforeSelect(treeViewCancelEventArgs);
                        if (treeViewCancelEventArgs.Cancel)
                        {
                            return false;
                        }
                        this.PreserveNodeColors(tn);
                        tn.BackColor = this.SelectionBackColor;
                        tn.ForeColor = this.BackColor;
                        this.htblSelectedNodes.Add(tn.GetHashCode(), tn);
                        flag = true;
                        this.blnSelectionChanged = true;
                        base.OnAfterSelect(new TreeViewEventArgs(tn, tva));
                        tn.BackColor = this.SelectionBackColor;
                        tn.ForeColor = this.BackColor;
                    }
                    this.tnMostRecentSelectedNode = tn;
                }
                result = flag;
            }
            return result;
        }

        private void SelectNodesInsideRange(TreeNode startNode, TreeNode endNode, TreeViewAction tva)
        {
            TreeNode treeNode;
            TreeNode treeNode2;
            if (startNode.Bounds.Y < endNode.Bounds.Y)
            {
                treeNode = startNode;
                treeNode2 = endNode;
            }
            else
            {
                treeNode = endNode;
                treeNode2 = startNode;
            }
            this.SelectNode(treeNode, true, tva);
            TreeNode treeNode3 = treeNode;
            while (treeNode3 != treeNode2)
            {
                treeNode3 = treeNode3.NextVisibleNode;
                if (treeNode3 != null)
                {
                    this.SelectNode(treeNode3, true, tva);
                }
            }
            this.SelectNode(treeNode2, true, tva);
        }

        private void UnselectNodesOutsideRange(TreeNode startNode, TreeNode endNode, TreeViewAction tva)
        {
            TreeNode treeNode;
            TreeNode treeNode2;
            if (startNode.Bounds.Y >= endNode.Bounds.Y)
            {
                treeNode = endNode;
                treeNode2 = startNode;
            }
            else
            {
                treeNode = startNode;
                treeNode2 = endNode;
            }
            TreeNode treeNode3 = treeNode;
            while (treeNode3 != null)
            {
                treeNode3 = treeNode3.PrevVisibleNode;
                if (treeNode3 != null)
                {
                    this.SelectNode(treeNode3, false, tva);
                }
            }
            treeNode3 = treeNode2;
            while (treeNode3 != null)
            {
                treeNode3 = treeNode3.NextVisibleNode;
                if (treeNode3 != null)
                {
                    this.SelectNode(treeNode3, false, tva);
                }
            }
        }

        private void UnselectNodesRecursively(TreeNode tn, TreeViewAction tva)
        {
            this.SelectNode(tn, false, tva);
            foreach (object obj in tn.Nodes)
            {
                TreeNode tn2 = (TreeNode)obj;
                this.UnselectNodesRecursively(tn2, tva);
            }
        }

        private bool IsClickOnNode(TreeNode tn, MouseEventArgs e)
        {
            bool result;
            if (tn != null)
            {
                int num = tn.Bounds.X + tn.Bounds.Width;
                result = (tn != null && e.X < num);
            }
            else
            {
                result = false;
            }
            return result;
        }

        public int GetNodeLevel(TreeNode node)
        {
            int num = 0;
            while ((node = node.Parent) != null)
            {
                num++;
            }
            return num;
        }

        private bool IsChildOf(TreeNode child, TreeNode parent)
        {
            bool result = false;
            for (TreeNode treeNode = child; treeNode != null; treeNode = treeNode.Parent)
            {
                if (treeNode == parent)
                {
                    result = true;
                    return result;
                }
            }
            return result;
        }

        public TreeNode GetRootParent(TreeNode child)
        {
            TreeNode treeNode = child;
            while (treeNode.Parent != null)
            {
                treeNode = treeNode.Parent;
            }
            return treeNode;
        }

        private int GetNumberOfVisibleNodes()
        {
            int num = 0;
            for (TreeNode treeNode = base.Nodes[0]; treeNode != null; treeNode = treeNode.NextVisibleNode)
            {
                if (treeNode.IsVisible)
                {
                    num++;
                }
            }
            return num;
        }

        private TreeNode GetLastVisibleNode()
        {
            TreeNode treeNode = base.Nodes[0];
            while (treeNode.NextVisibleNode != null)
            {
                treeNode = treeNode.NextVisibleNode;
            }
            return treeNode;
        }

        private TreeNode GetNextTreeNode(TreeNode start, bool down, int intNumber)
        {
            TreeNode result;
            if (start == null)
            {
                if (base.Nodes.Count <= 0)
                {
                    result = null;
                }
                else
                {
                    result = base.Nodes[0];
                }
            }
            else
            {
                int i = 0;
                TreeNode treeNode = start;
                while (i < intNumber)
                {
                    if (!down)
                    {
                        if (treeNode.PrevVisibleNode == null)
                        {
                            break;
                        }
                        treeNode = treeNode.PrevVisibleNode;
                    }
                    else
                    {
                        if (treeNode.NextVisibleNode == null)
                        {
                            break;
                        }
                        treeNode = treeNode.NextVisibleNode;
                    }
                    i++;
                }
                result = treeNode;
            }
            return result;
        }

        private void SetFocusToNode(TreeNode tn, bool visible)
        {
            if (tn != null)
            {
                Graphics graphics = base.CreateGraphics();
                Rectangle rectangle = new Rectangle(tn.Bounds.X, tn.Bounds.Y, tn.Bounds.Width, tn.Bounds.Height);
                if (!visible)
                {
                    if (tn.BackColor != this.SelectionBackColor)
                    {
                        graphics.DrawRectangle(new Pen(new SolidBrush(this.BackColor), 1f), this.tnMostRecentSelectedNode.Bounds.X, this.tnMostRecentSelectedNode.Bounds.Y, this.tnMostRecentSelectedNode.Bounds.Width, this.tnMostRecentSelectedNode.Bounds.Height);
                    }
                    base.Invalidate(rectangle, false);
                    base.Update();
                }
                else
                {
                    base.Invalidate(rectangle, false);
                    base.Update();
                    if (tn.BackColor != this.SelectionBackColor)
                    {
                        graphics.DrawRectangle(new Pen(new SolidBrush(this.SelectionBackColor), 1f), rectangle);
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (!this.blnNodeProcessedOnMouseDown)
            {
                TreeNode nodeAt = base.GetNodeAt(e.X, e.Y);
                if (this.IsClickOnNode(nodeAt, e))
                {
                    this.ProcessNodeRange(this.tnMostRecentSelectedNode, nodeAt, e, Control.ModifierKeys, TreeViewAction.ByMouse, true);
                }
            }
            this.blnNodeProcessedOnMouseDown = false;
            base.OnMouseUp(e);
        }

        private bool IsPlusMinusClicked(TreeNode tn, MouseEventArgs e)
        {
            int nodeLevel = this.GetNodeLevel(tn);
            bool result = false;
            if (e.X < 20 + nodeLevel * 20)
            {
                result = true;
            }
            return result;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.tnKeysStartNode = null;
            this.intMouseClicks = e.Clicks;
            TreeNode nodeAt = base.GetNodeAt(e.X, e.Y);
            if (nodeAt != null)
            {
                this.PreserveNodeColors(nodeAt);
                if (!this.IsPlusMinusClicked(nodeAt, e) && nodeAt != null && this.IsClickOnNode(nodeAt, e) && !this.IsNodeSelected(nodeAt))
                {
                    this.tnToFlash = nodeAt;
                    Thread thread = new Thread(new ThreadStart(this.FlashNode));
                    thread.Start();
                    this.blnNodeProcessedOnMouseDown = true;
                    this.ProcessNodeRange(this.tnMostRecentSelectedNode, nodeAt, e, Control.ModifierKeys, TreeViewAction.ByMouse, true);
                }
                base.OnMouseDown(e);
            }
            else
            {
                base.OnMouseDown(e);
            }
        }

        private void FlashNode()
        {
            if (!this.method_0())
            {
                TreeNode treeNode = this.tnToFlash;
                if (!this.IsNodeSelected(treeNode))
                {
                    treeNode.BackColor = this.SelectionBackColor;
                    treeNode.ForeColor = this.BackColor;
                    base.Invalidate();
                    this.Refresh();
                    Application.DoEvents();
                    Thread.Sleep(200);
                }
                if (!this.IsNodeSelected(treeNode))
                {
                    treeNode.BackColor = this.BackColor;
                    treeNode.ForeColor = this.ForeColor;
                }
            }
            else
            {
                base.Invoke(new MethodInvoker(delegate ()
                {
                    this.FlashNode();
                }));
            }
        }

        private void StartEdit()
        {
            Thread.Sleep(200);
            if (this.blnWasDoubleClick)
            {
                this.blnWasDoubleClick = false;
            }
            else
            {
                this.blnInternalCall = true;
                this.SelectedNode = this.tnNodeToStartEditOn;
                this.blnInternalCall = false;
                this.tnNodeToStartEditOn.BeginEdit();
            }
        }

        private void ProcessNodeRange(TreeNode startNode, TreeNode endNode, MouseEventArgs e, Keys keys, TreeViewAction tva, bool allowStartEdit)
        {
            this.blnSelectionChanged = false;
            if (e.Button == MouseButtons.Left)
            {
                this.blnWasDoubleClick = (this.intMouseClicks == 2);
                if ((keys & Keys.Control) == Keys.None && (keys & Keys.Shift) == Keys.None)
                {
                    this.tnSelectionMirrorPoint = endNode;
                    int count = this.SelectedNodes.Count;
                    if (this.blnWasDoubleClick)
                    {
                        base.OnMouseDown(e);
                        return;
                    }
                    if (!this.IsPlusMinusClicked(endNode, e))
                    {
                        bool flag = false;
                        if (this.IsNodeSelected(endNode))
                        {
                            flag = true;
                        }
                        this.UnselectAllNodesExceptNode(endNode, tva);
                        this.SelectNode(endNode, true, tva);
                        if (flag && base.LabelEdit && allowStartEdit && !this.blnWasDoubleClick && count <= 1)
                        {
                            this.tnNodeToStartEditOn = endNode;
                            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                            timer.Interval = 200;
                            timer.Tick += this.Timer_Tick;
                            timer.Start();
                        }
                    }
                }
                else if ((keys & Keys.Control) != Keys.None && (keys & Keys.Shift) == Keys.None)
                {
                    this.tnSelectionMirrorPoint = null;
                    if (!this.IsNodeSelected(endNode))
                    {
                        switch (this.selectionMode)
                        {
                            case TreeViewSelectionMode.SingleSelect:
                                this.UnselectAllNodesExceptNode(endNode, tva);
                                break;
                            case TreeViewSelectionMode.MultiSelectSameRootBranch:
                                {
                                    TreeNode rootParent = this.GetRootParent(endNode);
                                    this.UnselectAllNodesNotBelongingToParent(rootParent, tva);
                                    break;
                                }
                            case TreeViewSelectionMode.MultiSelectSameLevel:
                                this.UnselectAllNodesNotBelongingToLevel(this.GetNodeLevel(endNode), tva);
                                break;
                            case TreeViewSelectionMode.MultiSelectSameLevelAndRootBranch:
                                {
                                    TreeNode rootParent2 = this.GetRootParent(endNode);
                                    this.UnselectAllNodesNotBelongingToParent(rootParent2, tva);
                                    this.UnselectAllNodesNotBelongingToLevel(this.GetNodeLevel(endNode), tva);
                                    break;
                                }
                            case TreeViewSelectionMode.MultiSelectSameParent:
                                {
                                    TreeNode parent = endNode.Parent;
                                    this.UnselectAllNodesNotBelongingDirectlyToParent(parent, tva);
                                    break;
                                }
                        }
                        this.SelectNode(endNode, true, tva);
                    }
                    else
                    {
                        this.SelectNode(endNode, false, tva);
                    }
                }
                else if ((keys & Keys.Control) == Keys.None && (keys & Keys.Shift) > Keys.None)
                {
                    if (this.tnSelectionMirrorPoint == null)
                    {
                        this.tnSelectionMirrorPoint = startNode;
                    }
                    switch (this.selectionMode)
                    {
                        case TreeViewSelectionMode.SingleSelect:
                            this.UnselectAllNodesExceptNode(endNode, tva);
                            this.SelectNode(endNode, true, tva);
                            break;
                        case TreeViewSelectionMode.MultiSelect:
                            this.SelectNodesInsideRange(this.tnSelectionMirrorPoint, endNode, tva);
                            this.UnselectNodesOutsideRange(this.tnSelectionMirrorPoint, endNode, tva);
                            break;
                        case TreeViewSelectionMode.MultiSelectSameRootBranch:
                            {
                                TreeNode rootParent3 = this.GetRootParent(startNode);
                                TreeNode treeNode = startNode;
                                while (treeNode != null && treeNode != endNode)
                                {
                                    if (startNode.Bounds.Y > endNode.Bounds.Y)
                                    {
                                        treeNode = treeNode.PrevVisibleNode;
                                    }
                                    else
                                    {
                                        treeNode = treeNode.NextVisibleNode;
                                    }
                                    if (treeNode != null)
                                    {
                                        TreeNode rootParent4 = this.GetRootParent(treeNode);
                                        if (rootParent4 == rootParent3)
                                        {
                                            this.SelectNode(treeNode, true, tva);
                                        }
                                    }
                                }
                                this.UnselectAllNodesNotBelongingToParent(rootParent3, tva);
                                this.UnselectNodesOutsideRange(this.tnSelectionMirrorPoint, endNode, tva);
                                break;
                            }
                        case TreeViewSelectionMode.MultiSelectSameLevel:
                            {
                                int nodeLevel = this.GetNodeLevel(startNode);
                                TreeNode treeNode2 = startNode;
                                while (treeNode2 != null && treeNode2 != endNode)
                                {
                                    if (startNode.Bounds.Y <= endNode.Bounds.Y)
                                    {
                                        treeNode2 = treeNode2.NextVisibleNode;
                                    }
                                    else
                                    {
                                        treeNode2 = treeNode2.PrevVisibleNode;
                                    }
                                    if (treeNode2 != null)
                                    {
                                        int nodeLevel2 = this.GetNodeLevel(treeNode2);
                                        if (nodeLevel2 == nodeLevel)
                                        {
                                            this.SelectNode(treeNode2, true, tva);
                                        }
                                    }
                                }
                                this.UnselectAllNodesNotBelongingToLevel(nodeLevel, tva);
                                this.UnselectNodesOutsideRange(this.tnSelectionMirrorPoint, endNode, tva);
                                break;
                            }
                        case TreeViewSelectionMode.MultiSelectSameLevelAndRootBranch:
                            {
                                TreeNode rootParent5 = this.GetRootParent(startNode);
                                int nodeLevel3 = this.GetNodeLevel(startNode);
                                TreeNode treeNode3 = startNode;
                                while (treeNode3 != null && treeNode3 != endNode)
                                {
                                    if (startNode.Bounds.Y <= endNode.Bounds.Y)
                                    {
                                        treeNode3 = treeNode3.NextVisibleNode;
                                    }
                                    else
                                    {
                                        treeNode3 = treeNode3.PrevVisibleNode;
                                    }
                                    if (treeNode3 != null)
                                    {
                                        int nodeLevel4 = this.GetNodeLevel(treeNode3);
                                        TreeNode rootParent6 = this.GetRootParent(treeNode3);
                                        if (nodeLevel4 == nodeLevel3 && rootParent6 == rootParent5)
                                        {
                                            this.SelectNode(treeNode3, true, tva);
                                        }
                                    }
                                }
                                this.UnselectAllNodesNotBelongingToParent(rootParent5, tva);
                                this.UnselectAllNodesNotBelongingToLevel(nodeLevel3, tva);
                                this.UnselectNodesOutsideRange(this.tnSelectionMirrorPoint, endNode, tva);
                                break;
                            }
                        case TreeViewSelectionMode.MultiSelectSameParent:
                            {
                                TreeNode parent2 = startNode.Parent;
                                TreeNode treeNode4 = startNode;
                                while (treeNode4 != null && treeNode4 != endNode)
                                {
                                    if (startNode.Bounds.Y <= endNode.Bounds.Y)
                                    {
                                        treeNode4 = treeNode4.NextVisibleNode;
                                    }
                                    else
                                    {
                                        treeNode4 = treeNode4.PrevVisibleNode;
                                    }
                                    if (treeNode4 != null)
                                    {
                                        TreeNode parent3 = treeNode4.Parent;
                                        if (parent3 == parent2)
                                        {
                                            this.SelectNode(treeNode4, true, tva);
                                        }
                                    }
                                }
                                this.UnselectAllNodesNotBelongingDirectlyToParent(parent2, tva);
                                this.UnselectNodesOutsideRange(this.tnSelectionMirrorPoint, endNode, tva);
                                break;
                            }
                    }
                }
                else if ((keys & Keys.Control) != Keys.None && (keys & Keys.Shift) > Keys.None)
                {
                    switch (this.selectionMode)
                    {
                        case TreeViewSelectionMode.SingleSelect:
                            this.UnselectAllNodesExceptNode(endNode, tva);
                            this.SelectNode(endNode, true, tva);
                            break;
                        case TreeViewSelectionMode.MultiSelect:
                            {
                                TreeNode treeNode5 = startNode;
                                while (treeNode5 != null)
                                {
                                    if (treeNode5 == endNode)
                                    {
                                        break;
                                    }
                                    if (startNode.Bounds.Y > endNode.Bounds.Y)
                                    {
                                        treeNode5 = treeNode5.PrevVisibleNode;
                                    }
                                    else
                                    {
                                        treeNode5 = treeNode5.NextVisibleNode;
                                    }
                                    if (treeNode5 != null)
                                    {
                                        this.SelectNode(treeNode5, true, tva);
                                    }
                                }
                                break;
                            }
                        case TreeViewSelectionMode.MultiSelectSameRootBranch:
                            {
                                TreeNode rootParent7 = this.GetRootParent(startNode);
                                TreeNode treeNode6 = startNode;
                                while (treeNode6 != null && treeNode6 != endNode)
                                {
                                    if (startNode.Bounds.Y <= endNode.Bounds.Y)
                                    {
                                        treeNode6 = treeNode6.NextVisibleNode;
                                    }
                                    else
                                    {
                                        treeNode6 = treeNode6.PrevVisibleNode;
                                    }
                                    if (treeNode6 != null)
                                    {
                                        TreeNode rootParent8 = this.GetRootParent(treeNode6);
                                        if (rootParent8 == rootParent7)
                                        {
                                            this.SelectNode(treeNode6, true, tva);
                                        }
                                    }
                                }
                                this.UnselectAllNodesNotBelongingToParent(rootParent7, tva);
                                break;
                            }
                        case TreeViewSelectionMode.MultiSelectSameLevel:
                            {
                                int nodeLevel5 = this.GetNodeLevel(startNode);
                                TreeNode treeNode7 = startNode;
                                while (treeNode7 != null && treeNode7 != endNode)
                                {
                                    if (startNode.Bounds.Y > endNode.Bounds.Y)
                                    {
                                        treeNode7 = treeNode7.PrevVisibleNode;
                                    }
                                    else
                                    {
                                        treeNode7 = treeNode7.NextVisibleNode;
                                    }
                                    if (treeNode7 != null)
                                    {
                                        int nodeLevel6 = this.GetNodeLevel(treeNode7);
                                        if (nodeLevel6 == nodeLevel5)
                                        {
                                            this.SelectNode(treeNode7, true, tva);
                                        }
                                    }
                                }
                                this.UnselectAllNodesNotBelongingToLevel(nodeLevel5, tva);
                                break;
                            }
                        case TreeViewSelectionMode.MultiSelectSameLevelAndRootBranch:
                            {
                                TreeNode rootParent9 = this.GetRootParent(startNode);
                                int nodeLevel7 = this.GetNodeLevel(startNode);
                                TreeNode treeNode8 = startNode;
                                while (treeNode8 != null && treeNode8 != endNode)
                                {
                                    if (startNode.Bounds.Y > endNode.Bounds.Y)
                                    {
                                        treeNode8 = treeNode8.PrevVisibleNode;
                                    }
                                    else
                                    {
                                        treeNode8 = treeNode8.NextVisibleNode;
                                    }
                                    if (treeNode8 != null)
                                    {
                                        int nodeLevel8 = this.GetNodeLevel(treeNode8);
                                        TreeNode rootParent10 = this.GetRootParent(treeNode8);
                                        if (nodeLevel8 == nodeLevel7 && rootParent10 == rootParent9)
                                        {
                                            this.SelectNode(treeNode8, true, tva);
                                        }
                                    }
                                }
                                this.UnselectAllNodesNotBelongingToParent(rootParent9, tva);
                                this.UnselectAllNodesNotBelongingToLevel(nodeLevel7, tva);
                                break;
                            }
                        case TreeViewSelectionMode.MultiSelectSameParent:
                            {
                                TreeNode parent4 = startNode.Parent;
                                TreeNode treeNode9 = startNode;
                                while (treeNode9 != null && treeNode9 != endNode)
                                {
                                    if (startNode.Bounds.Y <= endNode.Bounds.Y)
                                    {
                                        treeNode9 = treeNode9.NextVisibleNode;
                                    }
                                    else
                                    {
                                        treeNode9 = treeNode9.PrevVisibleNode;
                                    }
                                    if (treeNode9 != null)
                                    {
                                        TreeNode parent5 = treeNode9.Parent;
                                        if (parent5 == parent4)
                                        {
                                            this.SelectNode(treeNode9, true, tva);
                                        }
                                    }
                                }
                                this.UnselectAllNodesNotBelongingDirectlyToParent(parent4, tva);
                                break;
                            }
                    }
                }
            }
            else if (e.Button == MouseButtons.Right && !this.IsNodeSelected(endNode))
            {
                this.UnselectAllNodes(tva);
                this.SelectNode(endNode, true, tva);
            }
            this.OnSelectionsChanged();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.StartEdit();
            ((System.Windows.Forms.Timer)sender).Stop();
        }

        protected override void OnBeforeLabelEdit(NodeLabelEditEventArgs e)
        {
            this.blnSelectionChanged = false;
            this.SelectNode(e.Node, true, TreeViewAction.ByMouse);
            this.UnselectAllNodesExceptNode(e.Node, TreeViewAction.ByMouse);
            this.OnSelectionsChanged();
            base.OnBeforeLabelEdit(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            Keys keys = Keys.None;
            Keys modifiers = e.Modifiers;
            if (modifiers != Keys.Shift && modifiers != Keys.Control && modifiers != (Keys.Shift | Keys.Control))
            {
                this.tnKeysStartNode = null;
            }
            else
            {
                keys = Keys.Shift;
                if (this.tnKeysStartNode == null)
                {
                    this.tnKeysStartNode = this.tnMostRecentSelectedNode;
                }
            }
            int num = 0;
            TreeNode treeNode = null;
            Keys keyCode = e.KeyCode;
            Keys keys2 = keyCode;
            switch (keys2)
            {
                case Keys.Prior:
                    num = this.GetNumberOfVisibleNodes();
                    treeNode = this.GetNextTreeNode(this.tnMostRecentSelectedNode, false, num);
                    break;
                case Keys.Next:
                    num = this.GetNumberOfVisibleNodes();
                    treeNode = this.GetNextTreeNode(this.tnMostRecentSelectedNode, true, num);
                    break;
                case Keys.End:
                    treeNode = this.GetLastVisibleNode();
                    break;
                case Keys.Home:
                    treeNode = base.Nodes[0];
                    break;
                case Keys.Left:
                    {
                        TreeNode treeNode2 = this.tnMostRecentSelectedNode;
                        if (treeNode2 != null)
                        {
                            treeNode2.Collapse();
                        }
                        break;
                    }
                case Keys.Up:
                    if (this.tnMostRecentSelectedNode != null)
                    {
                        TreeNode treeNode3 = this.tnMostRecentSelectedNode;
                        treeNode = ((treeNode3 != null) ? treeNode3.PrevVisibleNode : null);
                    }
                    else if (base.Nodes.Count > 0)
                    {
                        treeNode = base.Nodes[0];
                    }
                    break;
                case Keys.Right:
                    {
                        TreeNode treeNode4 = this.tnMostRecentSelectedNode;
                        if (treeNode4 != null)
                        {
                            treeNode4.Expand();
                        }
                        break;
                    }
                case Keys.Down:
                    if (this.tnMostRecentSelectedNode != null)
                    {
                        TreeNode treeNode5 = this.tnMostRecentSelectedNode;
                        treeNode = ((treeNode5 != null) ? treeNode5.NextVisibleNode : null);
                    }
                    else if (base.Nodes.Count > 0)
                    {
                        treeNode = base.Nodes[0];
                    }
                    break;
                default:
                    if (keys2 != Keys.F2)
                    {
                        base.OnKeyDown(e);
                        return;
                    }
                    treeNode = this.tnMostRecentSelectedNode;
                    break;
            }
            if (treeNode != null)
            {
                this.SetFocusToNode(this.tnMostRecentSelectedNode, false);
                this.ProcessNodeRange(this.tnKeysStartNode, treeNode, new MouseEventArgs(MouseButtons.Left, 1, Cursor.Position.X, Cursor.Position.Y, 0), keys, TreeViewAction.ByKeyboard, false);
                this.tnMostRecentSelectedNode = treeNode;
                this.SetFocusToNode(this.tnMostRecentSelectedNode, true);
            }
            if (this.tnMostRecentSelectedNode != null)
            {
                TreeNode treeNode6 = null;
                Keys keyCode2 = e.KeyCode;
                Keys keys3 = keyCode2;
                switch (keys3)
                {
                    case Keys.Prior:
                        treeNode6 = this.GetNextTreeNode(this.tnMostRecentSelectedNode, false, num - 2);
                        break;
                    case Keys.Next:
                        treeNode6 = this.GetNextTreeNode(this.tnMostRecentSelectedNode, true, num - 2);
                        break;
                    case Keys.End:
                    case Keys.Home:
                        treeNode6 = this.tnMostRecentSelectedNode;
                        break;
                    case Keys.Left:
                    case Keys.Right:
                        break;
                    case Keys.Up:
                        treeNode6 = this.GetNextTreeNode(this.tnMostRecentSelectedNode, false, 5);
                        break;
                    case Keys.Down:
                        treeNode6 = this.GetNextTreeNode(this.tnMostRecentSelectedNode, true, 5);
                        break;
                    default:
                        if (keys3 == Keys.F2)
                        {
                            this.tnMostRecentSelectedNode.BeginEdit();
                        }
                        break;
                }
                if (treeNode6 != null)
                {
                    treeNode6.EnsureVisible();
                }
            }
            base.OnKeyDown(e);
        }

        protected override void OnAfterCollapse(TreeViewEventArgs e)
        {
            this.blnSelectionChanged = false;
            bool flag = false;
            foreach (object obj in e.Node.Nodes)
            {
                TreeNode tn = (TreeNode)obj;
                if (this.IsNodeSelected(tn))
                {
                    flag = true;
                }
                this.UnselectNodesRecursively(tn, TreeViewAction.Collapse);
            }
            if (flag)
            {
                this.SelectNode(e.Node, true, TreeViewAction.Collapse);
            }
            this.OnSelectionsChanged();
            base.OnAfterCollapse(e);
        }

        /*
		protected override void OnItemDrag(ItemDragEventArgs e)
		{
			e = new ItemDragEventArgs(MouseButtons.Left, this.SelectedNodes);
			base.OnItemDrag(e);
		}
		*/
        bool method_0()
        {
            return base.InvokeRequired;
        }

        private Container components;

        private bool blnInternalCall;

        private Hashtable htblSelectedNodes = new Hashtable();

        private bool blnSelectionChanged;

        private Hashtable htblSelectedNodesOrigColors = new Hashtable();

        private TreeNode tnNodeToStartEditOn;

        private bool blnWasDoubleClick;

        private TreeNode tnMostRecentSelectedNode;

        private TreeNode tnSelectionMirrorPoint;

        private int intMouseClicks;

        private TreeViewSelectionMode selectionMode;

        private Color selectionBackColor = SystemColors.Highlight;

        private bool blnNodeProcessedOnMouseDown;

        private TreeNode tnToFlash;

        private TreeNode tnKeysStartNode;
    }
}
