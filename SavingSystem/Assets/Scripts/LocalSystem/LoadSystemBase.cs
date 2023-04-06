using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Newtonsoft.Json;

namespace SavingSystem
{
    public class LoadSystemBase
    {
        public string SavePath { get; set; }

        public void RestoreState (Dictionary<string, SerializableObject> state)
        {
            DictionaryDeserializer dictionaryDeserializer = new DictionaryDeserializer();

            foreach (SaveableObject saveable in GlobalSaveableObjectListHolder.GlobalSavingSystemCollection)
            {
                if (state.TryGetValue(saveable.CurrentId, out SerializableObject value))
                {
                    IDictionary<string, SerializableObject> stateDictionary = dictionaryDeserializer.DeserializeFileToDictionary(value);
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
            byte[] saveData = File.ReadAllBytes(SavePath);

            try
            {
                dictionary = DeserializeAsJson(saveData);
            }
            catch (JsonReaderException)
            {
                dictionary = DeserializeAsBinary(saveData);
            }

            return dictionary;
        }

        private Dictionary<string, object> DeserializeAsJson (byte[] saveData)
        {
            string dataString = Encoding.UTF8.GetString(saveData);
            Dictionary<string, object> deserializedObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataString);

            return deserializedObject;
        }

        private Dictionary<string, object> DeserializeAsBinary (byte[] saveData)
        {
            using (MemoryStream stream = new MemoryStream(saveData))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                Dictionary<string, object> deserializedObject = (Dictionary<string, object>)formatter.Deserialize(stream);

                return deserializedObject;
            }
        }
    }
}