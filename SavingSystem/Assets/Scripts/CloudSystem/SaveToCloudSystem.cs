using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace SavingSystem
{
    public class SaveToCloudSystem
    {
        public SaveToCloudSystem ()
        {
        }

        public void CaptureState (Dictionary<string, object> state)
        {
            foreach (SaveableObject saveable in GlobalSaveableObjectListHolder.GlobalSavingSystemCollection)
            {
                state[saveable.CurrentId] = saveable.CaptureState();
            }
        }

        public string SaveFileAsBinary (object state)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
                byte[] bytes = stream.ToArray();
                string binaryString = Encoding.UTF8.GetString(bytes);

                return binaryString;
            }
        }

        public string SaveFileAsJson (object state)
        {
            string json = JsonConvert.SerializeObject(state);
            return json;
        }

        private void SaveJsonToCloud (string jsonData)
        {
            Debug.Log("Json data has been uploaded to cloud");
        }

        private void SaveBinaryToCloud (string binaryData)
        {
            Debug.Log("Binary data has been uploaded to cloud");
        }
    }
}