using Tecnomatix.Engineering;

namespace AutoJTTXCoreUtilities
{
    public class AJTLMLocationsApp
    {
        public TxObjectList GetLocationsFromSelection()
        {
            TxObjectList txObjectList = new TxObjectList();
            TxSelection activeSelection = TxApplication.ActiveSelection;
            TxObjectList orderedItems = activeSelection.GetOrderedItems();
            foreach (ITxObject txObject in orderedItems)
            {
                ITxOperation txOperation = txObject as ITxOperation;
                if (txOperation != null)
                {
                    TxObjectList locationsFromOperation = this.GetLocationsFromOperation(txOperation);
                    txObjectList.Append(locationsFromOperation);
                }
            }
            return txObjectList;
        }

        public TxObjectList GetLocationsFromOperation(ITxOperation op)
        {
            TxObjectList txObjectList = new TxObjectList();
            ITxOrderedObjectCollection txOrderedObjectCollection = op as ITxOrderedObjectCollection;
            if (txOrderedObjectCollection != null)
            {
                int count = txOrderedObjectCollection.Count;
                for (int i = 0; i < count; i++)
                {
                    ITxLocationOperation txLocationOperation = txOrderedObjectCollection.GetChildAt(i) as ITxLocationOperation;
                    if (txLocationOperation != null)
                    {
                        txObjectList.Add(txLocationOperation);
                    }
                    else
                    {
                        ITxOrderedObjectCollection txOrderedObjectCollection2 = txOrderedObjectCollection.GetChildAt(i) as ITxOrderedObjectCollection;
                        if (txOrderedObjectCollection2 != null)
                        {
                            txObjectList.Append(this.GetLocationsFromOperation(txOrderedObjectCollection2 as ITxOperation));
                        }
                    }
                }
            }
            else
            {
                ITxLocationOperation txLocationOperation2 = op as ITxLocationOperation;
                if (txLocationOperation2 != null)
                {
                    txObjectList.Add(txLocationOperation2);
                }
            }
            return txObjectList;
        }
    }

}
