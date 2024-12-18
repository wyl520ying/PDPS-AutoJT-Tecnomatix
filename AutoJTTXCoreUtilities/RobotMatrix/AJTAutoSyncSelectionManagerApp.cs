using System;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Ui;

namespace AutoJTTXCoreUtilities.RobotMatrix
{
    public class AJTAutoSyncSelectionManagerApp
    {
        public AJTAutoSyncSelectionManagerApp(TxObjGridCtrl weldList)
        {
            this.m_weldList = weldList;
        }

        public void Initialize()
        {
            TxSelection activeTemporarySelection = TxApplication.ActiveTemporarySelection;
            activeTemporarySelection.ItemsAdded += new TxSelection_ItemsAddedEventHandler(this.OnSelectionAdded);
            activeTemporarySelection.ItemsRemoved += new TxSelection_ItemsRemovedEventHandler(this.OnSelectionRemoved);
            activeTemporarySelection.ItemsSet += new TxSelection_ItemsSetEventHandler(this.OnSelectionSet);
            activeTemporarySelection.Cleared += new TxSelection_ClearedEventHandler(this.OnSelectionCleared);
        }

        public void UnInitialize()
        {
            TxSelection activeTemporarySelection = TxApplication.ActiveTemporarySelection;
            activeTemporarySelection.ItemsAdded -= new TxSelection_ItemsAddedEventHandler(this.OnSelectionAdded);
            activeTemporarySelection.ItemsRemoved -= new TxSelection_ItemsRemovedEventHandler(this.OnSelectionRemoved);
            activeTemporarySelection.ItemsSet -= new TxSelection_ItemsSetEventHandler(this.OnSelectionSet);
            activeTemporarySelection.Cleared -= new TxSelection_ClearedEventHandler(this.OnSelectionCleared);
        }

        private void OnSelectionAdded(object sender, TxSelection_ItemsAddedEventArgs args)
        {
            this.ChangeSelectedItems();
        }

        private void OnSelectionRemoved(object sender, TxSelection_ItemsRemovedEventArgs args)
        {
            this.ChangeSelectedItems();
        }

        private void OnSelectionSet(object sender, TxSelection_ItemsSetEventArgs args)
        {
            this.ChangeSelectedItems();
        }

        private void OnSelectionCleared(object sender, TxSelection_ClearedEventArgs args)
        {
            this.ChangeSelectedItems();
        }

        private void ChangeSelectedItems()
        {
            Type[] array = new Type[]
            {
                typeof(TxWeldLocationOperation),
                typeof(ITxWeldOperation),
                typeof(TxWeldPoint)
            };
            TxObjectList filteredItems = TxApplication.ActiveTemporarySelection.GetFilteredItems(new TxTypeFilter(array));
            if (filteredItems.Count > 0 && this.m_weldList != null)
            {
                TxApplication.ActiveSelection.SetItems(filteredItems);
            }
        }

        public void SynchronizeApplicationSelection(TxObjectList selection)
        {
            TxApplication.ActiveSelection.SetItems(selection);
        }

        private TxObjGridCtrl m_weldList;
    }

}
