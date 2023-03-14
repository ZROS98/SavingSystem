using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using UnityEngine;

namespace SavingSystem
{
    public class SaveLoadSystem
    {
        private string SavePath
        {
            get { return $"{Application.persistentDataPath}/save.txt"; }
        }
        
        public SaveLoadSystem ()
        {
        }
        
        public void SaveFileAsBinary (object state)
        {
            using (FileStream stream = File.Open(SavePath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        public void SaveFileAsJson (object state)
        {
            using (StreamWriter writer = new StreamWriter(SavePath))
            {
                string json = JsonConvert.SerializeObject(state);
                writer.Write(json);
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