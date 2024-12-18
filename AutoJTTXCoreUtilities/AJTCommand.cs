using System;
using System.Windows.Input;

namespace AutoJTTXCoreUtilities
{
    public class AJTCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public Action<object> DoExecute { get; set; }

        // 这里给个默认的值，不实现就返回true
        public Func<object, bool> DoCanExecute { get; set; } = new Func<object, bool>(obj => true);

        public bool CanExecute(object parameter)
        {
            // 让实例去实现这个委托
            return DoCanExecute?.Invoke(parameter) == true;// 绑定的对象 可用
        }

        public void Execute(object parameter)
        {
            // 让实例去实现这个委托
            DoExecute?.Invoke(parameter);
        }


        //目的 就是触发一次CanExecuteChanged事件   
        public void DoCanExecuteChanged()
        {
            // 触发事件的目的就是重新调用CanExecute方法
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}