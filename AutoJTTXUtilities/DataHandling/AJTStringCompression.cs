using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace AutoJTTXUtilities.DataHandling
{
    public class AJTStringCompression
    {
        public static string Compress(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            string result;
            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                using (MemoryStream memoryStream2 = new MemoryStream())
                {
                    using (GZipStream gzipStream = new GZipStream(memoryStream2, CompressionMode.Compress))
                    {
                        memoryStream.CopyTo(gzipStream);
                    }
                    result = Convert.ToBase64String(memoryStream2.ToArray());
                }
            }
            return result;
        }

        public static string Uncompress(string str)
        {
            byte[] buffer = Convert.FromBase64String(str);
            string @string;
            using (MemoryStream memoryStream = new MemoryStream(buffer))
            {
                using (MemoryStream memoryStream2 = new MemoryStream())
                {
                    using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                    {
                        gzipStream.CopyTo(memoryStream2);
                    }
                    @string = Encoding.UTF8.GetString(memoryStream2.ToArray());
                }
            }
            return @string;
        }
    }
}
