using EngineeringInternalExtension;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Tecnomatix.Engineering;

namespace AutoJTTXCoreUtilities
{
    public class TecnomatixInfos
    {
        public TecnomatixInfos(ToolStripStatusLabel ToolStripStatusLabel_INFO1)
        {
            this.ToolStripStatusLabel_INFO = ToolStripStatusLabel_INFO1;
        }

        ToolStripStatusLabel ToolStripStatusLabel_INFO { get; set; }

        #region infos

        public void ShowInfos(string info)
        {
            Thread t = new Thread(SetTextBoxValue);
            t.Start(info);
        }
        void SetTextBoxValue(object obj)
        {
            try
            {
                ToolStripStatusLabel_INFO.Text = "";
                Thread.Sleep(50);

                ToolStripStatusLabel_INFO.ForeColor = Color.Red;
                ToolStripStatusLabel_INFO.Text = obj.ToString();

                Thread.Sleep(300);
                ToolStripStatusLabel_INFO.Text = "";
                Thread.Sleep(100);

                ToolStripStatusLabel_INFO.Text = obj.ToString();
                ToolStripStatusLabel_INFO.ForeColor = Color.Black;
            }
            catch { }
        }

        #endregion


        public static string GETNEWGuid()
        {
            return Guid.NewGuid().ToString("N");
        }
        public static string GETNEWGuid2()
        {
            return Guid.NewGuid().ToString().ToUpper();
        }

        /// <summary>
        /// 1.pd ,2.ps
        /// </summary>
        /// <returns></returns>
        public static int GetUsersDirectory()
        {
            int result = -1;
            try
            {
                string dircetory1 = Tecnomatix.Engineering.TxApplication.AllUsersDirectory;
                if (dircetory1.ToUpper().IndexOf("Process Designer".ToUpper()) != -1)
                {
                    result = 1;
                }
                else
                {
                    result = 2;
                }
            }
            catch
            {
                return -1;
            }

            return result;
        }

