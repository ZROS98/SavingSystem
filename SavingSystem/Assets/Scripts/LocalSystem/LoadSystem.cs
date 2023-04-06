using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

namespace SavingSystem
{
    public class LoadSystem
    {
        private string SavePath { get; set; }

        public LoadSystem (string savePath)
        {
            SavePath = savePath;
        }

        public void RestoreState (Dictionary<string, object> state)
        {
            DictionaryDeserializer dictionaryDeserializer = new DictionaryDeserializer();
            
            foreach (SaveableObject saveable in GlobalSaveableObjectListHolder.GlobalSavingSystemCollection)
            {
                if (state.TryGetValue(saveable.CurrentId, out object value))
                {
                    Dictionary<string, object> stateDictionary = dictionaryDeserializer.DeserializeFileToDictionary(value);
                    saveable.RestoreState(stateDictionary);
                }
            }
        }

        public Dictionary<string, object> LoadFile ()
        {
            if (File.Exists(SavePath) == false)
            {
                return new Dictionary<string, object>();
            }

            return DeserializeFileToDictionary();
        }

        private Dictionary<string, object> DeserializeFileToDictionary ()
        {
            Dictionary<string, object> dictionary;

            try
            {
                dictionary = DeserializeAsJson();
            }
            catch (JsonReaderException)
            {
                dictionary = DeserializeAsBinary();
            }

            return dictionary;
        }

        private Dictionary<string, object> DeserializeAsJson ()
        {
            string fileContents = File.ReadAllText(SavePath);
            var deserializedObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(fileContents);

            return deserializedObject;
        }

        private Dictionary<string, object> DeserializeAsBinary ()
        {
            using (FileStream stream = File.Open(SavePath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                var deserializedObject = (Dictionary<string, object>)formatter.Deserialize(stream);

                return deserializedObject;
            }
        }
    }
}