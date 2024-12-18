using System;
using System.Windows.Forms;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Ui.WPF;

namespace AutoJTTXCoreUtilities
{
    internal class AJTRobotJogCollisionLogic
    {
        #region private field

        //2 pi
        private const double TwicePI = 6.2831853071795862;
        //collision Manager
        private AJTCollisionQueryMgr _collisionManager;
        //移动对象集合
        private TxObjectList _listMovingObjects;
        //源对象
        private ITxLocatableObject _sourceObject;
        //停止运行碰撞
        private bool _stopRunCollision;

        #endregion

        #region Public Constructors

        public AJTRobotJogCollisionLogic(ITxLocatableObject sourceObject)
        {
            //初始化源对象
            this._sourceObject = sourceObject;
        }

        #endregion

        #region private method

        //添加移动对象的All Descendants
        private void AddAllDescendants(TxObjectList list)
        {
            ITxTypeFilter txTypeFilter = new TxTypeFilter(typeof(ITxObject));
            foreach (ITxObject txObject in list)
            {
                if (!this._listMovingObjects.Contains(txObject))
                {
                    this._listMovingObjects.Add(txObject);
                    if (txObject is ITxLocatableObject)
                    {
                        this.AddAllDescendants((txObject as ITxLocatableObject).GetDirectAttachmentDescendants(txTypeFilter));
                    }
                    if (txObject is ITxObjectCollection)
                    {
                        this.AddAllDescendants((txObject as ITxObjectCollection).GetDirectDescendants(txTypeFilter));
                    }
                }
            }
        }

        //查询碰撞结果
        private bool HasCollidingObjects(TxCollisionQueryResults listCollision)
        {
            foreach (object obj in listCollision.States)
            {
                TxCollisionState txCollisionState = (TxCollisionState)obj;
                bool flag = this._listMovingObjects.Contains(txCollisionState.FirstObject);
                bool flag2 = this._listMovingObjects.Contains(txCollisionState.SecondObject);
                if (flag ^ flag2)
                {
                    return true;
                }
            }
            return false;
        }

        //移动
        private bool MoveOneStep(TxPlacementCollisionControlRunCollisionEventArgs args)
        {
            args.RunCallBack(this);
            return args.HasMove;
        }

        private bool IsFullRotation(double start, TxPlacementCollisionControlRunCollisionEventArgs args)
        {
            return args.MoveOrRotateRunCollision == TxPlacementCollisionControlRunCollisionEventArgs.MoveRotateRunCollision.RotateCollision && Math.Abs(args.Value - start) * TxApplication.Options.Units.AngularMultiplier > 6.2831853071795862;
        }

        private bool HasCollision(bool RestartNow, bool lockTCPF, bool enableRobotPlacement)
        {
            TxCollisionQueryResults txCollisionQueryResults;
            if (this._collisionManager == null || RestartNow)
            {
                this._listMovingObjects = new TxObjectList();
                if (this._sourceObject != null)
                {
                    if (this._sourceObject is TxRobot)
                    {
                        TxRobot txRobot = this._sourceObject as TxRobot;
                        if (!lockTCPF)
                        {
                            ITxTypeFilter txTypeFilter = new TxTypeFilter(typeof(ITxObject));
                            TxObjectList directAttachmentDescendants = txRobot.Toolframe.GetDirectAttachmentDescendants(txTypeFilter);
                            directAttachmentDescendants.Add(txRobot);
                            this.AddAllDescendants(directAttachmentDescendants);
                        }
                        else if (enableRobotPlacement)
                        {
                            this._listMovingObjects.Add(this._sourceObject);
                        }
                    }
                    else
                    {
                        TxObjectList txObjectList = new TxObjectList();
                        txObjectList.Add(this._sourceObject);
                        this.AddAllDescendants(txObjectList);
                    }
                }
                this._collisionManager = new AJTCollisionQueryMgr();
                this._collisionManager.SelectedFilter = eFilterType.CollisionsOnly;
                txCollisionQueryResults = this._collisionManager.GetAllCollisionsQueryResults();
            }
            else
            {
                txCollisionQueryResults = this._collisionManager.GetQueryResults();
            }
            return txCollisionQueryResults.States.Count != 0 && (!RestartNow || this.HasCollidingObjects(txCollisionQueryResults));
        }

        #endregion

        #region public method

        public bool RunToCollision(TxPlacementCollisionControlRunCollisionEventArgs args, bool lockTCPF, bool enableRobotPlacement)
        {
            Application.DoEvents();
            this._stopRunCollision = false;
            bool restartNow = true;
            bool flag = true;
            double value = args.Value;
            while (!this.HasCollision(restartNow, lockTCPF, enableRobotPlacement))
            {
                flag = false;
                if (!this.MoveOneStep(args) || this.IsFullRotation(value, args))
                {
                    return false;
                }
                Application.DoEvents();
                restartNow = false;
                if (this._stopRunCollision)
                {
                    return false;
                }
            }
            if (!flag)
            {
                if (args.LeftOrRightRunCollision == TxPlacementCollisionControlRunCollisionEventArgs.LeftRightRunCollision.LeftCollision)
                {
                    args.LeftOrRightRunCollision = TxPlacementCollisionControlRunCollisionEventArgs.LeftRightRunCollision.RightCollision;
                }
                else if (args.LeftOrRightRunCollision == TxPlacementCollisionControlRunCollisionEventArgs.LeftRightRunCollision.RightCollision)
                {
                    args.LeftOrRightRunCollision = TxPlacementCollisionControlRunCollisionEventArgs.LeftRightRunCollision.LeftCollision;
                }
                this.MoveOneStep(args);
            }
            return true;
        }

        public void StopRunToCollision()
        {
            this._stopRunCollision = true;
        }

        #endregion
    }
}