using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AutoJTTXUtilities.AJTLoadingHandling
{

    public partial class Fmloading : Form
    {
        public int i_c;
        private Image bgImg;
        public Timer timer;
        public int fla_1;
        public string fla_2;
        public string str;
        public string fm_close;

        public string fml_close
        {
            get
            {
                return fm_close;
            }
            set
            {
                fm_close = value;
            }
        }

        public Fmloading()
        {
            InitializeComponent();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var createParams = base.CreateParams;
                createParams.Style |= 131072;
                var flag = !DesignMode;
                if (flag)
                {
                    createParams.ExStyle |= 524288;
                }
                return createParams;
            }
        }

        public void SetBits(Bitmap bitmap)
        {
            var flag9 = Image.IsCanonicalPixelFormat(bitmap.PixelFormat) && Image.IsAlphaPixelFormat(bitmap.PixelFormat);
            if (flag9)
            {
                var hObj = IntPtr.Zero;
                var dc = HelpWin32.GetDC(IntPtr.Zero);
                var intPtr = IntPtr.Zero;
                var intPtr2 = HelpWin32.CreateCompatibleDC(dc);
                try
                {
                    var point = new HelpWin32.Point(Left, Top);
                    var size = new HelpWin32.Size(bitmap.Width, bitmap.Height);
                    var blendfunction = default(HelpWin32.BLENDFUNCTION);
                    var point2 = new HelpWin32.Point(0, 0);
                    intPtr = bitmap.GetHbitmap(Color.FromArgb(0));
                    hObj = HelpWin32.SelectObject(intPtr2, intPtr);
                    blendfunction.BlendOp = 0;
                    blendfunction.SourceConstantAlpha = byte.MaxValue;
                    blendfunction.AlphaFormat = 1;
                    blendfunction.BlendFlags = 0;
                    HelpWin32.UpdateLayeredWindow(Handle, dc, ref point, ref size, intPtr2, ref point2, 0, ref blendfunction, 2);
                    return;
                }
                finally
                {
                    var flag10 = intPtr != IntPtr.Zero;
                    if (flag10)
                    {
                        HelpWin32.SelectObject(intPtr2, hObj);
                        HelpWin32.DeleteObject(intPtr);
                    }
                    HelpWin32.ReleaseDC(IntPtr.Zero, dc);
                    HelpWin32.DeleteDC(intPtr2);
                }
            }
            throw new ApplicationException("图片必须是32位带Alhpa通道的图片。");
        }

        public void InitializeComponent()
        {
            timer = new Timer();
            fm_close = "窗体已开启";
            SuspendLayout();
            ShowInTaskbar = false;
            this.ShowIcon = false;

            AutoScaleDimensions = new SizeF(6f, 12f);
            AutoScaleMode = AutoScaleMode.Font;
            ForeColor = Color.Aqua;
            FormBorderStyle = FormBorderStyle.None;
            Name = "Fmloading";
            Text = "Fmloading";
            TopMost = true;
            ClientSize = new Size(120, 120);
            Location = (Point)new Size(500, 500);
            StartPosition = FormStartPosition.CenterScreen;
            Set_png();
            ResumeLayout(false);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                var flag9 = fm_close != "窗体已开启";
                if (flag9)
                {
                    Close();
                }
                var flag18 = i_c >= fla_1;
                if (flag18)
                {
                    i_c = 0;
                }
                bgImg = (Image)new ComponentResourceManager(typeof(Fmloading)).GetObject(i_c + fla_2 + ".png");
                SetBits((Bitmap)bgImg);
                i_c++;
            }
            catch
            {
                //MessageBox.Show("加载窗体关闭报错");
            }
        }

        public void Set_png()
        {
            string a;
            try
            {
                a = "窗体";//IniHelp.GetValue("配置", "窗体动画");
            }
            catch
            {
                a = "窗体";
            }
            var flag6 = a == "窗体";

            if (flag6)
            {
                timer.Interval = 80;
                fla_1 = 4;
                fla_2 = "_load";
            }
            else
            {
                timer.Interval = 80;
                fla_1 = 4;
                fla_2 = "_load";
            }
            bgImg = null;
            i_c = 0;
            timer.Tick += timer1_Tick;
            timer.Start();
        }
    }
}