using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using Unity.VisualScripting;

namespace SavingSystem
{
    public class LoadSystem
    {
        private string SavePath { get; set; }

        public LoadSystem (string savePath)
        {
            SavePath = savePath;
        }

        public void RestoreState (Dictionary<string, SaveableObject> state)
        {
            DictionaryDeserializer dictionaryDeserializer = new DictionaryDeserializer();
            
            foreach (SaveableObject saveable in GlobalSaveableObjectListHolder.GlobalSavingSystemCollection)
            {
                if (state.TryGetValue(saveable.CurrentId, out SaveableObject value))
                {
                    IDictionary<string, SerializableObject> stateDictionary = dictionaryDeserializer.DeserializeFileToDictionary(value);
                    saveable.RestoreState(stateDictionary);
                }
            }
        }

        public IDictionary<string, object> LoadFile ()
        {
            if (File.Exists(SavePath) == false)
            {
                return new Dictionary<string, object>();
            }

            return DeserializeFileToDictionary();
        }

        private IDictionary<string, object> DeserializeFileToDictionary ()
        {
            try
            {
                return DeserializeAsJson();
            }
            catch (JsonReaderException)
            {
                return DeserializeAsBinary();
            }

        }

        private IDictionary<string, object> DeserializeAsJson ()
        {
            byte[] saveData;
            saveData = File.ReadAllBytes(SavePath);
            string x = saveData.ToString();

            string fileContents = File.ReadAllText(SavePath);
            IDictionary<string, object> deserializedObject = (Dictionary<string, object>)new Dictionary<string, object>(JsonConvert.DeserializeObject<Dictionary<string, object>>(fileContents)).AsReadOnlyCollection();

            return deserializedObject;
        }

        private IDictionary<string, object> DeserializeAsBinary ()
        {
            byte[] saveData = File.ReadAllBytes(SavePath);

            using (MemoryStream stream = new MemoryStream(saveData))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                IDictionary<string, object> deserializedObject = (Dictionary<string, object>)formatter.Deserialize(stream);

                return deserializedObject;
            }
            
            
            
            // using (FileStream stream = File.Open(SavePath, FileMode.Open))
            // {
            //     BinaryFormatter formatter = new BinaryFormatter();
            //     var deserializedObject = (Dictionary<string, object>)formatter.Deserialize(stream);
            //
            //     return deserializedObject;
            // }
        }
    }
}