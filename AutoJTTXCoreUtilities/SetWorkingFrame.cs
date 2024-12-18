using EngineeringInternalExtension;
using System;
using System.Windows.Forms;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Ui;

namespace AutoJTTXCoreUtilities
{
    public partial class SetWorkingFrame : TxForm
    {
        private CUiContinuousButton cUiContinuousButton_ok;
        private CUiContinuousButton cUiContinuousButton_cancel;
        private TxFrameEditBoxCtrl txFrameEditBoxCtrl1;


        TxFrameEditBoxCtrl_ValidFrameSetEventArgs _ValidFrameArgs;
        public Action<string> IsSetWorkingFrame;//之前的定义委托和定义事件由这一句话代替

        //父级的控件
        Tecnomatix.Engineering.Ui.WPF.TxObjectGridControl m_txObjectCtr;
        Tecnomatix.Engineering.Ui.TxObjGridCtrl m_txObjectCtr2;
        System.Windows.Controls.Button m_button;
        public SetWorkingFrame(Tecnomatix.Engineering.Ui.WPF.TxObjectGridControl txObjectCtr = null,
            Tecnomatix.Engineering.Ui.TxObjGridCtrl txObjectCtr2 = null, System.Windows.Controls.Button button = null)
        {
            InitializeComponent();
            Text = "Set Working Frame";

            //显示dpi
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoSize = false;

            Width = 250;
            Height = 130;
            MinimumSize = new System.Drawing.Size(250, 130);
            ShowIcon = false;
            MinimizeBox = false;
            MaximizeBox = false;
            AcceptButton = cUiContinuousButton_ok;
            CancelButton = cUiContinuousButton_cancel;

            this.m_txObjectCtr = txObjectCtr;
            this.m_txObjectCtr2 = txObjectCtr2;
            this.m_button = button;
        }
        private void SetWorkingFrame_Load(object sender, EventArgs e)
        {
            txFrameEditBoxCtrl1.Focus();
            cUiContinuousButton_ok.Enabled = false;
            _ValidFrameArgs = null;

            try
            {
                if (this.m_button != null)
                {
                    this.m_button.IsEnabled = false;
                }
            }
            catch
            {
            }
        }
        private void InitializeComponent()
        {
            this.txFrameEditBoxCtrl1 = new Tecnomatix.Engineering.Ui.TxFrameEditBoxCtrl();
            this.cUiContinuousButton_ok = new Tecnomatix.Engineering.Ui.CUiContinuousButton();
            this.cUiContinuousButton_cancel = new Tecnomatix.Engineering.Ui.CUiContinuousButton();
            this.SuspendLayout();
            // 
            // txFrameEditBoxCtrl1
            // 
            this.txFrameEditBoxCtrl1.EnableRelativeToWorkingFrameCoordinates = false;
            this.txFrameEditBoxCtrl1.ListenToPick = true;
            this.txFrameEditBoxCtrl1.Location = new System.Drawing.Point(17, 17);
            this.txFrameEditBoxCtrl1.Name = "txFrameEditBoxCtrl1";
            this.txFrameEditBoxCtrl1.PickLevel = Tecnomatix.Engineering.Ui.TxPickLevel.Application;
            this.txFrameEditBoxCtrl1.Size = new System.Drawing.Size(200, 22);
            this.txFrameEditBoxCtrl1.TabIndex = 16;
            this.txFrameEditBoxCtrl1.ValidatorType = Tecnomatix.Engineering.Ui.TxValidatorType.AnyLocatableObject;
            this.txFrameEditBoxCtrl1.VisualizePickedFrameInGraphicViewer = true;
            this.txFrameEditBoxCtrl1.ValidFrameSet += new Tecnomatix.Engineering.Ui.TxFrameEditBoxCtrl_ValidFrameSetEventHandler(this.txFrameEditBoxCtrl1_ValidFrameSet);
            // 
            // cUiContinuousButton_ok
            // 
            this.cUiContinuousButton_ok.ClickInterval = 100;
            this.cUiContinuousButton_ok.Location = new System.Drawing.Point(47, 54);
            this.cUiContinuousButton_ok.Name = "cUiContinuousButton_ok";
            this.cUiContinuousButton_ok.Size = new System.Drawing.Size(81, 26);
            this.cUiContinuousButton_ok.TabIndex = 17;
            this.cUiContinuousButton_ok.Text = "OK";
            this.cUiContinuousButton_ok.UseVisualStyleBackColor = true;
            this.cUiContinuousButton_ok.Click += new System.EventHandler(this.cUiContinuousButton_ok_Click);
            // 
            // cUiContinuousButton_cancel
            // 
            this.cUiContinuousButton_cancel.ClickInterval = 100;
            this.cUiContinuousButton_cancel.Location = new System.Drawing.Point(136, 54);
            this.cUiContinuousButton_cancel.Name = "cUiContinuousButton_cancel";
            this.cUiContinuousButton_cancel.Size = new System.Drawing.Size(81, 26);
            this.cUiContinuousButton_cancel.TabIndex = 17;
            this.cUiContinuousButton_cancel.Text = "Cancel";
            this.cUiContinuousButton_cancel.UseVisualStyleBackColor = true;
            this.cUiContinuousButton_cancel.Click += new System.EventHandler(this.cUiContinuousButton_cancel_Click);
            // 
            // SetWorkingFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(234, 92);
            this.Controls.Add(this.cUiContinuousButton_cancel);
            this.Controls.Add(this.cUiContinuousButton_ok);
            this.Controls.Add(this.txFrameEditBoxCtrl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetWorkingFrame";
            this.ShowIcon = false;
            this.Text = "Set Working Frame";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SetWorkingFrame_FormClosed);
            this.Load += new System.EventHandler(this.SetWorkingFrame_Load);
            this.ResumeLayout(false);

        }

