using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace AutoJTTXUtilities.Controls.AJTToast
{
    /// <summary>
    /// Toast.xaml 的交互逻辑
    /// </summary>
    public partial class Toast : UserControl
    {


        //User-Changeable Values
        public string Message { get; set; }
        public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(2);
        public TimeSpan DurationAnimation { set; get; } = TimeSpan.FromMilliseconds(300);
        public ToastDuration DurationToast
        {
            set
            {
                Duration = ToastDurationToTimeSpan(value);
                _internalDurationToast = value;
            }
            get
            {
                return _internalDurationToast;
            }
        }

        public new object Content
        {
            set
            {
                Message = value as string;
            }
            get
            {
                return Message;
            }
        }

        public enum ToastDuration { Short, Medium, Long }

        private ToastDuration _internalDurationToast;
        private Thread _waitThread;

        public Toast()
        {
            InitializeComponent();
            this.Opacity = 0;
        }


        public void Show(string message, TimeSpan duration)
        {
            ToastContent.Text = message;

            try
            {
                if (_waitThread != null)
                {
                    _waitThread.Abort();
                }
            }
            catch (Exception) { }


            DoubleAnimation anim = new DoubleAnimation(0d, 1d, this.DurationAnimation);

            anim.Completed += delegate
            {
                DelayedClose();
            };


            if (this.Opacity > 0)
            {
                DoubleAnimation fadeOut = new DoubleAnimation(this.Opacity, 0d, TimeSpan.FromMilliseconds(150));

                fadeOut.Completed += delegate
                {
                    this.BeginAnimation(UserControl.OpacityProperty, anim);
                };

                this.BeginAnimation(UserControl.OpacityProperty, fadeOut);
            }
            else
            {
                this.BeginAnimation(UserControl.OpacityProperty, anim);
            }
        }

        public void Show()
        {
            Show(this.Message, this.Duration);
        }

        public void Show(string message)
        {
            Show(message, this.Duration);
        }

        public void Show(TimeSpan duration)
        {
            Show(this.Message, duration);
        }

        public void Show(ToastDuration duration)
        {
            Show(this.Message, ToastDurationToTimeSpan(duration));
        }

        public void Hide()
        {
            DoubleAnimation anim = new DoubleAnimation(this.Opacity, 0d, this.DurationAnimation);
            this.BeginAnimation(UserControl.OpacityProperty, anim);
        }

        private void DelayedClose()
        {
            _waitThread = new Thread(() =>
            {
                try
                {
                    Thread.Sleep(this.Duration);

                    Dispatcher.Invoke(() =>
                    {
                        Hide();
                    });
                }
                catch (Exception) { }
            });
            _waitThread.Start();
        }

        private TimeSpan ToastDurationToTimeSpan(ToastDuration tduration)
        {
            TimeSpan ret = Duration;

            switch (tduration)
            {
                case ToastDuration.Short:
                    ret = TimeSpan.FromSeconds(2);
                    break;
                case ToastDuration.Medium:
                    ret = TimeSpan.FromSeconds(3);
                    break;
                case ToastDuration.Long:
                    ret = TimeSpan.FromSeconds(4);
                    break;
            }

            return ret;
        }



        //#region - 用于绑定ViewModel部分 -

        //public ICommand Command
        //{
        //    get { return (ICommand)GetValue(CommandProperty); }
        //    set { SetValue(CommandProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty CommandProperty =
        //    DependencyProperty.Register("Command", typeof(ICommand), typeof(Toast), new PropertyMetadata(default(ICommand)));

        //public object CommandParameter
        //{
        //    get { return (object)GetValue(CommandParameterProperty); }
        //    set { SetValue(CommandParameterProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty CommandParameterProperty =
        //    DependencyProperty.Register("CommandParameter", typeof(object), typeof(Toast), new PropertyMetadata(default(object)));

        //public IInputElement CommandTarget { get; set; }

        //#endregion

        //#region 用于Xaml触发路由事件部分

        ////声明和注册路由事件
        //public static readonly RoutedEvent MyEventRoutedEvent =
        //    EventManager.RegisterRoutedEvent("MyEvent", RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(Toast));
        ////CLR事件包装
        //public event RoutedEventHandler MyEvent
        //{
        //    add { this.AddHandler(MyEventRoutedEvent, value); }
        //    remove { this.RemoveHandler(MyEventRoutedEvent, value); }
        //}

        ////激发路由事件,借用Click事件的激发方法

        //protected void OnMyEvent()
        //{
        //    RoutedEventArgs args = new RoutedEventArgs(MyEventRoutedEvent, this);
        //    this.RaiseEvent(args);
        //}

        //#endregion
    }
}
