using System.Collections.Generic;
using Tecnomatix.Engineering;

namespace AutoJTTXCoreUtilities
{
    public class AJTTxObjectListUtilities
    {
        public static bool IsNullOrEmpty(TxObjectList objList)
        {
            return objList == null || objList.Count <= 0;
        }

        public static bool Contains(TxObjectList objList, ITxObject obj)
        {
            return AJTTxObjectListUtilities.GetObjectIndex(objList, obj) >= 0;
        }

        public static int GetObjectIndex(TxObjectList objList, ITxObject obj)
        {
            if (!AJTTxObjectListUtilities.IsNullOrEmpty(objList) & obj != null)
            {
                for (int i = 0; i < objList.Count; i++)
                {
                    if (objList[i].Equals(obj))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public static ITxObject GetObjectAt(TxObjectList objList, int index)
        {
            ITxObject result;
            if (!AJTTxObjectListUtilities.IsNullOrEmpty(objList) && index >= 0 && index < objList.Count)
            {
                result = objList[index];
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static TxObjectList Copy(TxObjectList objList)
        {
            TxObjectList result;
            if (!AJTTxObjectListUtilities.IsNullOrEmpty(objList))
            {
                result = new TxObjectList(objList);
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static ITxObject First(TxObjectList objList)
        {
            ITxObject result;
            if (!AJTTxObjectListUtilities.IsNullOrEmpty(objList))
            {
                result = objList[0];
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static ITxObject Last(TxObjectList objList)
        {
            ITxObject result;
            if (!AJTTxObjectListUtilities.IsNullOrEmpty(objList))
            {
                result = objList[objList.Count - 1];
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static TxObjectList ToTxObjectList(List<ITxObject> objList)
        {
            TxObjectList txObjectList = new TxObjectList();
            TxObjectList result;
            if (objList != null)
            {
                foreach (ITxObject item in objList)
                {
                    txObjectList.Add(item);
                }
                result = txObjectList;
            }
            else
            {
                result = txObjectList;
            }
            return result;
        }

        public static TxObjectList Replace(TxObjectList objList, ITxObject toReplace, ITxObject replaceWith)
        {
            TxObjectList result;
            if (!AJTTxObjectListUtilities.IsNullOrEmpty(objList) && AJTTxObjectListUtilities.Contains(objList, toReplace) && !AJTTxObjectListUtilities.Contains(objList, replaceWith))
            {
                int objectIndex = AJTTxObjectListUtilities.GetObjectIndex(objList, toReplace);
                if (objectIndex >= 0)
                {
                    objList[objectIndex] = replaceWith;
                }
                result = objList;
            }
            else
            {
                result = objList;
            }
            return result;
        }

        public static TxObjectList Remove(TxObjectList objList, TxObjectList objectsToRemove)
        {
            TxObjectList result;
            if (AJTTxObjectListUtilities.IsNullOrEmpty(objList))
            {
                result = objList;
            }
            else
            {
                foreach (ITxObject objectToRemove in objectsToRemove)
                {
                    objList = AJTTxObjectListUtilities.Remove(objList, objectToRemove);
                }
                result = objList;
            }
            return result;
        }

        public static TxObjectList Remove(TxObjectList objList, ITxObject objectToRemove)
        {
            TxObjectList result;
            if (!AJTTxObjectListUtilities.IsNullOrEmpty(objList) && AJTTxObjectListUtilities.Contains(objList, objectToRemove))
            {
                objList.Remove(objectToRemove);
                result = objList;
            }
            else
            {
                result = objList;
            }
            return result;
        }
    }
}
