using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace AutoJTTXUtilities.WechatHandling
{
    public class JsonHelper
    {
        public static string GetJson<T>(T obj)
        {
            DataContractJsonSerializer json = new DataContractJsonSerializer(obj.GetType());
            string text;
            using (MemoryStream stream = new MemoryStream())
            {
                json.WriteObject(stream, obj);
                string szJson = Encoding.UTF8.GetString(stream.ToArray());
                text = szJson;
            }
            return text;
        }

        public static T ParseFromJson<T>(string szJson)
        {
            T obj = Activator.CreateInstance<T>();
            T t;
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(szJson)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                t = (T)((object)serializer.ReadObject(ms));
            }
            return t;
        }
    }
}
