using System;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;

namespace AutoJTTXUtilities.Controls
{
    /// <summary>
    /// Creates IWin32Window around an IntPtr
    /// </summary>
    public class WindowWrapper : System.Windows.Forms.IWin32Window
    {
        public WindowWrapper(IntPtr handle)
        {
            _hwnd = handle;
        }

        public WindowWrapper(Window window)
        {
            _hwnd = new System.Windows.Interop.WindowInteropHelper(window).Handle;
        }

        public IntPtr Handle
        {
            get { return _hwnd; }
        }

        private IntPtr _hwnd;
    }
    public class WindowHelper
    {
        public static bool IsDisposed(Window window)
        {
            return new System.Windows.Interop.WindowInteropHelper(window).Handle == IntPtr.Zero;
        }

        public static IntPtr GetWindowHandle(Window window)
        {
            return new System.Windows.Interop.WindowInteropHelper(window).Handle;
        }

        public static System.Windows.Forms.IWin32Window GetWindowIWin32Window(Window window)
        {
            System.Windows.Forms.IWin32Window win32Window = new WindowWrapper(window);
            return win32Window;
        }

        public static void SetOwner(Window wpfWindow, Window mainWindow)
        {
            WindowInteropHelper windowInteropHelper = new WindowInteropHelper(wpfWindow);
            windowInteropHelper.Owner = GetWindowHandle(mainWindow);
        }
        public static void SetOwner(Window wpfWindow, IntPtr mainWindow)
        {
            WindowInteropHelper windowInteropHelper = new WindowInteropHelper(wpfWindow);
            windowInteropHelper.Owner = mainWindow;
        }

        //ToolBar去掉右边箭头（扩展图标）
        public static void RemoveToolBarExtendIconMethod(object sender)
        {
            System.Windows.Controls.ToolBar toolBar = sender as System.Windows.Controls.ToolBar;

            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }

            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness(0);
            }
        }

        //选择文件夹
        public static string BrowseToFolder(string title, string rootPath, IntPtr mainWindow)
        {
            Type typeFromProgID = Type.GetTypeFromProgID("Shell.Application");
            object obj = Activator.CreateInstance(typeFromProgID);
            object obj2 = typeFromProgID.InvokeMember("BrowseForFolder", BindingFlags.InvokeMethod, null, obj, new object[]
            {
                mainWindow.ToInt32(),
                title,
                0,
                rootPath
            });
            if (obj2 == null)
            {
                return null;
            }
            object obj3 = obj2.GetType().InvokeMember("Self", BindingFlags.GetProperty, null, obj2, null);
            return obj3.GetType().InvokeMember("Path", BindingFlags.GetProperty, null, obj3, null) as string;
        }
    }
}