        private void cUiContinuousButton_cancel_Click(object sender, EventArgs e)
        {
            IsSetWorkingFrame?.Invoke(null);//执行委托实例  
            Close();
        }
        private void txFrameEditBoxCtrl1_ValidFrameSet(object sender, TxFrameEditBoxCtrl_ValidFrameSetEventArgs args)
        {
            cUiContinuousButton_ok.Enabled = true;
            _ValidFrameArgs = args;
        }
        private void cUiContinuousButton_ok_Click(object sender, EventArgs e)
        {
            if (_ValidFrameArgs == null)
            {
                return;
            }

            //set working frame
            try
            {
                SetWorkFrameMethod(out string locationString);
                IsSetWorkingFrame?.Invoke(locationString);//执行委托实例  
                Close();
            }
            catch (Exception ex)
            {
                TxMessageBoxEx.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, TxMessageBoxEx.TxOptions.TopMost);
                IsSetWorkingFrame?.Invoke(null);//执行委托实例  
            }
        }

        void SetWorkFrameMethod(out string locationString)
        {
            locationString = null;
            TxDocument txDocument = TxApplication.ActiveDocument;
            txDocument.WorkingFrame = _ValidFrameArgs.Location;

            TxVector txVector = _ValidFrameArgs.Location.Translation;
            locationString = txVector.X.ToString() + txVector.Y.ToString() + txVector.Z.ToString();
        }

        private void SetWorkingFrame_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (this.m_txObjectCtr != null) { this.m_txObjectCtr.Focus(); }
            }
            catch
            {
            }

            try
            {
                if (this.m_txObjectCtr2 != null) { this.m_txObjectCtr2.Focus(); }
            }
            catch
            {
            }

            try
            {
                if (this.m_button != null)
                {
                    this.m_button.IsEnabled = true;
                }
            }
            catch
            {
            }

            try
            {
                this.txFrameEditBoxCtrl1.ValidFrameSet -= txFrameEditBoxCtrl1_ValidFrameSet;
                this.txFrameEditBoxCtrl1.UnInitialize();
                this.txFrameEditBoxCtrl1 = null;
            }
            catch
            {
            }
        }
    }
}