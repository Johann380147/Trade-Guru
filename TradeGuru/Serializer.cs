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
        private const string ItemFileName = @"../../../../SavedResults.bin";
        private const string SearchObjFileName = @"../../../../SavedSearches.bin";

        public static void SerializeSearchObjectList(List<SearchObject> lst)
        {
            Stream SaveFileStream = File.Create(SearchObjFileName);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(SaveFileStream, lst);
            SaveFileStream.Close();
        }

        public static void SerializeItemList(List<ItemList> lst)
        {
            Stream SaveFileStream = File.Create(ItemFileName);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(SaveFileStream, lst);
            SaveFileStream.Close();
        }

        public static List<SearchObject> GetSerializedSearchObjectList()
        {
            if (File.Exists(SearchObjFileName))
            {
                Stream openFileStream = File.OpenRead(SearchObjFileName);
                BinaryFormatter deserializer = new BinaryFormatter();
                var lst = (List<SearchObject>)deserializer.Deserialize(openFileStream);
                openFileStream.Close();
                return lst;
            }

            return null;
        }

        public static List<ItemList> GetSerializedItemList()
        {
            if (File.Exists(ItemFileName))
            {
                Stream openFileStream = File.OpenRead(ItemFileName);
                BinaryFormatter deserializer = new BinaryFormatter();
                var lst = (List<ItemList>)deserializer.Deserialize(openFileStream);
                openFileStream.Close();
                return lst;
            }
            
            return null;
        }
    }
}
