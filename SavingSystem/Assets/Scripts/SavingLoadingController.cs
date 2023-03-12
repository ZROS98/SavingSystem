using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SavingSystem
{
    public class SavingLoadingController : MonoBehaviour
    {
        private string SavePath => $"{Application.persistentDataPath}/save.txt";
        
        public void Save ()
        {
            var state = LoadFile();
            CaptureState(state);
            SaveFile(state);
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

            using (FileStream stream = File.Open(SavePath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

        private void SaveFile (object state)
        {
            using (FileStream stream = File.Open(SavePath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
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