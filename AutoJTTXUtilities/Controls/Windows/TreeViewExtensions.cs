using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AutoJTTXUtilities.Controls.Windows
{
    public class TreeViewExtensions : DependencyObject
    {
        /// <summary>
        /// Gets the value of the dependency property "EnableMultiSelect".
        /// </summary>
        /// <param name="obj">Dependency Object</param>
        /// <returns></returns>
        public static bool GetEnableMultiSelect(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableMultiSelectProperty);
        }

        /// <summary>
        /// Sets the value of the dependency property "EnableMultiSelect".
        /// </summary>
        /// <param name="obj">Dependency Object</param>
        /// <param name="value"></param>
        public static void SetEnableMultiSelect(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableMultiSelectProperty, value);
        }

        // Using a DependencyProperty as the backing store for EnableMultiSelect.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableMultiSelectProperty =
            DependencyProperty.RegisterAttached("EnableMultiSelect", typeof(bool), typeof(TreeViewExtensions), new FrameworkPropertyMetadata(false)
            {
                PropertyChangedCallback = EnableMultiSelectChanged,
                BindsTwoWayByDefault = true
            });

        /// <summary>
        /// Gets the value of the dependency property "SelectedItems".
        /// </summary>
        /// <param name="obj">Dependency Object</param>
        /// <returns></returns>
        public static IList GetSelectedItems(DependencyObject obj)
        {
            return (IList)obj.GetValue(SelectedItemsProperty);
        }

        /// <summary>
        /// Sets the value of the dependency property "SelectedItems".
        /// </summary>
        /// <param name="obj">Dependency Object</param>
        /// <param name="value"></param>
        public static void SetSelectedItems(DependencyObject obj, IList value)
        {
            obj.SetValue(SelectedItemsProperty, value);
        }

        // Using a DependencyProperty as the backing store for SelectedItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached("SelectedItems", typeof(IList), typeof(TreeViewExtensions), new PropertyMetadata(null));

        /// <summary>
        /// Gets the value of the dependency property "AnchorItem".
        /// </summary>
        /// <param name="obj">Dependency Object</param>
        /// <returns></returns>
        static TreeViewItem GetAnchorItem(DependencyObject obj)
        {
            return (TreeViewItem)obj.GetValue(AnchorItemProperty);
        }

        /// <summary>
        /// Sets the value of the dependency property "AnchorItem".
        /// </summary>
        /// <param name="obj">Dependency Object</param>
        /// <param name="value"></param>
        static void SetAnchorItem(DependencyObject obj, TreeViewItem value)
        {
            obj.SetValue(AnchorItemProperty, value);
        }

        // Using a DependencyProperty as the backing store for AnchorItem.  This enables animation, styling, binding, etc...
        static readonly DependencyProperty AnchorItemProperty =
            DependencyProperty.RegisterAttached("AnchorItem", typeof(TreeViewItem), typeof(TreeViewExtensions), new PropertyMetadata(null));

        /// <summary>
        /// Gets the value of the dependency property "IsSelected".
        /// </summary>
        /// <param name="obj">Dependency Object</param>
        /// <returns></returns>
        public static bool GetIsSelected(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsSelectedProperty);
        }

        /// <summary>
        /// Sets the value of the dependency property "IsSelected".
        /// </summary>
        /// <param name="obj">Dependency Object</param>
        /// <param name="value"></param>
        public static void SetIsSelected(DependencyObject obj, bool value)
        {
            if (value)
            {
                GradientStopCollection gradientStopCollection = new GradientStopCollection();
                gradientStopCollection.Add(new GradientStop()
                {
                    Color = (Color)ColorConverter.ConvertFromString("#FFC7DFFC"),
                    Offset = 1
                });
                gradientStopCollection.Add(new GradientStop()
                {
                    Color = (Color)ColorConverter.ConvertFromString("#FF3832B8"),
                    Offset = 1
                });
                LinearGradientBrush brush = new LinearGradientBrush(gradientStopCollection, new Point(0.5, 0), new Point(0.5, 1));
                (obj as TreeViewItem).Background = brush;
            }
            else
            {
                (obj as TreeViewItem).Background = Brushes.Transparent;
            }
            obj.SetValue(IsSelectedProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.RegisterAttached("IsSelected", typeof(bool), typeof(TreeViewExtensions), new PropertyMetadata(false)
            {
                PropertyChangedCallback = RealSelectedChanged
            });

        /// <summary>
        /// "EnableMultiSelect" changed event.
        /// </summary>
        /// <param name="s">Dependency Object</param>
        /// <param name="args">Event parameter</param>
        static void EnableMultiSelectChanged(DependencyObject s, DependencyPropertyChangedEventArgs args)
        {
            TreeView tree = (TreeView)s;
            var wasEnable = (bool)args.OldValue;
            var isEnabled = (bool)args.NewValue;
            if (wasEnable)
            {
                tree.RemoveHandler(TreeViewItem.MouseDownEvent, new MouseButtonEventHandler(ItemClicked));
                tree.RemoveHandler(TreeView.KeyDownEvent, new KeyEventHandler(KeyDown));
            }
            if (isEnabled)
            {
                tree.AddHandler(TreeViewItem.MouseDownEvent, new MouseButtonEventHandler(ItemClicked), true);
                tree.AddHandler(TreeView.KeyDownEvent, new KeyEventHandler(KeyDown));
            }
        }

        /// <summary>
        /// Gets TreeView which contains the TreeViewItem.
        /// </summary>
        /// <param name="item">item</param>
        /// <returns>TreeView</returns>
        static TreeView GetTree(TreeViewItem item)
        {
            Func<DependencyObject, DependencyObject> getParent = (o) => VisualTreeHelper.GetParent(o);
            FrameworkElement currentItem = item;
            while (!(getParent(currentItem) is TreeView))
            {
                currentItem = (FrameworkElement)getParent(currentItem);
            }
            return (TreeView)getParent(currentItem);
        }

        /// <summary>
        /// TreeViewItem seleted changed event.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="args">event parameter</param>
        static void RealSelectedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            TreeViewItem item = (TreeViewItem)sender;
            var selectedItems = GetSelectedItems(GetTree(item));
            if (selectedItems != null)
            {
                var isSelected = GetIsSelected(item);
                if (isSelected)
                {
                    try
                    {
                        selectedItems.Add(item.Header);
                    }
                    catch (ArgumentException)
                    {
                    }
                }
                else
                {
                    selectedItems.Remove(item.Header);
                }
            }
        }

        /// <summary>
        /// Key down event.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event parameter</param>
        static void KeyDown(object sender, KeyEventArgs e)
        {
            TreeView tree = (TreeView)sender;
            if (e.Key == Key.A && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                foreach (var item in GetExpandedTreeViewItems(tree))
                {
                    SetIsSelected(item, true);
                }
                e.Handled = true;
            }
        }

        /// <summary>
        /// Item clicked event.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event parameter</param>
        static void ItemClicked(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = FindTreeViewItem(e.OriginalSource);
            if (item == null)
            {
                return;
            }
            TreeView tree = (TreeView)sender;

            var mouseButton = e.ChangedButton;
            if (mouseButton != MouseButton.Left)
            {
                if ((mouseButton == MouseButton.Right) && ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) == ModifierKeys.None))
                {
                    if (GetIsSelected(item))
                    {
                        UpdateAnchorAndActionItem(tree, item);
                        return;
                    }
                    MakeSingleSelection(tree, item);
                }
                return;
            }
            if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != (ModifierKeys.Shift | ModifierKeys.Control))
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    MakeToggleSelection(tree, item);
                    return;
                }
                if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                {
                    MakeAnchorSelection(tree, item, true);
                    return;
                }
                MakeSingleSelection(tree, item);
                return;
            }
        }

        /// <summary>
        /// Find TreeViewItem which contains the object.
        /// </summary>
        /// <param name="obj">obj</param>
        /// <returns></returns>
        private static TreeViewItem FindTreeViewItem(object obj)
        {
            DependencyObject dpObj = obj as DependencyObject;
            if (dpObj == null)
            {
                return null;
            }
            if (dpObj is TreeViewItem)
            {
                return (TreeViewItem)dpObj;
            }
            return FindTreeViewItem(VisualTreeHelper.GetParent(dpObj));
        }

        /// <summary>
        /// Gets all expanded TreeViewItems.
        /// </summary>
        /// <param name="tree">TreeView</param>
        /// <returns></returns>
        private static IEnumerable<TreeViewItem> GetExpandedTreeViewItems(ItemsControl tree)
        {
            for (int i = 0; i < tree.Items.Count; i++)
            {
                var item = (TreeViewItem)tree.ItemContainerGenerator.ContainerFromIndex(i);
                if (item == null)
                {
                    continue;
                }
                yield return item;
                if (item.IsExpanded)
                {
                    foreach (var subItem in GetExpandedTreeViewItems(item))
                    {
                        yield return subItem;
                    }
                }
            }
        }

        /// <summary>
        /// Select by Shift key.
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="actionItem"></param>
        /// <param name="clearCurrent"></param>
        private static void MakeAnchorSelection(TreeView tree, TreeViewItem actionItem, bool clearCurrent)
        {
            if (GetAnchorItem(tree) == null)
            {
                var selectedItems = GetSelectedTreeViewItems(tree);
                if (selectedItems.Count > 0)
                {
                    SetAnchorItem(tree, selectedItems[selectedItems.Count - 1]);
                }
                else
                {
                    SetAnchorItem(tree, GetExpandedTreeViewItems(tree).Skip(3).FirstOrDefault());
                }
                if (GetAnchorItem(tree) == null)
                {
                    return;
                }
            }

            var anchor = GetAnchorItem(tree);

            var items = GetExpandedTreeViewItems(tree);
            bool betweenBoundary = false;
            foreach (var item in items)
            {
                bool isBoundary = item == anchor || item == actionItem;
                if (isBoundary)
                {
                    betweenBoundary = !betweenBoundary;
                }
                if (betweenBoundary || isBoundary)
                {
                    SetIsSelected(item, true);
                }
                else
                {
                    if (clearCurrent)
                    {
                        SetIsSelected(item, false);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Gets all selected TreeViewItems.
        /// </summary>
        /// <param name="tree">TreeView</param>
        /// <returns></returns>
        private static List<TreeViewItem> GetSelectedTreeViewItems(TreeView tree)
        {
            return GetExpandedTreeViewItems(tree).Where(i => GetIsSelected(i)).ToList();
        }

        /// <summary>
        /// Select by left mouse button.
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="item"></param>
        private static void MakeSingleSelection(TreeView tree, TreeViewItem item)
        {
            foreach (TreeViewItem selectedItem in GetExpandedTreeViewItems(tree))
            {
                if (selectedItem == null)
                {
                    continue;
                }
                if (selectedItem != item)
                {
                    SetIsSelected(selectedItem, false);
                }
                else
                {
                    SetIsSelected(selectedItem, true);
                }
            }
            UpdateAnchorAndActionItem(tree, item);
        }

        /// <summary>
        /// Select by Ctrl key.
        /// </summary>
        /// <param name="tree">TreeView</param>
        /// <param name="item">TreeViewItem</param>
        private static void MakeToggleSelection(TreeView tree, TreeViewItem item)
        {
            SetIsSelected(item, !GetIsSelected(item));
            UpdateAnchorAndActionItem(tree, item);
        }

        /// <summary>
        /// Update the Anchor TreeViewItem.
        /// </summary>
        /// <param name="tree">TreeView</param>
        /// <param name="item">TreeViewItem</param>
        private static void UpdateAnchorAndActionItem(TreeView tree, TreeViewItem item)
        {
            SetAnchorItem(tree, item);
        }
    }
}