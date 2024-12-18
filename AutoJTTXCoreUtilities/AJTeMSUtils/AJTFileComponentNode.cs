using System.Drawing;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Ui;

namespace AutoJTTXCoreUtilities.AJTeMSUtils
{
    public class AJTFileComponentNode : IAJTComponentNodeComposite
    {
        public AJTFileComponentNode(string name, string fullPath)
        {
            this.Name = name;
            this.FullPath = fullPath;
            this.Icon = null;
        }

        public AJTFileComponentNode(string name, string fullPath, TxPlanningTypeMetaData prototypeType) : this(name, fullPath)
        {
            if (prototypeType != null)
            {
                this._type = prototypeType;
                this.Icon = TxImageProvider.GetImageByPlanningType(this._type.TypeName);
                this.IsAssigned = true;
            }
        }

        public bool IsAssigned { get; private set; }

        public bool IsDirty { get; private set; }

        public string Name { get; private set; }

        public Image Icon { get; private set; }

        public string FullPath { get; private set; }

        public string ExternalID { get; set; }

        public TxPlanningTypeMetaData TypeMetaData
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
                if (this._type != null)
                {
                    this.Icon = TxImageProvider.GetImageByPlanningType(this._type.TypeName);
                    this.IsDirty = true;
                    return;
                }
                this.Icon = null;
                this.IsDirty = false;
            }
        }

        public string TypeName
        {
            get
            {
                string result = null;
                if (this._type != null)
                {
                    return this._type.DisplayName;
                }
                return result;
            }
        }

        private TxPlanningTypeMetaData _type;
    }
}
