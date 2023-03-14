using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using UnityEngine;

namespace SavingSystem
{
    public class SavingLoadingController : MonoBehaviour
    {
        private string SavePath
        {
            get { return $"{Application.persistentDataPath}/save.txt"; }
        }

        public void SaveBinary ()
        {
            var state = LoadFile();
            CaptureState(state);
            SaveFileAsBinary(state);
        }

        public void SaveJson ()
        {
            var state = LoadFile();
            CaptureState(state);
            SaveFileAsJson(state);
        }

        public void Load ()
        {
            var state = LoadFile();
            RestoreState(state);
        }

        private Dictionary<string, object> LoadFile ()
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

        private void SaveFileAsBinary (object state)
        {
            using (FileStream stream = File.Open(SavePath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        private void SaveFileAsJson (object state)
        {
            using (StreamWriter writer = new StreamWriter(SavePath))
            {
                string json = JsonConvert.SerializeObject(state);
                writer.Write(json);
            }
        }

        private void CaptureState (Dictionary<string, object> state)
        {
            foreach (SaveableObject saveable in FindObjectsOfType<SaveableObject>())
            {
                state[saveable.CurrentId] = saveable.CaptureState();
            }
        }

        private void RestoreState (Dictionary<string, object> state)
        {
            foreach (SaveableObject saveable in FindObjectsOfType<SaveableObject>())
            {
                if (state.TryGetValue(saveable.CurrentId, out object value))
                {
                    saveable.RestoreState(value);
                }
            }
        }
    }
}