using System.Collections.Generic;
using Tecnomatix.Engineering;

namespace AutoJTTXCoreUtilities
{
    public struct MyTxWeldLocationOperation//撤销
    {
        public ITxOperation MytxWeldLocationOperation;
        public string WeldName;
    }
    public class UndoOperation//撤销类
    {
        private List<List<MyTxWeldLocationOperation>> _undoTxWeldLocationOperations;
        private int iCount;
        public List<List<MyTxWeldLocationOperation>> UndoTxWeldLocationOperations { get => _undoTxWeldLocationOperations; set => _undoTxWeldLocationOperations = value; }
        public int ICount
        {
            get => this.UndoTxWeldLocationOperations.Count;
        }


        public UndoOperation()
        {
            this.UndoTxWeldLocationOperations = new List<List<MyTxWeldLocationOperation>>();
        }
        public void AddData(List<MyTxWeldLocationOperation> myTxWeldLocationOperations)
        {
            this.UndoTxWeldLocationOperations.Add(myTxWeldLocationOperations);
        }


    }
}
