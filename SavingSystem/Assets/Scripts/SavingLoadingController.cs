using System;
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

            string fileContents = File.ReadAllText(SavePath);
            Dictionary<string, object> dictionary = null;

            try
            { 
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(fileContents);
            }
            catch (Exception ex)
            {
                using (FileStream stream = File.Open(SavePath, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    dictionary = (Dictionary<string, object>)formatter.Deserialize(stream);
                }
            }

            return dictionary;
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