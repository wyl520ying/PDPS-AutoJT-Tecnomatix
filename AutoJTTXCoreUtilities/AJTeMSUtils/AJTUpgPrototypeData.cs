using AutoJTTXUtilities.DataHandling;
using EMPTYPELIBRARYLib;
using EngineeringInternalExtension;
using System.IO;

namespace AutoJTTXCoreUtilities
{
    internal class AJTUpgPrototypeData : IAJTUpgComponentOwnerData
    {
        public int Id
        {
            get
            {
                return this.m_id;
            }
            internal set
            {
                this.m_id = value;
            }
        }

        internal int ClassTypeId
        {
            get
            {
                return this.m_classTypeId;
            }
            set
            {
                this.m_classTypeId = value;
            }
        }

        public string Caption
        {
            get
            {
                return this.m_caption;
            }
            internal set
            {
                this.m_caption = value;
            }
        }

        public string ExternalId
        {
            get
            {
                return this.m_externalId;
            }
            internal set
            {
                this.m_externalId = value;
            }
        }

        public EmpEnumCOState CiCoState
        {
            get
            {
                return this.m_CiCoState;
            }
            internal set
            {
                this.m_CiCoState = value;
            }
        }

        internal string OrgFileName
        {
            get
            {
                return this.m_COFileName;
            }
            private set
            {
                this.m_COFileName = value;
            }
        }

        internal string OrgEmsFileName
        {
            get
            {
                return this.m_COEmsFileName;
            }
            set
            {
                this.m_COEmsFileName = value;
                this.OrgFileName = TxEmsUtilities.CalculateFullPathFromRelativeEmsPath(value);
            }
        }

        internal string ImageFileName
        {
            get
            {
                return this.m_ImageFileName;
            }
            private set
            {
                this.m_ImageFileName = value;
            }
        }

        internal string ImageEmsFileName
        {
            get
            {
                return this.m_ImageEmsFileName;
            }
            set
            {
                this.m_ImageEmsFileName = value;
                this.ImageFileName = TxEmsUtilities.CalculateFullPathFromRelativeEmsPath(value);
            }
        }

        internal int ReferenceFileId
        {
            get
            {
                return this.m_referenceFileId;
            }
            set
            {
                this.m_referenceFileId = value;
            }
        }

        internal int ReferenceImageFileId
        {
            get
            {
                return this.m_referenceImageFileId;
            }
            set
            {
                this.m_referenceImageFileId = value;
            }
        }

        internal string UserLogin
        {
            get
            {
                return this.m_userLogin;
            }
            set
            {
                this.m_userLogin = value;
            }
        }

        internal AJTUpgPrototypesUpgradeResultData Result
        {
            get
            {
                return this.m_result;
            }
            set
            {
                this.m_result = value;
            }
        }

        internal bool IsCoJtFile
        {
            get
            {
                return Path.GetExtension(this.OrgFileName).ToLower() == ".cojt";
            }
        }

        internal string COJtFileName
        {
            get
            {
                string result = string.Empty;
                if (this.IsCoJtFile)
                {
                    result = this.OrgFileName;
                }
                else
                {
                    result = this.OrgFileName + "jt";
                }
                return result;
            }
        }

        internal string COJtEmsFileName
        {
            get
            {
                string result = string.Empty;
                if (this.IsCoJtFile)
                {
                    result = this.OrgEmsFileName;
                }
                else
                {
                    result = this.OrgEmsFileName + "jt";
                }
                return result;
            }
        }

        internal void UpdateAlgoCheckOut(EUpgProcessResult algoCOResult)
        {
            this.Result.AlgoCO = algoCOResult;
            if (algoCOResult == EUpgProcessResult.Succeeded)
            {
                this.CiCoState = EmpEnumCOState.CO_Me_State;
            }
        }

        internal void UpdateComponentStatus(EUpgComponentStatus status)
        {
            this.Result.CompStatus = status;
        }

        private int m_id;

        private int m_classTypeId;

        private string m_caption;

        private string m_externalId;

        private EmpEnumCOState m_CiCoState = EmpEnumCOState.CO_Other_State;

        private string m_COFileName = string.Empty;

        private string m_COEmsFileName = string.Empty;

        private string m_ImageFileName = string.Empty;

        private string m_ImageEmsFileName = string.Empty;

        private int m_referenceFileId;

        private int m_referenceImageFileId;

        private string m_userLogin = string.Empty;

        private AJTUpgPrototypesUpgradeResultData m_result = new AJTUpgPrototypesUpgradeResultData();
    }

}
