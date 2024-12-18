





using System;
using System.Windows;
using System.Windows.Interop;


namespace AutoJTTXUtilities.Controls
{
  public class WindowWrapper : System.Windows.Forms.IWin32Window
  {
    private IntPtr _hwnd;

    public WindowWrapper(IntPtr handle) => this._hwnd = handle;

    public WindowWrapper(Window window) => this._hwnd = new WindowInteropHelper(window).Handle;

    public IntPtr Handle => this._hwnd;
  }
}