        /// <summary>
        /// 获取软件版本信息
        /// </summary>
        /// <returns></returns>
        public static string GetUsersDirectorySoftVersion()
        {
            string result = string.Empty;
            try
            {
                System.IO.DirectoryInfo parentVer = new System.IO.DirectoryInfo(Tecnomatix.Engineering.TxApplication.AllUsersDirectory);
                result = string.Format("{0} {1}", parentVer.Parent, parentVer.Name);
            }
            catch
            {
                return result;
            }

            return result;
        }
        /// <summary>
        /// 检查是否是ProcessDesigner
        /// </summary>
        /// <returns></returns>
        public static bool IsProcessDesigner()
        {
            bool result = false;
            TxApplicationEx txApplicationEx = new TxApplicationEx();
            if (txApplicationEx.IsNewInfra() && txApplicationEx.IsOnLine())
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Get System Root Path
        /// </summary>
        /// <returns></returns>
        public static string GetSystemRootPath()
        {
            string text = TxApplication.SystemRootDirectory;
            if (text.LastIndexOf('\\') == text.Length - 1)
            {
                text = text.Remove(text.Length - 1, 1);
            }
            return text;
        }
        public static string GetSystemRoot()
        {
            string text = TxApplication.SystemRootDirectory;
            if (text.EndsWith("/") || text.EndsWith("\\"))
            {
                text = text.Remove(text.Length - 1, 1);
            }
            return text;
        }

        public static void textBox_KeyDown(object sender, KeyEventArgs e)//TextBox被单击,设置按键某些按键
        {
            TextBox tb = (TextBox)sender;
            try
            {
                List<Keys> globalnePovoleneKlavesy = new List<Keys>();//只允许键盘输入下列按键
                globalnePovoleneKlavesy.Add(Keys.D0);
                globalnePovoleneKlavesy.Add(Keys.D1);
                globalnePovoleneKlavesy.Add(Keys.D2);
                globalnePovoleneKlavesy.Add(Keys.D3);
                globalnePovoleneKlavesy.Add(Keys.D4);
                globalnePovoleneKlavesy.Add(Keys.D5);
                globalnePovoleneKlavesy.Add(Keys.D6);
                globalnePovoleneKlavesy.Add(Keys.D7);
                globalnePovoleneKlavesy.Add(Keys.D8);
                globalnePovoleneKlavesy.Add(Keys.D9);
                globalnePovoleneKlavesy.Add(Keys.NumPad0);
                globalnePovoleneKlavesy.Add(Keys.NumPad1);
                globalnePovoleneKlavesy.Add(Keys.NumPad2);
                globalnePovoleneKlavesy.Add(Keys.NumPad3);
                globalnePovoleneKlavesy.Add(Keys.NumPad4);
                globalnePovoleneKlavesy.Add(Keys.NumPad5);
                globalnePovoleneKlavesy.Add(Keys.NumPad6);
                globalnePovoleneKlavesy.Add(Keys.NumPad7);
                globalnePovoleneKlavesy.Add(Keys.NumPad8);
                globalnePovoleneKlavesy.Add(Keys.NumPad9);
                globalnePovoleneKlavesy.Add(Keys.Left);
                globalnePovoleneKlavesy.Add(Keys.Right);
                globalnePovoleneKlavesy.Add(Keys.Back);
                globalnePovoleneKlavesy.Add(Keys.Add);
                globalnePovoleneKlavesy.Add(Keys.Subtract);

                //当前文本框内容长度大于0              按键不等于左右                                                       确定某元素是否在 List<T> 中。
                if (tb.SelectedText.Length > 0 && e.KeyCode != Keys.Left && e.KeyCode != Keys.Right && globalnePovoleneKlavesy.Contains(e.KeyCode))
                {
                    tb.Text = tb.Text.Replace(tb.SelectedText, "");
                }

                List<Keys> povoleneKlavesyProEditaci = new List<Keys>();
                povoleneKlavesyProEditaci.Add(Keys.Left);
                povoleneKlavesyProEditaci.Add(Keys.Right);
                povoleneKlavesyProEditaci.Add(Keys.Back);
                povoleneKlavesyProEditaci.Add(Keys.Add);
                povoleneKlavesyProEditaci.Add(Keys.Subtract);


                if (!povoleneKlavesyProEditaci.Contains(e.KeyCode))
                {
                    e.SuppressKeyPress = true;
                }


                if ((e.KeyCode == Keys.OemPeriod || e.KeyCode == Keys.Decimal))
                {
                    tb.AppendText(".");
                }

                if (e.KeyCode == Keys.D0 || e.KeyCode == Keys.NumPad0)
                {
                    tb.AppendText("0");
                }
                if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
                {
                    tb.AppendText("1");
                }
                if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2)
                {
                    tb.AppendText("2");
                }
                if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3)
                {
                    tb.AppendText("3");
                }
                if (e.KeyCode == Keys.D4 || e.KeyCode == Keys.NumPad4)
                {
                    tb.AppendText("4");
                }
                if (e.KeyCode == Keys.D5 || e.KeyCode == Keys.NumPad5)
                {
                    tb.AppendText("5");
                }
                if (e.KeyCode == Keys.D6 || e.KeyCode == Keys.NumPad6)
                {
                    tb.AppendText("6");
                }
                if (e.KeyCode == Keys.D7 || e.KeyCode == Keys.NumPad7)
                {
                    tb.AppendText("7");
                }
                if (e.KeyCode == Keys.D8 || e.KeyCode == Keys.NumPad8)
                {
                    tb.AppendText("8");
                }
                if (e.KeyCode == Keys.D9 || e.KeyCode == Keys.NumPad9)
                {
                    tb.AppendText("9");
                }
            }
            catch
            {
                tb.Text = "";
            }
        }

