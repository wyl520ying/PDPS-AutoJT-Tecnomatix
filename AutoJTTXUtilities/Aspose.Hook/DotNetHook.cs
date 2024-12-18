using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoJTTXUtilities.Aspose.Hook
{
    public class DotNetHook : IDisposable
    {
        private class Cache
        {
            /// <summary>
            /// 原方法
            /// </summary>
            public MethodBase MethodFrom { get; set; }
            /// <summary>
            /// 目标方法
            /// </summary>
            public MethodBase MethodTo { get; set; }
            /// <summary>
            /// 原方法地址
            /// </summary>
            public IntPtr MethodFromAddress { get; set; } = IntPtr.Zero;
            /// <summary>
            /// 目标方法地址
            /// </summary>
            public IntPtr MethodToAddress { get; set; } = IntPtr.Zero;
            /// <summary>
            /// 原方法指针位置的值
            /// </summary>
            public byte[] MethodFromValue { get; set; }

            /// <summary>
            /// 目标方法指针位置的值
            /// </summary>
            public byte[] MethodToValue { get; set; }

        }

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                Remove();
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DotNetHook()
        {
            Dispose(false);
        }

        public bool IsEnabled { get; protected set; }

        private Cache _Cache = new Cache();

        public DotNetHook(MethodBase from, MethodBase to)
        {
            _Cache.MethodFrom = from;
            _Cache.MethodTo = to;
        }

        public void Apply()
        {
            if (IsEnabled == false)
            {
                Redirect();
                IsEnabled = true;
            }
        }

        public void ReApply()
        {

            try
            {
                if (IsEnabled == false)
                {
                    //hook again
                    Marshal.Copy(_Cache.MethodToValue, 0, _Cache.MethodFromAddress + 8, _Cache.MethodFromValue.Length);
                    IsEnabled = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ReApply error:" + e.Message);
            }
        }

        public void Remove()
        {
            try
            {
                if (IsEnabled == true)
                {
                    //restore value
                    Marshal.Copy(_Cache.MethodFromValue, 0, _Cache.MethodFromAddress + 8, _Cache.MethodFromValue.Length);
                    IsEnabled = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Remove error:" + e.Message);
            }
        }

        public T InvokeOriginal<T>(object instance, params object[] args)
        {
            try
            {
                Remove();
                var ret = _Cache.MethodFrom.Invoke(instance, args);
                ReApply();
                return (T)Convert.ChangeType(ret, typeof(T));
            }
            catch (Exception e)
            {
                Console.WriteLine("call error:" + e.Message);
            }

            ReApply();
            return default(T);

        }

        private void Redirect()
        {
            RuntimeHelpers.PrepareMethod(_Cache.MethodFrom.MethodHandle);
            RuntimeHelpers.PrepareMethod(_Cache.MethodTo.MethodHandle);

            Console.WriteLine(_Cache.MethodFrom.Name + " MT:" + "0x" + _Cache.MethodFrom.DeclaringType.TypeHandle.Value.ToString("x"));

            _Cache.MethodFromAddress = _Cache.MethodFrom.MethodHandle.Value;
            _Cache.MethodToAddress = _Cache.MethodTo.MethodHandle.Value;

            _Cache.MethodFromValue = new byte[IntPtr.Size];
            _Cache.MethodToValue = new byte[IntPtr.Size];

            //save value before hook
            Marshal.Copy(_Cache.MethodToAddress + 8, _Cache.MethodToValue, 0, _Cache.MethodToValue.Length);
            Marshal.Copy(_Cache.MethodFromAddress + 8, _Cache.MethodFromValue, 0, _Cache.MethodFromValue.Length);
            //hook
            Marshal.Copy(_Cache.MethodToValue, 0, _Cache.MethodFromAddress + 8, _Cache.MethodToValue.Length);

        }
    }
}