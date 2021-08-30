using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace TradeGuru
{
    public static class Serializer
    {
        public static void SerializeObject<T>(string fileName, T lst)
        {
            Stream fileStream = File.Create(fileName);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(fileStream, lst);
            fileStream.Close();
        }

        public static T DeserializeObject<T>(string fileName)
        {
            if (File.Exists(fileName))
            {
                Stream openFileStream = File.OpenRead(fileName);
                if (openFileStream.Length != 0)
                {
                    BinaryFormatter deserializer = new BinaryFormatter();
                    var obj = (T)deserializer.Deserialize(openFileStream);
                    openFileStream.Close();
                    return obj;
                }
            }

            return default(T);
        }

        public static void SerializeObjectList<T>(string fileName, List<T> lst)
        {
            Stream fileStream = File.Create(fileName);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(fileStream, lst);
            fileStream.Close();
        }

        public static List<T> DeserializeObjectList<T>(string fileName)
        {
            if (File.Exists(fileName))
            {
                Stream openFileStream = File.OpenRead(fileName);
                if (openFileStream.Length != 0)
                {
                    BinaryFormatter deserializer = new BinaryFormatter();
                    var lst = (List<T>)deserializer.Deserialize(openFileStream);
                    openFileStream.Close();
                    return lst;
                }
            }

            return null;
        }

        public static byte[] Zip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    msi.CopyTo(gs);
                }

                return mso.ToArray();
            }
        }

        public static string Unzip(byte[] bytes)
        {
            if (bytes == null) return String.Empty;

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    gs.CopyTo(mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }
    }
}