        //TextBox被单击,设置按键某些按键 只允许0~9
        public static void textBox_KeyDown2(object sender, KeyEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            try
            {
                List<Keys> globalnePovoleneKlavesy = new List<Keys>();//只允许键盘输入下列按键
                globalnePovoleneKlavesy.Add(Keys.D0);
                globalnePovoleneKlavesy.Add(Keys.D1);
                globalnePovoleneKlavesy.Add(Keys.D2);
                globalnePovoleneKlavesy.Add(Keys.D3);
                globalnePovoleneKlavesy.Add(Keys.D4);
                globalnePovoleneKlavesy.Add(Keys.D5);
                globalnePovoleneKlavesy.Add(Keys.D6);
                globalnePovoleneKlavesy.Add(Keys.D7);
                globalnePovoleneKlavesy.Add(Keys.D8);
                globalnePovoleneKlavesy.Add(Keys.D9);
                globalnePovoleneKlavesy.Add(Keys.NumPad0);
                globalnePovoleneKlavesy.Add(Keys.NumPad1);
                globalnePovoleneKlavesy.Add(Keys.NumPad2);
                globalnePovoleneKlavesy.Add(Keys.NumPad3);
                globalnePovoleneKlavesy.Add(Keys.NumPad4);
                globalnePovoleneKlavesy.Add(Keys.NumPad5);
                globalnePovoleneKlavesy.Add(Keys.NumPad6);
                globalnePovoleneKlavesy.Add(Keys.NumPad7);
                globalnePovoleneKlavesy.Add(Keys.NumPad8);
                globalnePovoleneKlavesy.Add(Keys.NumPad9);
                globalnePovoleneKlavesy.Add(Keys.Left);
                globalnePovoleneKlavesy.Add(Keys.Right);
                globalnePovoleneKlavesy.Add(Keys.Back);
                //globalnePovoleneKlavesy.Add(Keys.Add);
                //globalnePovoleneKlavesy.Add(Keys.Subtract);

                //当前文本框内容长度大于0              按键不等于左右                                                       确定某元素是否在 List<T> 中。
                if (tb.SelectedText.Length > 0 && e.KeyCode != Keys.Left && e.KeyCode != Keys.Right && globalnePovoleneKlavesy.Contains(e.KeyCode))
                {
                    tb.Text = tb.Text.Replace(tb.SelectedText, "");
                }

                List<Keys> povoleneKlavesyProEditaci = new List<Keys>();
                povoleneKlavesyProEditaci.Add(Keys.Left);
                povoleneKlavesyProEditaci.Add(Keys.Right);
                povoleneKlavesyProEditaci.Add(Keys.Back);
                //povoleneKlavesyProEditaci.Add(Keys.Add);
                //povoleneKlavesyProEditaci.Add(Keys.Subtract);


                if (!povoleneKlavesyProEditaci.Contains(e.KeyCode))
                {
                    e.SuppressKeyPress = true;
                }


                if ((e.KeyCode == Keys.OemPeriod || e.KeyCode == Keys.Decimal))
                {
                    tb.Text = "";
                }

                if (e.KeyCode == Keys.D0 || e.KeyCode == Keys.NumPad0)
                {
                    tb.Text = "0";
                }
                if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
                {
                    tb.Text = "1";
                }
                if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2)
                {
                    tb.Text = "2";
                }
                if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3)
                {
                    tb.Text = "3";
                }
                if (e.KeyCode == Keys.D4 || e.KeyCode == Keys.NumPad4)
                {
                    tb.Text = "4";
                }
                if (e.KeyCode == Keys.D5 || e.KeyCode == Keys.NumPad5)
                {
                    tb.Text = "5";
                }
                if (e.KeyCode == Keys.D6 || e.KeyCode == Keys.NumPad6)
                {
                    tb.Text = "6";
                }
                if (e.KeyCode == Keys.D7 || e.KeyCode == Keys.NumPad7)
                {
                    tb.Text = "7";
                }
                if (e.KeyCode == Keys.D8 || e.KeyCode == Keys.NumPad8)
                {
                    tb.Text = "8";
                }
                if (e.KeyCode == Keys.D9 || e.KeyCode == Keys.NumPad9)
                {
                    tb.Text = "9";
                }

            }
            catch
            {
                tb.Text = "";
            }
        }
        //TextBox被单击,设置按键某些按键 只允许0~15
        public static void textBox_KeyDown2_1(object sender, KeyEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            //初始text
            //string text1 = tb.Text;
            try
            {
                List<Keys> globalnePovoleneKlavesy = new List<Keys>();//只允许键盘输入下列按键
                globalnePovoleneKlavesy.Add(Keys.D0);
                globalnePovoleneKlavesy.Add(Keys.D1);
                globalnePovoleneKlavesy.Add(Keys.D2);
                globalnePovoleneKlavesy.Add(Keys.D3);
                globalnePovoleneKlavesy.Add(Keys.D4);
                globalnePovoleneKlavesy.Add(Keys.D5);
                globalnePovoleneKlavesy.Add(Keys.D6);
                globalnePovoleneKlavesy.Add(Keys.D7);
                globalnePovoleneKlavesy.Add(Keys.D8);
                globalnePovoleneKlavesy.Add(Keys.D9);
                globalnePovoleneKlavesy.Add(Keys.NumPad0);
                globalnePovoleneKlavesy.Add(Keys.NumPad1);
                globalnePovoleneKlavesy.Add(Keys.NumPad2);
                globalnePovoleneKlavesy.Add(Keys.NumPad3);
                globalnePovoleneKlavesy.Add(Keys.NumPad4);
                globalnePovoleneKlavesy.Add(Keys.NumPad5);
                globalnePovoleneKlavesy.Add(Keys.NumPad6);
                globalnePovoleneKlavesy.Add(Keys.NumPad7);
                globalnePovoleneKlavesy.Add(Keys.NumPad8);
                globalnePovoleneKlavesy.Add(Keys.NumPad9);
                globalnePovoleneKlavesy.Add(Keys.Left);
                globalnePovoleneKlavesy.Add(Keys.Right);
                globalnePovoleneKlavesy.Add(Keys.Back);
                //globalnePovoleneKlavesy.Add(Keys.Add);
                //globalnePovoleneKlavesy.Add(Keys.Subtract);

                //当前文本框内容长度大于0              按键不等于左右                                                       确定某元素是否在 List<T> 中。
                if (tb.SelectedText.Length > 0 && e.KeyCode != Keys.Left && e.KeyCode != Keys.Right && globalnePovoleneKlavesy.Contains(e.KeyCode))
                {
                    tb.Text = tb.Text.Replace(tb.SelectedText, "");
                }

                List<Keys> povoleneKlavesyProEditaci = new List<Keys>();
                povoleneKlavesyProEditaci.Add(Keys.Left);
                povoleneKlavesyProEditaci.Add(Keys.Right);
                povoleneKlavesyProEditaci.Add(Keys.Back);
                //povoleneKlavesyProEditaci.Add(Keys.Add);
                //povoleneKlavesyProEditaci.Add(Keys.Subtract);


                if (!povoleneKlavesyProEditaci.Contains(e.KeyCode))
                {
                    e.SuppressKeyPress = true;
                }

                //.
                if ((e.KeyCode == Keys.OemPeriod || e.KeyCode == Keys.Decimal))
                {
                    return;
                }

                //vlaue
                string value_1 = String.Empty;
                if (e.KeyCode == Keys.D0 || e.KeyCode == Keys.NumPad0)
                {
                    value_1 = "0";
                }
                if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
                {
                    value_1 = "1";
                }
                if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2)
                {
                    value_1 = "2";
                }
                if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3)
                {
                    value_1 = "3";
                }
                if (e.KeyCode == Keys.D4 || e.KeyCode == Keys.NumPad4)
                {
                    value_1 = "4";
                }
                if (e.KeyCode == Keys.D5 || e.KeyCode == Keys.NumPad5)
                {
                    value_1 = "5";
                }
                if (e.KeyCode == Keys.D6 || e.KeyCode == Keys.NumPad6)
                {
                    value_1 = "6";
                }
                if (e.KeyCode == Keys.D7 || e.KeyCode == Keys.NumPad7)
                {
                    value_1 = "7";
                }
                if (e.KeyCode == Keys.D8 || e.KeyCode == Keys.NumPad8)
                {
                    value_1 = "8";
                }
                if (e.KeyCode == Keys.D9 || e.KeyCode == Keys.NumPad9)
                {
                    value_1 = "9";
                }


                //检查是否大于15
                if (!string.IsNullOrEmpty(e.KeyCode.ToString()))
                {
                    bool bl1 = int.TryParse(tb.Text + value_1, out int result);
                    if (bl1)
                    {
                        if (result > 15)
                        {
                            tb.Text = "";
                            TxMessageBoxEx.Show("输入值大于15", "AutoJT", MessageBoxButtons.OK, MessageBoxIcon.Error, TxMessageBoxEx.TxOptions.TopMost);
                            return;
                        }
                    }
                }

                tb.AppendText(value_1);
            }
            catch
            {
                tb.Text = "";
            }
        }

        //TextBox被单击,设置按键某些按键 (所有数字, 一个小数点)
        public static void textBox_KeyDown3(object sender, KeyEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            try
            {
                List<Keys> globalnePovoleneKlavesy = new List<Keys>();//只允许键盘输入下列按键
                globalnePovoleneKlavesy.Add(Keys.D0);
                globalnePovoleneKlavesy.Add(Keys.D1);
                globalnePovoleneKlavesy.Add(Keys.D2);
                globalnePovoleneKlavesy.Add(Keys.D3);
                globalnePovoleneKlavesy.Add(Keys.D4);
                globalnePovoleneKlavesy.Add(Keys.D5);
                globalnePovoleneKlavesy.Add(Keys.D6);
                globalnePovoleneKlavesy.Add(Keys.D7);
                globalnePovoleneKlavesy.Add(Keys.D8);
                globalnePovoleneKlavesy.Add(Keys.D9);
                globalnePovoleneKlavesy.Add(Keys.NumPad0);
                globalnePovoleneKlavesy.Add(Keys.NumPad1);
                globalnePovoleneKlavesy.Add(Keys.NumPad2);
                globalnePovoleneKlavesy.Add(Keys.NumPad3);
                globalnePovoleneKlavesy.Add(Keys.NumPad4);
                globalnePovoleneKlavesy.Add(Keys.NumPad5);
                globalnePovoleneKlavesy.Add(Keys.NumPad6);
                globalnePovoleneKlavesy.Add(Keys.NumPad7);
                globalnePovoleneKlavesy.Add(Keys.NumPad8);
                globalnePovoleneKlavesy.Add(Keys.NumPad9);
                globalnePovoleneKlavesy.Add(Keys.Left);
                globalnePovoleneKlavesy.Add(Keys.Right);
                globalnePovoleneKlavesy.Add(Keys.Back);
                //globalnePovoleneKlavesy.Add(Keys.Add);
                //globalnePovoleneKlavesy.Add(Keys.Subtract);

                //当前文本框内容长度大于0              按键不等于左右                                                       确定某元素是否在 List<T> 中。
                if (tb.SelectedText.Length > 0 && e.KeyCode != Keys.Left && e.KeyCode != Keys.Right && globalnePovoleneKlavesy.Contains(e.KeyCode))
                {
                    tb.Text = tb.Text.Replace(tb.SelectedText, "");
                }

                List<Keys> povoleneKlavesyProEditaci = new List<Keys>();
                povoleneKlavesyProEditaci.Add(Keys.Left);
                povoleneKlavesyProEditaci.Add(Keys.Right);
                povoleneKlavesyProEditaci.Add(Keys.Back);
                //povoleneKlavesyProEditaci.Add(Keys.Add);
                //povoleneKlavesyProEditaci.Add(Keys.Subtract);


                if (!povoleneKlavesyProEditaci.Contains(e.KeyCode))
                {
                    e.SuppressKeyPress = true;
                }


                if ((e.KeyCode == Keys.OemPeriod || e.KeyCode == Keys.Decimal))
                {
                    if (tb.Text.IndexOf('.') == -1)
                    {
                        tb.AppendText(".");
                    }
                }

                if (e.KeyCode == Keys.D0 || e.KeyCode == Keys.NumPad0)
                {
                    tb.AppendText("0");
                }
                if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
                {
                    tb.AppendText("1");
                }
                if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2)
                {
                    tb.AppendText("2");
                }
                if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3)
                {
                    tb.AppendText("3");
                }
                if (e.KeyCode == Keys.D4 || e.KeyCode == Keys.NumPad4)
                {
                    tb.AppendText("4");
                }
                if (e.KeyCode == Keys.D5 || e.KeyCode == Keys.NumPad5)
                {
                    tb.AppendText("5");
                }
                if (e.KeyCode == Keys.D6 || e.KeyCode == Keys.NumPad6)
                {
                    tb.AppendText("6");
                }
                if (e.KeyCode == Keys.D7 || e.KeyCode == Keys.NumPad7)
                {
                    tb.AppendText("7");
                }
                if (e.KeyCode == Keys.D8 || e.KeyCode == Keys.NumPad8)
                {
                    tb.AppendText("8");
                }
                if (e.KeyCode == Keys.D9 || e.KeyCode == Keys.NumPad9)
                {
                    tb.AppendText("9");
                }
            }
            catch
            {
                tb.Text = "";
            }
        }

        //TextBox被单击,设置按键某些按键 (所有数字, 一个小数点, 负号)
        public static void textBox_KeyDown4(object sender, KeyEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            try
            {
                List<Keys> globalnePovoleneKlavesy = new List<Keys>();//只允许键盘输入下列按键
                globalnePovoleneKlavesy.Add(Keys.D0);
                globalnePovoleneKlavesy.Add(Keys.D1);
                globalnePovoleneKlavesy.Add(Keys.D2);
                globalnePovoleneKlavesy.Add(Keys.D3);
                globalnePovoleneKlavesy.Add(Keys.D4);
                globalnePovoleneKlavesy.Add(Keys.D5);
                globalnePovoleneKlavesy.Add(Keys.D6);
                globalnePovoleneKlavesy.Add(Keys.D7);
                globalnePovoleneKlavesy.Add(Keys.D8);
                globalnePovoleneKlavesy.Add(Keys.D9);
                globalnePovoleneKlavesy.Add(Keys.NumPad0);
                globalnePovoleneKlavesy.Add(Keys.NumPad1);
                globalnePovoleneKlavesy.Add(Keys.NumPad2);
                globalnePovoleneKlavesy.Add(Keys.NumPad3);
                globalnePovoleneKlavesy.Add(Keys.NumPad4);
                globalnePovoleneKlavesy.Add(Keys.NumPad5);
                globalnePovoleneKlavesy.Add(Keys.NumPad6);
                globalnePovoleneKlavesy.Add(Keys.NumPad7);
                globalnePovoleneKlavesy.Add(Keys.NumPad8);
                globalnePovoleneKlavesy.Add(Keys.NumPad9);
                globalnePovoleneKlavesy.Add(Keys.Left);
                globalnePovoleneKlavesy.Add(Keys.Right);
                globalnePovoleneKlavesy.Add(Keys.Back);
                //globalnePovoleneKlavesy.Add(Keys.Add);
                globalnePovoleneKlavesy.Add(Keys.Subtract);

                //当前文本框内容长度大于0              按键不等于左右                                                       确定某元素是否在 List<T> 中。
                if (tb.SelectedText.Length > 0 && e.KeyCode != Keys.Left && e.KeyCode != Keys.Right && globalnePovoleneKlavesy.Contains(e.KeyCode))
                {
                    tb.Text = tb.Text.Replace(tb.SelectedText, "");
                }

                List<Keys> povoleneKlavesyProEditaci = new List<Keys>();
                povoleneKlavesyProEditaci.Add(Keys.Left);
                povoleneKlavesyProEditaci.Add(Keys.Right);
                povoleneKlavesyProEditaci.Add(Keys.Back);
                //povoleneKlavesyProEditaci.Add(Keys.Add);
                //povoleneKlavesyProEditaci.Add(Keys.Subtract);


                if (!povoleneKlavesyProEditaci.Contains(e.KeyCode))
                {
                    e.SuppressKeyPress = true;
                }

                if (e.KeyCode == Keys.Subtract || e.KeyCode == Keys.OemMinus)
                {
                    if (tb.Text.IndexOf('-') == -1)
                    {
                        tb.AppendText("-");
                    }
                }

                if ((e.KeyCode == Keys.OemPeriod || e.KeyCode == Keys.Decimal))
                {
                    if (tb.Text.IndexOf('.') == -1)
                    {
                        tb.AppendText(".");
                    }
                }

                if (e.KeyCode == Keys.D0 || e.KeyCode == Keys.NumPad0)
                {
                    tb.AppendText("0");
                }
                if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
                {
                    tb.AppendText("1");
                }
                if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2)
                {
                    tb.AppendText("2");
                }
                if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3)
                {
                    tb.AppendText("3");
                }
                if (e.KeyCode == Keys.D4 || e.KeyCode == Keys.NumPad4)
                {
                    tb.AppendText("4");
                }
                if (e.KeyCode == Keys.D5 || e.KeyCode == Keys.NumPad5)
                {
                    tb.AppendText("5");
                }
                if (e.KeyCode == Keys.D6 || e.KeyCode == Keys.NumPad6)
                {
                    tb.AppendText("6");
                }
                if (e.KeyCode == Keys.D7 || e.KeyCode == Keys.NumPad7)
                {
                    tb.AppendText("7");
                }
                if (e.KeyCode == Keys.D8 || e.KeyCode == Keys.NumPad8)
                {
                    tb.AppendText("8");
                }
                if (e.KeyCode == Keys.D9 || e.KeyCode == Keys.NumPad9)
                {
                    tb.AppendText("9");
                }
            }
            catch
            {
                tb.Text = "";
            }
        }

        public static void comboBox_KeyDown3(object sender, KeyEventArgs e)//TextBox被单击,设置按键某些按键
        {
            ComboBox comBox = (ComboBox)sender;
            try
            {
                List<Keys> globalnePovoleneKlavesy = new List<Keys>();//只允许键盘输入下列按键
                globalnePovoleneKlavesy.Add(Keys.D0);
                globalnePovoleneKlavesy.Add(Keys.D1);
                globalnePovoleneKlavesy.Add(Keys.D2);
                globalnePovoleneKlavesy.Add(Keys.D3);
                globalnePovoleneKlavesy.Add(Keys.D4);
                globalnePovoleneKlavesy.Add(Keys.D5);
                globalnePovoleneKlavesy.Add(Keys.D6);
                globalnePovoleneKlavesy.Add(Keys.D7);
                globalnePovoleneKlavesy.Add(Keys.D8);
                globalnePovoleneKlavesy.Add(Keys.D9);
                globalnePovoleneKlavesy.Add(Keys.NumPad0);
                globalnePovoleneKlavesy.Add(Keys.NumPad1);
                globalnePovoleneKlavesy.Add(Keys.NumPad2);
                globalnePovoleneKlavesy.Add(Keys.NumPad3);
                globalnePovoleneKlavesy.Add(Keys.NumPad4);
                globalnePovoleneKlavesy.Add(Keys.NumPad5);
                globalnePovoleneKlavesy.Add(Keys.NumPad6);
                globalnePovoleneKlavesy.Add(Keys.NumPad7);
                globalnePovoleneKlavesy.Add(Keys.NumPad8);
                globalnePovoleneKlavesy.Add(Keys.NumPad9);
                globalnePovoleneKlavesy.Add(Keys.Left);
                globalnePovoleneKlavesy.Add(Keys.Right);
                globalnePovoleneKlavesy.Add(Keys.Back);
                //globalnePovoleneKlavesy.Add(Keys.Add);
                //globalnePovoleneKlavesy.Add(Keys.Subtract);

                //当前文本框内容长度大于0              按键不等于左右                                                       确定某元素是否在 List<T> 中。
                if (comBox.SelectedText.Length > 0 && e.KeyCode != Keys.Left && e.KeyCode != Keys.Right && globalnePovoleneKlavesy.Contains(e.KeyCode))
                {
                    comBox.Text = comBox.Text.Replace(comBox.SelectedText, "");
                }

                List<Keys> povoleneKlavesyProEditaci = new List<Keys>();
                povoleneKlavesyProEditaci.Add(Keys.Left);
                povoleneKlavesyProEditaci.Add(Keys.Right);
                povoleneKlavesyProEditaci.Add(Keys.Back);
                //povoleneKlavesyProEditaci.Add(Keys.Add);
                //povoleneKlavesyProEditaci.Add(Keys.Subtract);


                if (!povoleneKlavesyProEditaci.Contains(e.KeyCode))
                {
                    e.SuppressKeyPress = true;
                }


                if ((e.KeyCode == Keys.OemPeriod || e.KeyCode == Keys.Decimal))
                {
                    if (comBox.Text.IndexOf('.') == -1)
                    {
                        comBox.Text += '.';
                    }
                }

                if (e.KeyCode == Keys.D0 || e.KeyCode == Keys.NumPad0)
                {
                    comBox.Text += '0';
                }
                if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
                {
                    comBox.Text += '1';
                }
                if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2)
                {
                    comBox.Text += '2';
                }
                if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3)
                {
                    comBox.Text += '3';
                }
                if (e.KeyCode == Keys.D4 || e.KeyCode == Keys.NumPad4)
                {
                    comBox.Text += '4';
                }
                if (e.KeyCode == Keys.D5 || e.KeyCode == Keys.NumPad5)
                {
                    comBox.Text += '5';
                }
                if (e.KeyCode == Keys.D6 || e.KeyCode == Keys.NumPad6)
                {
                    comBox.Text += '6';
                }
                if (e.KeyCode == Keys.D7 || e.KeyCode == Keys.NumPad7)
                {
                    comBox.Text += '7';
                }
                if (e.KeyCode == Keys.D8 || e.KeyCode == Keys.NumPad8)
                {
                    comBox.Text += '8';
                }
                if (e.KeyCode == Keys.D9 || e.KeyCode == Keys.NumPad9)
                {
                    comBox.Text += '9';
                }

                comBox.Select(comBox.Text.Length, 0);
            }
            catch
            {
                comBox.Text = "";
            }
        }

        public static IntPtr GetAppHandle()
        {
           return TxApplicationEx.GetMainWindowHandle(IntPtr.Zero);
        }
    }
}