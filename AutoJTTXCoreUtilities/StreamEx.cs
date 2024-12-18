using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace AutoJTTXCoreUtilities
{
    internal class StreamEx : Stream
    {
        public StreamEx(UCOMIStream coStream)
        {
            if (coStream == null)
            {
                throw new ArgumentNullException("coStream");
            }
            this.m_pOrigStream = coStream;
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return base.BeginRead(buffer, offset, count, callback, state);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return base.BeginWrite(buffer, offset, count, callback, state);
        }

        public override void Close()
        {
            if (this.m_pOrigStream != null)
            {
                this.m_pOrigStream.Commit(0);
                Marshal.ReleaseComObject(this.m_pOrigStream);
                this.m_pOrigStream = null;
                GC.SuppressFinalize(this);
            }
        }

        protected override WaitHandle CreateWaitHandle()
        {
            return base.CreateWaitHandle();
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            return base.EndRead(asyncResult);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            base.EndWrite(asyncResult);
        }

        public override void Flush()
        {
            if (this.m_pOrigStream == null)
            {
                throw new ObjectDisposedException("m_pOrigStream");
            }
            this.m_pOrigStream.Commit(0);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this.m_pOrigStream == null)
            {
                throw new ObjectDisposedException("m_pOrigStream");
            }
            IntPtr zero = IntPtr.Zero;
            if (offset != 0)
            {
                byte[] array = new byte[count];
                this.m_pOrigStream.Read(array, count, zero);
                Array destinationArray = Array.CreateInstance(typeof(byte), count);
                Array array2 = Array.CreateInstance(typeof(byte), count);
                array.CopyTo(array2, count);
                Array.Copy(array2, 0, destinationArray, offset, zero.ToInt32());
            }
            else
            {
                this.m_pOrigStream.Read(buffer, count, zero);
            }
            return zero.ToInt32();
        }

        public override int ReadByte()
        {
            return base.ReadByte();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (this.m_pOrigStream == null)
            {
                throw new ObjectDisposedException("m_pOrigStream");
            }
            IntPtr zero = IntPtr.Zero;
            this.m_pOrigStream.Seek(offset, (int)origin, zero);
            return (long)zero.ToInt32();
        }

        public override void SetLength(long value)
        {
            if (this.m_pOrigStream == null)
            {
                throw new ObjectDisposedException("m_pOrigStream");
            }
            this.m_pOrigStream.SetSize(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (this.m_pOrigStream == null)
            {
                throw new ObjectDisposedException("m_pOrigStream");
            }
            IntPtr zero = IntPtr.Zero;
            if (offset != 0)
            {
                int num = buffer.Length - offset;
                byte[] array = new byte[num];
                Array.Copy(buffer, offset, array, 0, num);
                this.m_pOrigStream.Write(array, num, zero);
                return;
            }
            this.m_pOrigStream.Write(buffer, count, zero);
        }

        public override void WriteByte(byte b)
        {
            IntPtr zero = IntPtr.Zero;
            byte[] pv = new byte[]
            {
                b
            };
            this.m_pOrigStream.Write(pv, 1, zero);
        }

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return true;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override long Length
        {
            get
            {
                if (this.m_pOrigStream == null)
                {
                    throw new ObjectDisposedException("m_pOrigStream");
                }
                STATSTG statstg;
                this.m_pOrigStream.Stat(out statstg, 1);
                return statstg.cbSize;
            }
        }

        public override long Position
        {
            get
            {
                return this.Seek(0L, SeekOrigin.Current);
            }
            set
            {
                this.Seek(value, SeekOrigin.Begin);
            }
        }

        private UCOMIStream m_pOrigStream;
    }

}
