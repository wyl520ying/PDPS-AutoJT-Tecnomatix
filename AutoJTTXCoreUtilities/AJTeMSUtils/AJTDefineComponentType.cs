using System;
using System.Collections.Generic;
using System.Linq;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.DataTypes;
using Tecnomatix.Engineering.Utilities;

namespace AutoJTTXCoreUtilities.AJTeMSUtils
{
    public class AJTDefineComponentType
    {
        #region Properties

        /// <summary>
        /// TuneData.xml 文件的处理程序，包含组件 PM 数据。
        /// </summary>
        private TxTunePmDocumentManager _tunePmDocHanlder = new TxTunePmDocumentManager();


        private string _rootFolderPath;
        private SortedDictionary<string, TxPlanningTypeMetaData> _prototypesDictionary;

        public AJTDefineComponentType(string rootFolderPath)
        {
            this._rootFolderPath = rootFolderPath;
        }

        #endregion

        #region Public Method

        public SortedDictionary<string, TxPlanningTypeMetaData> PrototypesMetaData
        {
            get
            {
                if (this._prototypesDictionary == null)
                {
                    this._prototypesDictionary = this.GetPrototypesMetaData();
                }
                return this._prototypesDictionary;
            }
        }

        #endregion


        #region GetPrototypesMetaData

        private SortedDictionary<string, TxPlanningTypeMetaData> GetPrototypesMetaData()
        {
            SortedDictionary<string, TxPlanningTypeMetaData> sortedDictionary = null;
            TxPlanningTypeMetaData txPlanningTypeMetaData = null;
            ITxPlatformGlobalServicesProvider platformGlobalServicesProvider = TxApplication.ActiveDocument.PlatformGlobalServicesProvider;
            TxPlanningTypeMetaData typeMetaData = platformGlobalServicesProvider.GetTypeMetaData("PmPartPrototype");
            txPlanningTypeMetaData = platformGlobalServicesProvider.GetTypeMetaData("PmToolPrototype");
            sortedDictionary = new SortedDictionary<string, TxPlanningTypeMetaData>();
            if (typeMetaData != txPlanningTypeMetaData)
            {
                foreach (object obj in typeMetaData.DerivedTypes)
                {
                    TxPlanningTypeMetaData md = (TxPlanningTypeMetaData)obj;
                    this.AddToArrayAndHandleDuplicate(sortedDictionary, md);
                }
            }
            if (txPlanningTypeMetaData != null)
            {
                foreach (object obj2 in txPlanningTypeMetaData.DerivedTypes)
                {
                    TxPlanningTypeMetaData md2 = (TxPlanningTypeMetaData)obj2;
                    this.AddToArrayAndHandleDuplicate(sortedDictionary, md2);
                }
            }
            return sortedDictionary;
        }

        private void AddToArrayAndHandleDuplicate(SortedDictionary<string, TxPlanningTypeMetaData> retVal, TxPlanningTypeMetaData md)
        {
            if (retVal.Keys.Contains(md.DisplayName))
            {
                TxPlanningTypeMetaData txPlanningTypeMetaData = retVal[md.DisplayName];
                retVal.Remove(md.DisplayName);
                retVal.Add(this.GetUniqueDisplayname(txPlanningTypeMetaData), txPlanningTypeMetaData);
                retVal.Add(this.GetUniqueDisplayname(md), md);
                return;
            }
            retVal.Add(md.DisplayName, md);
        }

        private string GetUniqueDisplayname(TxPlanningTypeMetaData md)
        {
            return md.DisplayName + "(" + md.TypeDefinition.Name + ")";
        }

        #endregion

        public bool UpdateComponentTuneDataXml(AJTFileComponentNode componentNode, string _ExternalID)
        {
            bool flag = false;

            if (componentNode.TypeMetaData != null && componentNode.IsDirty)
            {
                try
                {
                    string text = _ExternalID;
                    if (string.IsNullOrEmpty(text))
                    {
                        text = Guid.NewGuid().ToString();
                    }

                    string typeName = componentNode.TypeMetaData.TypeName;
                    TxTunePmDocumentPrototypeData txTunePmDocumentPrototypeData = this._tunePmDocHanlder.CreatePrototypeData(text, typeName, 0.0);
                    this._tunePmDocHanlder.SavePrototypeDataToDocument(componentNode.FullPath, txTunePmDocumentPrototypeData);
                    flag = true;
                }
                catch
                {
                    flag = false;
                }
            }

            return flag;
        }
    }
}
