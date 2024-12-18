namespace AutoJTTXUtilities.DataHandling
{
    public enum EUpgProcessResult
    {
        None,
        Started,
        Failed,
        Succeeded,
        Skipped,
        Canceled,
        Unknown
    }
    public enum EUpgComponentStatus
    {
        None,
        ComponentAlreadyCoJt,
        NoComponent,
        CoJtComponentExists,
        ComponentDoesNoExist,
        InvalidComponentType,
        InvalidComponent,
        Valid
    }
    public class AJTUpgPrototypesUpgradeResultData
    {
        internal EUpgProcessResult CompResult
        {
            get
            {
                return this.m_compResult;
            }
            set
            {
                this.m_compResult = value;
            }
        }

        public EUpgComponentStatus CompStatus
        {
            get
            {
                return this.m_compStatus;
            }
            set
            {
                this.m_compStatus = value;
            }
        }

        internal EUpgProcessResult PrototypeSetDataResult
        {
            get
            {
                return this.m_prototypeSetDataResult;
            }
            set
            {
                this.m_prototypeSetDataResult = value;
            }
        }

        public EUpgProcessResult AlgoCO
        {
            get
            {
                return this.m_algoCO;
            }
            set
            {
                this.m_algoCO = value;
            }
        }

        internal EUpgProcessResult UpgradeResult(bool mustSetData)
        {
            EUpgProcessResult result = EUpgProcessResult.None;
            switch (this.CompResult)
            {
                case EUpgProcessResult.None:
                    {
                        EUpgProcessResult algoCO = this.AlgoCO;
                        if (algoCO <= EUpgProcessResult.Succeeded)
                        {
                            result = EUpgProcessResult.Failed;
                        }
                        EUpgComponentStatus compStatus = this.CompStatus;
                        if (compStatus > EUpgComponentStatus.InvalidComponent)
                        {
                            if (compStatus != EUpgComponentStatus.Valid)
                            {
                            }
                        }
                        else
                        {
                            result = EUpgProcessResult.Failed;
                        }
                        break;
                    }
                case EUpgProcessResult.Failed:
                    result = EUpgProcessResult.Failed;
                    break;
                case EUpgProcessResult.Succeeded:
                    switch (this.PrototypeSetDataResult)
                    {
                        case EUpgProcessResult.None:
                            if (mustSetData)
                            {
                                result = EUpgProcessResult.Failed;
                            }
                            else
                            {
                                result = EUpgProcessResult.Succeeded;
                            }
                            break;
                        case EUpgProcessResult.Started:
                        case EUpgProcessResult.Failed:
                            result = EUpgProcessResult.Failed;
                            break;
                        case EUpgProcessResult.Succeeded:
                            result = EUpgProcessResult.Succeeded;
                            break;
                    }
                    break;
                case EUpgProcessResult.Skipped:
                    result = EUpgProcessResult.Skipped;
                    break;
                case EUpgProcessResult.Unknown:
                    result = EUpgProcessResult.Unknown;
                    break;
            }
            return result;
        }

        private EUpgProcessResult m_compResult;

        private EUpgComponentStatus m_compStatus;

        private EUpgProcessResult m_prototypeSetDataResult;

        private EUpgProcessResult m_algoCO;
    }

}
