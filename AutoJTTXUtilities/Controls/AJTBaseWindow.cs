using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AutoJTTXUtilities.Controls
{
    public partial class AJTBaseWindow : Window
    {
        public AJTBaseWindow()
        {
            //处理全局异常
            this.SetExceptionHandler();

            //快捷键事件
            this.KeyDown += AJTBaseWindow_KeyDown;
        }

        protected override void OnClosed(EventArgs e)
        {
            this.UnSetExceptionHandler();
            base.OnClosed(e);
        }

        #region 快捷键

        private void AJTBaseWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //Esc键退出  
            if (e.Key == Key.Escape)
            {
                Dispatcher.Invoke(
                      new Action(
                          delegate
                          {
                              this.Close();
                          }
                      )
                  );
            }
        }

        #endregion

        #region 处理全局异常

        //处理全局异常
        void SetExceptionHandler()
        {
            //非UI线程未捕获异常处理事件
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            //UI线程未捕获异常处理事件
            this.Dispatcher.UnhandledException += Dispatcher_UnhandledException;
        }
        void UnSetExceptionHandler()
        {
            //非UI线程未捕获异常处理事件
            AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;

            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException -= TaskScheduler_UnobservedTaskException;

            //UI线程未捕获异常处理事件
            this.Dispatcher.UnhandledException -= Dispatcher_UnhandledException;
        }

        //非UI线程未捕获异常处理事件
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            StringBuilder sbEx = new StringBuilder();
            if (e.IsTerminating)
            {
                sbEx.Append("程序发生致命错误，将终止！\n");
            }
            sbEx.Append("捕获未处理异常：");
            if (e.ExceptionObject is Exception)
            {
                sbEx.Append(((Exception)e.ExceptionObject).Message);
            }
            else
            {
                sbEx.Append(e.ExceptionObject);
            }
            MessageBox.Show(this, sbEx.ToString(), "AutoJT", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        //Task线程内未捕获异常处理事件
        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            try
            {
                //task线程内未处理捕获
                MessageBox.Show(this, "捕获线程内未处理异常：" + e.Exception.Message, "AutoJT", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch 
            {
            }
        
            e.SetObserved();//设置该异常已察觉（这样处理后就不会引起程序崩溃）
        }

        //UI线程未捕获异常处理事件
        private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                e.Handled = true; //把 Handled 属性设为true，表示此异常已处理，程序可以继续运行，不会强制退出      
                MessageBox.Show(this, "捕获未处理异常: " + e.Exception.Message, "AutoJT", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                //此时程序出现严重异常，将强制结束退出
                MessageBox.Show(this, "程序发生致命错误，将终止！", "AutoJT", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}
