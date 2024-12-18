using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Collections;
using System.Collections.Generic;

namespace AutoJTTXUtilities.Controls.TreeViewExt
{
    public sealed class MultiSelectTreeView : TreeView
    {
        #region Fields

        // 用于 shift 选择
        private TreeViewItem _lastItemSelected;

        #endregion Fields
        #region Dependency Properties

        public static readonly DependencyProperty IsItemSelectedProperty =
            DependencyProperty.RegisterAttached("IsItemSelected", typeof(bool), typeof(MultiSelectTreeView));

        //将选中得对象加入选择集
        public static void SetIsItemSelected(UIElement element, bool value)
        {
            element.SetValue(IsItemSelectedProperty, value);
        }
        public static bool GetIsItemSelected(UIElement element)
        {
            return (bool)element.GetValue(IsItemSelectedProperty);
        }

        #endregion Dependency Properties
        #region Properties

        private static bool IsCtrlPressed
        {
            //不用按ctrl就可以选择
            get { return true; }//return Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
        }
        private static bool IsShiftPressed
        {
            //取消shift按键监控
            get { return false; }//return Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
        }

        public IList SelectedItems
        {
            get
            {
                var selectedTreeViewItems = GetTreeViewItems(this, true).Where(GetIsItemSelected);
                var selectedModelItems = selectedTreeViewItems.Select(treeViewItem => treeViewItem.Header);

                return selectedModelItems.ToList();
            }
        }

        #endregion Properties
        #region Event Handlers



        /*
        protected override void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
        {
            base.OnSelectedItemChanged(e);

            // 查找与新选中项关联的 TreeViewItem
            TreeViewItem selectedItemContainer = GetTreeViewItemFromObject(e.NewValue);

            if (selectedItemContainer != null)
            {
                // 取消自动滚动行为
                selectedItemContainer.BringIntoView();  // 你可以选择是否调用此方法
            }
        }
        private TreeViewItem GetTreeViewItemFromObject(object data)
        {
            // 在 TreeView 控件中找到与数据项关联的 TreeViewItem
            return GetTreeViewItemRecursive(this, data);
        }
        private TreeViewItem GetTreeViewItemRecursive(ItemsControl parent, object data)
        {
            if (parent == null)
                return null;

            // 遍历当前 ItemsControl 的所有项
            for (int i = 0; i < parent.Items.Count; i++)
            {
                var item = parent.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem;

                if (item != null)
                {
                    // 如果当前项对应的数据项是我们要找的
                    if (item.DataContext == data)
                        return item;

                    // 递归查找子项
                    TreeViewItem childItem = GetTreeViewItemRecursive(item, data);
                    if (childItem != null)
                        return childItem;
                }
            }

            return null;
        }        
        */





        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);

            // 如果单击树枝扩展器...
            if (e.OriginalSource is Shape || e.OriginalSource is Grid || e.OriginalSource is Border)
                return;

            var item = GetTreeViewItemClicked((FrameworkElement)e.OriginalSource);
            if (item != null) SelectedItemChangedInternal(item);
        }

        #endregion Event Handlers
        #region Utility Methods

        private void SelectedItemChangedInternal(TreeViewItem tvItem)
        {
            // 如果没有按住 ctrl 键，则清除所有先前选定的项目状态
            if (!IsCtrlPressed)
            {
                var items = GetTreeViewItems(this, true);
                foreach (var treeViewItem in items)
                    SetIsItemSelected(treeViewItem, false);
            }

            // 这是一个项目范围选择吗？
            if (IsShiftPressed && _lastItemSelected != null)
            {
                var items = GetTreeViewItemRange(_lastItemSelected, tvItem);
                if (items.Count > 0)
                {
                    foreach (var treeViewItem in items)
                        SetIsItemSelected(treeViewItem, true);

                    _lastItemSelected = items.Last();
                }
            }
            // 否则，单个选择
            else
            {
                //判断是否重复选中了同一个对象
                //选中同一个对象就删除选中
                bool isflag = false;
                var items = this.SelectedItems;
                for (int k = items.Count - 1; k >= 0; k--)
                {
                    var treeViewItem = items[k];
                    if (treeViewItem == tvItem.DataContext)
                    {
                        SetIsItemSelected(tvItem, false);
                        isflag = true;
                        break;
                    }
                }

                if (!isflag)
                {
                    SetIsItemSelected(tvItem, true);
                    _lastItemSelected = tvItem;
                }
            }
        }

        //移除指定的选择项
        public void RemoveAt(TreeViewItem framework)
        {
            if (framework != null)
            {
                SetIsItemSelected(framework, false);
            }
        }

        //清空选择
        public void Clear()
        {
            var items = GetTreeViewItems(this, true);
            foreach (var treeViewItem in items)
                SetIsItemSelected(treeViewItem, false);
        }

        //获取树item被单击
        public static TreeViewItem GetTreeViewItemClicked(DependencyObject sender)
        {
            while (sender != null && !(sender is TreeViewItem))
                sender = VisualTreeHelper.GetParent(sender);
            return sender as TreeViewItem;
        }
        private static List<TreeViewItem> GetTreeViewItems(ItemsControl parentItem, bool includeCollapsedItems, List<TreeViewItem> itemList = null)
        {
            if (itemList == null)
                itemList = new List<TreeViewItem>();

            for (var index = 0; index < parentItem.Items.Count; index++)
            {
                var tvItem = parentItem.ItemContainerGenerator.ContainerFromIndex(index) as TreeViewItem;
                if (tvItem == null) continue;

                itemList.Add(tvItem);
                if (includeCollapsedItems || tvItem.IsExpanded)
                    GetTreeViewItems(tvItem, includeCollapsedItems, itemList);
            }
            return itemList;
        }
        private List<TreeViewItem> GetTreeViewItemRange(TreeViewItem start, TreeViewItem end)
        {
            var items = GetTreeViewItems(this, false);

            var startIndex = items.IndexOf(start);
            var endIndex = items.IndexOf(end);
            var rangeStart = startIndex > endIndex || startIndex == -1 ? endIndex : startIndex;
            var rangeCount = startIndex > endIndex ? startIndex - endIndex + 1 : endIndex - startIndex + 1;

            if (startIndex == -1 && endIndex == -1)
                rangeCount = 0;

            else if (startIndex == -1 || endIndex == -1)
                rangeCount = 1;

            return rangeCount > 0 ? items.GetRange(rangeStart, rangeCount) : new List<TreeViewItem>();
        }

        #endregion Utility Methods
    }
}