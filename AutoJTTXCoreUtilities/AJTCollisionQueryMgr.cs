
using EngineeringInternalExtension;
using EngineeringInternalExtension.Options;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Tecnomatix.Engineering;

namespace AutoJTTXCoreUtilities
{
    internal class AJTCollisionQueryMgr
    {
        #region 创建 Collision Pair

        public bool CreateCollisionSet(ITxRoboticLocationOperation txRoboticLocationOperation_current, ITxWeldOperation txRoboticOperation_current)
        {
            bool result = false;

            if (txRoboticLocationOperation_current == null || txRoboticOperation_current == null)
            {
                return false;
            }

            //Collision Set Type 检查类型
            TxRoboticOperationCollisionSetTypeEx roboticOperationCollisionSetType = TxOptionsEx.RoboticOperationCollisionSetType;
            if (roboticOperationCollisionSetType == TxRoboticOperationCollisionSetTypeEx.ActiveSet)
            {
                MessageBox.Show("The “Auto Create Collision Set” can be used only when “Default Set” option is selected in the Advanced Collision Options dialog.", "AutoJT", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
            }

            //robot 
            TxRobot selectedRobot = null;
            //gun
            ITxTool selectedGun = null;
            try
            {
                //robot 
                selectedRobot = AJTTxRobotUtilities.GetRobot(txRoboticOperation_current);
                //gun
                selectedGun = AJTApGenUtils.GetAssignedGun(txRoboticLocationOperation_current);
            }
            catch
            {
                return false;
            }


            #region collision Name

            #region text_gun 名称

            //碰撞对的名称
            string text_gun = null;
            //robot
            if (selectedRobot != null)
            {
                //获取当前碰撞对
                TxRobotEx txRobotEx = new TxRobotEx(selectedRobot);
                if (selectedGun != null)
                {
                    //当前mounted tool 的机器人
                    TxRobot mountedToolRobot = TxEngineeringDataInternal.GetMountedToolRobot(selectedGun);

                    if (mountedToolRobot == selectedRobot)
                    {
                        text_gun = selectedGun.Name;
                    }
                    else
                    {
                        //gripper
                        ITxGripper actualSimulatedGripper = txRobotEx.ActualSimulatedGripper;
                        if (actualSimulatedGripper != null)
                        {
                            text_gun = actualSimulatedGripper.Name;
                        }
                    }
                }
                else
                {
                    ITxTool txTool = txRobotEx.ActualSimulatedGun as ITxTool;
                    if (txTool == null)
                    {
                        txTool = (txRobotEx.ActualSimulatedGripper as ITxTool);
                    }
                    if (txTool != null)
                    {
                        text_gun = txTool.Name;
                    }
                }
            }
            //gun
            else if (selectedGun != null)
            {
                text_gun = selectedGun.Name;
            }

            #endregion

            #region 名称前缀


            //名称前缀
            string text_1 = "AJT_1_";
            string text_2 = "AJT_2_";

            //机器人名称
            if (selectedRobot != null)
            {
                text_1 = text_1.Insert(text_1.Length, selectedRobot.Name);
                text_2 = text_2.Insert(text_2.Length, selectedRobot.Name);
            }

            //焊枪名称
            if (text_gun != null)
            {
                if (selectedRobot != null)
                {
                    text_1 = text_1.Insert(text_1.Length, "_");
                    text_2 = text_2.Insert(text_2.Length, "_");
                }

                text_1 = text_1.Insert(text_1.Length, text_gun);
                text_2 = text_2.Insert(text_2.Length, text_gun);
            }

            #endregion

            #endregion

            //检查是否存在
            TxCollisionRoot collisionRoot = TxApplication.ActiveDocument.CollisionRoot;
            foreach (ITxObject item in collisionRoot.PairList)
            {
                TxCollisionPair txCollisionPair = item as TxCollisionPair;
                if (txCollisionPair != null)
                {
                    if (txCollisionPair.Name == text_1)
                    {
                        txCollisionPair.Delete();
                    }
                    else if (txCollisionPair.Name == text_2)
                    {
                        txCollisionPair.Delete();
                    }
                }
            }

            //新建
            bool bl1 = this.CreateCollisionSet_1(selectedRobot, selectedGun, text_1);
            bool bl2 = this.CreateCollisionSet_2(selectedRobot, selectedGun, text_2);

            if (bl1 && bl2)
            {
                result = true;
            }

            return result;
        }

        //创建碰撞对1(机器人和焊枪)
        bool CreateCollisionSet_1(TxRobot selectedRobot, ITxTool selectedGun, string text1)
        {
            bool result = false;

            try
            {
                //碰撞 root
                TxCollisionRoot collisionRoot = TxApplication.ActiveDocument.CollisionRoot;

                //Creates a new collision pair.
                collisionRoot.CreateCollisionPair(new TxCollisionPairCreationData(text1)
                {
                    FirstList = new TxObjectList() { selectedRobot },
                    SecondList = new TxObjectList() { selectedGun }
                });

                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        //创建碰撞对2(机器人焊枪和机器人中心半径4m范围内所有的可见模型)
        bool CreateCollisionSet_2(TxRobot selectedRobot, ITxTool selectedGun, string text_2)
        {
            bool result = false;

            try
            {
                //干涉对集合
                List<TxCollisionPairData> list = null;

                //robot
                if (selectedRobot != null)
                {
                    //获取当前碰撞对
                    TxRobotEx txRobotEx = new TxRobotEx(selectedRobot);
                    list = txRobotEx.CollisionPair;
                }
                //gun
                else if (selectedGun != null)
                {
                    TxGunEx txGunEx = new TxGunEx(selectedGun);
                    //当前碰撞对
                    list = txGunEx.CollisionPair;
                }

                //碰撞 root
                TxCollisionRoot collisionRoot = TxApplication.ActiveDocument.CollisionRoot;

                //找到的碰撞对
                if (list != null)
                {
                    //在所有的碰撞对中遍历
                    for (int i = 0; i < list.Count; i++)
                    {
                        //Both lists of objects 
                        if (list[i].FirstList.Count > 0 && list[i].SecondList.Count > 0)
                        {
                            //更新secondList
                            TxObjectList secondList_new = this.CheckCollisionPairSecondList(selectedRobot, list[i].SecondList);

                            //Creates a new collision pair.
                            collisionRoot.CreateCollisionPair(new TxCollisionPairCreationData(text_2)
                            {
                                FirstList = list[i].FirstList,
                                SecondList = secondList_new
                            });
                        }
                    }
                }

                result = true;
                //TxApplication.RefreshDisplay();
            }
            catch
            {
                result = false;
            }

            return result;
        }

        //机器人焊枪和机器人中心半径4m范围内所有的可见模型
        TxObjectList CheckCollisionPairSecondList(ITxLocatableObject txLocatable_robot, TxObjectList SecondList)
        {
            TxObjectList result = new TxObjectList();

            if (txLocatable_robot == null)
            {
                return null;
            }

            foreach (ITxObject item in SecondList)
            {
                try
                {
                    //排除隐藏的模型
                    ITxDisplayableObject txDisplayable = item as ITxDisplayableObject;
                    //不可见
                    if (txDisplayable != null && txDisplayable.Visibility == TxDisplayableObjectVisibility.None)
                    {
                        continue;
                    }

                    if (item is ITxLocatableObject)
                    {
                        ITxLocatableObject txLocatable1 = (ITxLocatableObject)item;
                        TxTransformation txTransformation_1 = txLocatable1.GetLocationRelativeToObject(txLocatable_robot);
                        TxVector txVector1 = txTransformation_1.Translation;
                        //排除半径4m内的零件
                        if (txVector1.X <= 4000 && txVector1.Y <= 4000 && txVector1.Z <= 4000)
                        {
                            result.Add(item);
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }

            return result;
        }

        #endregion


        public TxCollisionQueryResults GetQueryResults()
        {
            TxCollisionQueryResults result = null;
            eFilterType selectedFilter = this.SelectedFilter;
            if (selectedFilter != eFilterType.CollisionsOnly)
            {
                if (selectedFilter == eFilterType.DistanceAnalysis)
                {
                    result = this.GetCollisionAndDistancesResults(false);
                }
            }
            else
            {
                result = this.GetCollisionAndNearMissResults(false);
            }
            return result;
        }

        public TxCollisionQueryResults GetAllCollisionsQueryResults()
        {
            TxCollisionQueryResults result = null;
            eFilterType selectedFilter = this.SelectedFilter;
            if (selectedFilter != eFilterType.CollisionsOnly)
            {
                if (selectedFilter == eFilterType.DistanceAnalysis)
                {
                    result = this.GetCollisionAndDistancesResults(true);
                }
            }
            else
            {
                result = this.GetCollisionAndNearMissResults(true);
            }
            return result;
        }

        public void ResetCheckCollisions()
        {
            this.m_checkCollisions = TxApplication.ActiveDocument.CollisionRoot.CheckCollisions;
            if (this.m_checkCollisions)
            {
                this.SetQueryContext();
            }
        }

        public void ResetQueryContext()
        {
            if (this.IsLocalContextUsed)
            {
                this.ResetQueryLocalContext();
                return;
            }
            this.ResetQueryGlobalContext();
        }

        public void Uninitialize()
        {
            try
            {
                this.UninitDocumentContext();
                TxApplication.DocumentCollection.DocumentLoaded -= new TxDocumentCollection_DocumentLoadedEventHandler(this.OnLoadDocument);
            }
            catch (Exception)
            {
            }
        }

        public void InitDynamicCollisionQueryParams(TxCollisionQueryParams queryParams)
        {
            TxCollisionRoot collisionRoot = TxApplication.ActiveDocument.CollisionRoot;
            queryParams.Mode = collisionRoot.CollisionQueryMode;
            queryParams.ComputeDistanceIfInNearMiss = true;
            queryParams.NearMissDistance = collisionRoot.NearMissDefaultValue;
            queryParams.UseNearMiss = collisionRoot.CheckNearMiss;
            queryParams.AllowedPenetration = (double)TxApplication.Options.Collision.ContactTolerance;
            queryParams.UseAllowedPenetration = TxApplication.Options.Collision.CheckContact;
            queryParams.FindPenetrationRegions = TxApplication.Options.Collision.FindPenetrationRegions;
            queryParams.ReportLevel = TxApplication.Options.Collision.ReportLevel;
        }

        public eFilterType SelectedFilter
        {
            get
            {
                return this.m_selectedFilter;
            }
            set
            {
                this.m_selectedFilter = value;
            }
        }

        public TxCollisionQueryContext QueryContext
        {
            get
            {
                TxCollisionQueryContext result;
                if (this.IsLocalContextUsed)
                {
                    result = this.m_queryLocalContext;
                }
                else
                {
                    result = this.m_queryGlobalContext;
                }
                return result;
            }
        }

        internal AJTCollisionQueryMgr()
        {
            //初始化
            this.Init();
        }
        private void Init()
        {
            try
            {
                TxApplication.DocumentCollection.DocumentLoaded += new TxDocumentCollection_DocumentLoadedEventHandler(this.OnLoadDocument);
                this.m_selectedFilter = eFilterType.CollisionsOnly;
                this.InitDocumentContext();
            }
            catch (Exception)
            {
            }
        }
        private void InitDocumentContext()
        {
            try
            {
                TxDocument activeDocument = TxApplication.ActiveDocument;
                if (activeDocument != null)
                {
                    activeDocument.Unloading += new TxDocument_UnloadingEventHandler(this.OnUnloadDocument);
                }
                TxCollisionRoot collisionRoot = TxApplication.ActiveDocument.CollisionRoot;
                this.m_checkCollisions = collisionRoot.CheckCollisions;
                this.SetQueryContext();
            }
            catch (Exception)
            {
            }
        }

        private void SetQueryContext()
        {
            TxCollisionRoot collisionRoot = TxApplication.ActiveDocument.CollisionRoot;
            if (this.m_queryGlobalContext == null)
            {
                this.m_queryGlobalContext = collisionRoot.GlobalQueryContext;
            }
            if (this.m_queryLocalContext == null)
            {
                this.m_queryLocalContext = collisionRoot.CreateQueryContext();
            }
        }

        private void OnLoadDocument(object sender, TxDocumentCollection_DocumentLoadedEventArgs args)
        {
            this.InitDocumentContext();
        }

        private TxCollisionQueryResults GetGlobalCollidingObjectsChanges(TxCollisionQueryParams queryParams)
        {
            TxCollisionRoot collisionRoot = TxApplication.ActiveDocument.CollisionRoot;
            return collisionRoot.GetGlobalCollidingObjectsChanges(queryParams, this.QueryContext);
        }

        private TxCollisionQueryResults GetCollisionAndNearMissResults(bool all)
        {
            TxCollisionQueryResults result = null;
            if (this.QueryContext != null)
            {
                try
                {
                    TxCollisionRoot collisionRoot = TxApplication.ActiveDocument.CollisionRoot;
                    TxCollisionQueryParams txCollisionQueryParams = new TxCollisionQueryParams();
                    txCollisionQueryParams.Mode = collisionRoot.CollisionQueryMode;
                    txCollisionQueryParams.ComputeDistanceIfInNearMiss = true;
                    txCollisionQueryParams.NearMissDistance = collisionRoot.NearMissDefaultValue;
                    txCollisionQueryParams.UseNearMiss = collisionRoot.CheckNearMiss;
                    txCollisionQueryParams.AllowedPenetration = (double)TxApplication.Options.Collision.ContactTolerance;
                    txCollisionQueryParams.UseAllowedPenetration = TxApplication.Options.Collision.CheckContact;
                    txCollisionQueryParams.FindPenetrationRegions = TxApplication.Options.Collision.FindPenetrationRegions;
                    txCollisionQueryParams.ReportLevel = TxApplication.Options.Collision.ReportLevel;
                    if (all)
                    {
                        this.ResetQueryContext();
                    }
                    return this.GetGlobalCollidingObjectsChanges(txCollisionQueryParams);
                }
                catch (Exception)
                {
                }
                return result;
            }
            return result;
        }

        private TxCollisionQueryResults GetCollidingObjectsAndDistancesChanges(TxCollisionAndDistancesQueryParams queryParams)
        {
            TxCollisionRoot collisionRoot = TxApplication.ActiveDocument.CollisionRoot;
            return collisionRoot.GetCollidingObjectsAndDistancesChanges(queryParams, this.QueryContext);
        }

        private TxCollisionQueryResults GetCollisionAndDistancesResults(bool all)
        {
            TxCollisionQueryResults result;
            try
            {
                TxCollisionRoot collisionRoot = TxApplication.ActiveDocument.CollisionRoot;
                TxCollisionAndDistancesQueryParams txCollisionAndDistancesQueryParams = new TxCollisionAndDistancesQueryParams();
                txCollisionAndDistancesQueryParams.Mode = collisionRoot.CollisionQueryMode;
                txCollisionAndDistancesQueryParams.NearMissDistance = collisionRoot.NearMissDefaultValue;
                txCollisionAndDistancesQueryParams.AllowedPenetration = (double)TxApplication.Options.Collision.ContactTolerance;
                txCollisionAndDistancesQueryParams.UseAllowedPenetration = TxApplication.Options.Collision.CheckContact;
                txCollisionAndDistancesQueryParams.UseNearMiss = collisionRoot.CheckNearMiss;
                txCollisionAndDistancesQueryParams.FindPenetrationRegions = TxApplication.Options.Collision.FindPenetrationRegions;
                txCollisionAndDistancesQueryParams.ReportLevel = TxApplication.Options.Collision.ReportLevel;
                if (all)
                {
                    this.ResetQueryContext();
                }
                result = this.GetCollidingObjectsAndDistancesChanges(txCollisionAndDistancesQueryParams);
            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }

        private void ResetQueryLocalContext()
        {
            try
            {
                if (this.m_queryLocalContext != null)
                {
                    TxCollisionRoot collisionRoot = TxApplication.ActiveDocument.CollisionRoot;
                    collisionRoot.ResetQueryContext(this.m_queryLocalContext);
                }
            }
            catch (Exception)
            {
            }
        }

        private void ResetQueryGlobalContext()
        {
            try
            {
                if (this.m_queryGlobalContext != null)
                {
                    TxCollisionRoot collisionRoot = TxApplication.ActiveDocument.CollisionRoot;
                    collisionRoot.ResetGlobalQueryContext(this.m_queryGlobalContext);
                }
            }
            catch (Exception)
            {
            }
        }

        private void UninitDocumentContext()
        {
            try
            {
                TxCollisionRoot collisionRoot = TxApplication.ActiveDocument.CollisionRoot;
                if (this.m_queryGlobalContext != null)
                {
                    this.m_queryGlobalContext = null;
                }
                if (this.m_queryLocalContext != null)
                {
                    collisionRoot.DeleteQueryContext(this.m_queryLocalContext);
                    this.m_queryLocalContext = null;
                }
                TxDocument activeDocument = TxApplication.ActiveDocument;
                if (activeDocument != null)
                {
                    activeDocument.Unloading -= new TxDocument_UnloadingEventHandler(this.OnUnloadDocument);
                }
            }
            catch (Exception)
            {
            }
        }

        private void OnUnloadDocument(object sender, TxDocument_UnloadingEventArgs args)
        {
            this.UninitDocumentContext();
        }

        private bool IsLocalContextUsed
        {
            get
            {
                return eFilterType.DistanceAnalysis == this.m_selectedFilter;
            }
        }

        private TxCollisionQueryContext m_queryGlobalContext;

        private TxCollisionQueryContext m_queryLocalContext;

        private eFilterType m_selectedFilter;

        private bool m_checkCollisions;

        private TxCollisionQueryParams m_dynamicCollisionQueryParams = new TxCollisionQueryParams();
    }

    public enum eFilterType
    {
        CollisionsOnly,
        DistanceAnalysis
    }
}
