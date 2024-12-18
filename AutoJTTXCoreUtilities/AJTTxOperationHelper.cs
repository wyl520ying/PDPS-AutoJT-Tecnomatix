using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Ui;

namespace AutoJTTXCoreUtilities
{
    public class AJTTxOperationHelper
    {
        #region 自定义筛选器

        //all
        public TxTypeFilter pickTypeFilter_all = new TxTypeFilter(new Type[] { typeof(ITxWeldOperation), typeof(TxCompoundOperation), typeof(TxWeldLocationOperation), typeof(TxRoboticViaLocationOperation) });
        //weld via
        public TxTypeFilter pickTypeFilter_weld_via = new TxTypeFilter(new Type[] { typeof(TxWeldLocationOperation), typeof(TxRoboticViaLocationOperation) });

        //op
        public TxTypeFilter filter_op = new TxTypeFilter(new Type[] { typeof(ITxWeldOperation) });
        //cop
        public TxTypeFilter filter_cop = new TxTypeFilter(new Type[] { typeof(TxCompoundOperation) });

        //weld
        public TxTypeFilter pickTypeFilter_WELD = new TxTypeFilter(new Type[] { typeof(TxWeldLocationOperation) });
        //via
        public TxTypeFilter pickTypeFilter_VIA = new TxTypeFilter(new Type[] { typeof(TxRoboticViaLocationOperation) });

        #endregion

        #region 焊点和过渡点选项

        //TxObjGridCtrl
        public TxObjGridCtrl txObjGridCtrl1 = null;

        //过渡点选项
        public bool viaPoint = false;
        //焊点选项
        public bool weldPoint = false;

        //是否只添加location
        public bool IsOnlyAppenLocation = false;

        #endregion

        #region  定义可能的pick-provider类型。

