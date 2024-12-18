using System;

namespace AutoJTTXCoreUtilities
{
    public class ChangesMonitorValue
    {
        #region [私有属性]

        //监测值
        private string monitorValue;

        //事件触发函数(值变化)
        private void WhenMyValueChange()
        {
            if (OnMyValueChanged != null)
            {
                OnMyValueChanged(this, null);

            }
        }

        #endregion [私有属性]

        #region [公有属性]

        /// <summary>
        /// 定义的委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void MyValueChanged(object sender, EventArgs e);
        /// <summary>
        /// 与委托相关联的事件
        /// </summary>
        public event MyValueChanged OnMyValueChanged;

        /// <summary>
        /// 监测值
        /// </summary>
        public string MonitorValue
        {
            get { return this.monitorValue; }
            set
            {
                if (value != this.monitorValue)
                {
                    //提前赋值
                    this.monitorValue = value;
                    //事件触发函数(值变化)
                    WhenMyValueChange();
                }
            }
        }

        #endregion [公有属性]
    }

    public class ChangesMonitorValue_bool
    {
        private bool monitorValue;
        public bool MonitorValue
        {
            get { return monitorValue; }
            set
            {
                if (value != monitorValue)
                {
                    monitorValue = value;//提前赋值
                    WhenMyValueChange();
                }
            }
        }
        //定义的委托
        public delegate void MyValueChanged(object sender, EventArgs e);
        //与委托相关联的事件
        public event MyValueChanged OnMyValueChanged;
        //事件触发函数
        private void WhenMyValueChange()
        {
            if (OnMyValueChanged != null)
            {
                OnMyValueChanged(this, null);

            }
        }
    }
}