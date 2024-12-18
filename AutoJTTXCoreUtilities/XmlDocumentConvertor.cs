using EMPTYPELIBRARYLib;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;

namespace AutoJTTXCoreUtilities
{
    internal class XmlDocumentConvertor
    {
        public XmlDocumentConvertor(XmlDocument doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException("doc");
            }
            this.doc_ = doc;
        }

        public XmlDocument Document
        {
            get
            {
                return this.doc_;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Document");
                }
                this.doc_ = value;
            }
        }

        public IStream ToStream()
        {
            IStream stream = null;
            XmlDocumentConvertor.CreateStreamOnHGlobal(IntPtr.Zero, 1, ref stream);
            StreamEx streamEx = new StreamEx(stream as UCOMIStream);
            streamEx.Seek(0L, SeekOrigin.Begin);
            this.doc_.Save(streamEx);
            return stream;
        }

        [DllImport("Ole32.dll")]
        private static extern int CreateStreamOnHGlobal(IntPtr hGlobal, int bDeleteOnRelease, ref IStream str);

        private XmlDocument doc_;
    }
}
