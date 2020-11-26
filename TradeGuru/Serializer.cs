using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TradeGuru
{
    public static class Serializer
    {
        const string FileName = @"../../../SavedResults.bin";

        public static void SerializeItemList(List<ItemList> lst)
        {
            Stream SaveFileStream = File.Create(FileName);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(SaveFileStream, lst);
            SaveFileStream.Close();
        }

        public static List<ItemList> GetSerializedItemList()
        {
            if (File.Exists(FileName))
            {
                Stream openFileStream = File.OpenRead(FileName);
                BinaryFormatter deserializer = new BinaryFormatter();
                var lst = (List<ItemList>)deserializer.Deserialize(openFileStream);
                openFileStream.Close();
                return lst;
            }
            
            return null;
        }
    }
}