        public static List<TxPickProvider> GetOperationPickProviders()
        {
            List<TxPickProvider> operationPickProviders = null;

            try
            {
                operationPickProviders = new List<TxPickProvider>();
                operationPickProviders.Add(TxPickProvider.ObjectTree);
                operationPickProviders.Add(TxPickProvider.GraphicViewer);
                operationPickProviders.Add(TxPickProvider.TaskInventoryViewer);

                if (TecnomatixInfos.IsProcessDesigner())
                {
                    operationPickProviders.Add(TxPickProvider.NavigationTree);
                }
                else
                {
                    operationPickProviders.Add(TxPickProvider.TaskManagerViewer);
                    operationPickProviders.Add(TxPickProvider.PathEditorViewer);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return operationPickProviders;
        }

        //用法
        //operationPicker.SetPickProviders(this.operationPickProviders.ToArray());

        #endregion


        #region 处理用户选中的对象        

        public AJTTxOperationHelper(bool viaPoint_1, bool weldPoint_1)
        {
            this.viaPoint = viaPoint_1;
            this.weldPoint = weldPoint_1;
        }

        #region 用户选择的对象的处理

        /// <summary>
        /// 用户选中的ITXObject对象的处理
        /// </summary>
        /// <param name="txObjects_local"></param>
        public void SelectedObject(ref TxObjectList txObjects_local)
        {
            //用户当前选中的对象
            TxObjectList txObjects = TxApplication.ActiveSelection.GetFilteredItems(this.pickTypeFilter_all);

            foreach (ITxObject item in txObjects)
            {
                //TxWeldLocationOperation TxRoboticViaLocationOperation
                if (item is ITxRoboticLocationOperation)
                {
                    AssignLocation(ref txObjects_local, item);
                }
                //ITxWeldOperation
                else if (item is ITxWeldOperation)
                {
                    ITxWeldOperation txWeldOperation = (ITxWeldOperation)item;

                    CheckSelectedOPWeldPoint_1(txWeldOperation, ref txObjects_local);
                }
                //others
                else
                {
                    //品
                    if (item is TxCompoundOperation)
                    {
                        //用户选择的品(cop) 找到所有的op和cop
                        FindAllOP4COP_1(ref txObjects_local, (TxCompoundOperation)item);
                    }
                }
            }
        }

        #endregion

        #region 处理COP

        //用户选择的品(cop) 找到所有的op和cop
        void FindAllOP4COP_1(ref TxObjectList txObjects_local, TxCompoundOperation cop)
        {
            //op
            TxObjectList directDescendants_op = cop.GetDirectDescendants(this.filter_op);
            int iCount_op = directDescendants_op.Count;

            //品
            TxObjectList directDescendants_cop = cop.GetDirectDescendants(this.filter_cop);
            int iCount_cop = directDescendants_cop.Count;


            //选定的品中有op
            if (iCount_op != 0)
            {
                //遍所有op, 逐个加入到list
                foreach (ITxObject item in directDescendants_op)
                {
                    ITxWeldOperation txWeldOperation1 = item as ITxWeldOperation;
                    if (txWeldOperation1 != null)
                    {
                        //检查op中是否有焊点
                        CheckSelectedOPWeldPoint_1(txWeldOperation1, ref txObjects_local);
                    }
                }
            }
            //选定的品中有品
            if (iCount_cop != 0)
            {
                RecursionTraverse_cop_1(directDescendants_cop, ref txObjects_local);
            }
        }

        //递归遍历cop 
        void RecursionTraverse_cop_1(TxObjectList cops_root, ref TxObjectList txObjects_local)
        {
            //在多个root品中遍历
            foreach (ITxObject item in cops_root)
            {
                //转换成品
                TxCompoundOperation txCompoundOperation = item as TxCompoundOperation;

                //当前品中的op
                TxObjectList directDescendants_op = txCompoundOperation.GetDirectDescendants(this.filter_op);
                int iCount_op = directDescendants_op.Count;

                //当前品中的品
                TxObjectList directDescendants_cop = txCompoundOperation.GetDirectDescendants(this.filter_cop);
                int iCount_cop = directDescendants_cop.Count;

                //当前品中有op
                if (iCount_op != 0)
                {
                    //遍所有op, 逐个加入到list
                    foreach (ITxObject item_op in directDescendants_op)
                    {
                        ITxWeldOperation txWeldOperation1 = item_op as ITxWeldOperation;
                        if (txWeldOperation1 != null)
                        {
                            //检查op中是否有焊点
                            CheckSelectedOPWeldPoint_1(txWeldOperation1, ref txObjects_local);
                        }
                    }
                }
                //选定的品中有品
                if (iCount_cop != 0)
                {
                    RecursionTraverse_cop_1(directDescendants_cop, ref txObjects_local);
                }
            }
        }

        #endregion

        #region 处理OP

        /// <summary>
        /// 检查选定的op中是否存在焊点, 将焊点或via加入到list
        /// </summary>
        /// <param name="txWeldOperation"></param>
        /// <param name="txObjects_local"></param>
        void CheckSelectedOPWeldPoint_1(ITxWeldOperation txWeldOperation, ref TxObjectList txObjects_local)
        {
            //op中的list
            TxObjectList roboticLocation1 = txWeldOperation.GetDirectDescendants(this.pickTypeFilter_weld_via);

            //在op中遍历
            foreach (ITxObject item2 in roboticLocation1)
            {
                //赋值op
                AssignLocation(ref txObjects_local, item2);
            }
        }

        /// <summary>
        /// 赋值op
        /// </summary>
        /// <param name="txObjects_local"></param>
        /// <param name="obj"></param>
        void AssignLocation(ref TxObjectList txObjects_local, ITxObject obj)
        {
            //weld
            if (obj is TxWeldLocationOperation && this.weldPoint)
            {
                txObjects_local.Add(obj);
            }
            //via
            else if (obj is TxRoboticViaLocationOperation && this.viaPoint)
            {
                txObjects_local.Add(obj);
            }
        }

        #endregion

        #endregion

        #region 处理 TxObjGridCtrl         

        public AJTTxOperationHelper(TxObjGridCtrl txObjGridCtrl_1, bool viaPoint_1, bool weldPoint_1, bool isOnlyAppenLocation = false)
        {
            this.txObjGridCtrl1 = txObjGridCtrl_1;

            this.viaPoint = viaPoint_1;
            this.weldPoint = weldPoint_1;

            this.IsOnlyAppenLocation = isOnlyAppenLocation;
        }


        //检查选中的焊接操作是否有焊点操作
        public void TxObjGridCtrl_ObjectInserted(Tecnomatix.Engineering.Ui.TxObjGridCtrl_ObjectInsertedEventArgs args)
        {
            //判断选定对象的类型
            ITxWeldOperation txWeldOperation = args.Obj as ITxWeldOperation;
            TxCompoundOperation txCompoundOperation = args.Obj as TxCompoundOperation;

            //用户选定的对象, 不是op也不是cop
            if (txWeldOperation == null && txCompoundOperation == null)
            {
                return;
            }

            //检查选定的op中是否存在焊点
            if (txWeldOperation != null)
            {
                CheckSelectedOPWeldPoint1(args, txWeldOperation, true);
            }
            //将品的op加入到list
            else if (txCompoundOperation != null)
            {
                //从list删除cop
                this.txObjGridCtrl1.Objects.Remove(args.Obj);
                this.txObjGridCtrl1.DeleteRow(args.Row);
                this.txObjGridCtrl1.CurrentRow = args.Row;

                FindAllOP1(args, txCompoundOperation);
            }
        }


        //检查选定的op中是否存在焊点
        void CheckSelectedOPWeldPoint1(TxObjGridCtrl_ObjectInsertedEventArgs args, ITxWeldOperation txWeldOperation, bool info = false)
        {
            TxObjectList directDescendants = null;
            if (this.viaPoint && this.weldPoint)
            {
                directDescendants = txWeldOperation.GetDirectDescendants(this.pickTypeFilter_weld_via);
            }
            else if (this.viaPoint && !this.weldPoint)
            {
                directDescendants = txWeldOperation.GetDirectDescendants(this.pickTypeFilter_VIA);
            }
            else if (!this.viaPoint && this.weldPoint)
            {
                directDescendants = txWeldOperation.GetDirectDescendants(this.pickTypeFilter_WELD);
            }

            int? iCount = directDescendants?.Count;

            if (iCount == 0)
            {
                this.txObjGridCtrl1.Objects.Remove(args.Obj);
                this.txObjGridCtrl1.DeleteRow(args.Row);
                this.txObjGridCtrl1.CurrentRow = args.Row;
                if (info)
                {
                    Tecnomatix.Engineering.TxMessageBox.ShowModal("选定的Operation中没有焊接操作！", "AutoJT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (this.IsOnlyAppenLocation)
                {
                    //从list删除op
                    this.txObjGridCtrl1.Objects.Remove(args.Obj);
                    this.txObjGridCtrl1.DeleteRow(args.Row);
                    this.txObjGridCtrl1.CurrentRow = args.Row;

                    this.OnlyAppenLocation(txWeldOperation);
                }
            }
        }

        //用户选择的品(cop) 找到所有的op
        void FindAllOP1(TxObjGridCtrl_ObjectInsertedEventArgs args, TxCompoundOperation txCompoundOperation)
        {
            //op
            TxObjectList directDescendants_op = txCompoundOperation.GetDirectDescendants(this.filter_op);
            int iCount_op = directDescendants_op.Count;

            //品
            TxObjectList directDescendants_cop = txCompoundOperation.GetDirectDescendants(this.filter_cop);
            int iCount_cop = directDescendants_cop.Count;

            //检查选定的品中没有有op也没有品
            if (iCount_op == 0 && iCount_cop == 0)
            {
                this.txObjGridCtrl1.Objects.Remove(args.Obj);
                this.txObjGridCtrl1.DeleteRow(args.Row);
                this.txObjGridCtrl1.CurrentRow = args.Row;
                Tecnomatix.Engineering.TxMessageBox.ShowModal("选定的'品'中没有WeldOperation也没有'品'！", "AutoJT", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //选定的品中有op
                if (iCount_op != 0)
                {
                    //遍所有op, 逐个加入到list
                    foreach (ITxObject item in directDescendants_op)
                    {
                        ITxWeldOperation txWeldOperation1 = item as ITxWeldOperation;
                        if (txWeldOperation1 != null)
                        {
                            //检查op中是否有焊点
                            CheckSelectedOPWeldPoint_1(txWeldOperation1);
                        }
                    }
                }
                //选定的品中有品
                if (iCount_cop != 0)
                {
                    RecursionTraverse_cop_1(directDescendants_cop);
                }
            }
        }

        //检查选定的op中是否存在焊点
        void CheckSelectedOPWeldPoint_1(ITxWeldOperation txWeldOperation, bool info = false)
        {
            TxObjectList directDescendants = null;
            if (this.viaPoint && this.weldPoint)
            {
                directDescendants = txWeldOperation.GetDirectDescendants(this.pickTypeFilter_weld_via);
            }
            else if (this.viaPoint && !this.weldPoint)
            {
                directDescendants = txWeldOperation.GetDirectDescendants(this.pickTypeFilter_VIA);
            }
            else if (!this.viaPoint && this.weldPoint)
            {
                directDescendants = txWeldOperation.GetDirectDescendants(this.pickTypeFilter_WELD);
            }

            int iCount = directDescendants.Count;

            if (iCount != 0)
            {
                if (this.IsOnlyAppenLocation)
                {
                    this.OnlyAppenLocation(txWeldOperation);
                }
                else
                {
                    this.txObjGridCtrl1.AppendObject(txWeldOperation);
                }
                if (info)
                {
                    Tecnomatix.Engineering.TxMessageBox.ShowModal("选定的Operation中没有焊接操作！", "AutoJT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //递归遍历cop 
        void RecursionTraverse_cop_1(TxObjectList cops_root)
        {
            //在多个root品中遍历
            foreach (ITxObject item in cops_root)
            {
                //转换成品
                TxCompoundOperation txCompoundOperation = item as TxCompoundOperation;

                //从list删除cop
                this.txObjGridCtrl1.Objects.Remove(txCompoundOperation);
                //this.txObjGridCtrl_Selected.DeleteRow(args.Row);
                //this.txObjGridCtrl_Selected.CurrentRow = args.Row;

                //当前品中的op
                TxObjectList directDescendants_op = txCompoundOperation.GetDirectDescendants(this.filter_op);
                int iCount_op = directDescendants_op.Count;

                //当前品中的品
                TxObjectList directDescendants_cop = txCompoundOperation.GetDirectDescendants(this.filter_cop);
                int iCount_cop = directDescendants_cop.Count;

                //当前品中有op
                if (iCount_op != 0)
                {
                    //遍所有op, 逐个加入到list
                    foreach (ITxObject item_op in directDescendants_op)
                    {
                        ITxWeldOperation txWeldOperation1 = item_op as ITxWeldOperation;
                        if (txWeldOperation1 != null)
                        {
                            //检查op中是否有焊点
                            CheckSelectedOPWeldPoint_1(txWeldOperation1);
                        }
                    }
                }
                //选定的品中有品
                if (iCount_cop != 0)
                {
                    RecursionTraverse_cop_1(directDescendants_cop);
                }
            }
        }

        //只添加Location
        void OnlyAppenLocation(ITxObject txObject)
        {

            if (this.viaPoint && txObject is TxRoboticViaLocationOperation)
            {
                this.txObjGridCtrl1.AppendObject(txObject);

            }
            else if (this.weldPoint && txObject is TxWeldLocationOperation)
            {
                this.txObjGridCtrl1.AppendObject(txObject);
            }
            else if (txObject is ITxWeldOperation)
            {
                ITxWeldOperation txWeldOperation = (ITxWeldOperation)txObject;

                TxObjectList directDescendants = null;
                if (this.viaPoint && this.weldPoint)
                {
                    directDescendants = txWeldOperation.GetDirectDescendants(this.pickTypeFilter_weld_via);
                }
                else if (this.viaPoint && !this.weldPoint)
                {
                    directDescendants = txWeldOperation.GetDirectDescendants(this.pickTypeFilter_VIA);
                }
                else if (!this.viaPoint && this.weldPoint)
                {
                    directDescendants = txWeldOperation.GetDirectDescendants(this.pickTypeFilter_WELD);
                }

                foreach (ITxObject item in directDescendants)
                {
                    this.txObjGridCtrl1.AppendObject(item);
                }
            }
        }
        #endregion

    }
}
