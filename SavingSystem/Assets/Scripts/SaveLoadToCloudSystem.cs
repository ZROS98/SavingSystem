using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace SavingSystem
{
    public class SaveLoadToCloudSystem
    {
        public Dictionary<string, object> SaveDataDictionary { get; set; }
        
        private MonoBehaviour CoroutineController { get; set; }

        public SaveLoadToCloudSystem (MonoBehaviour coroutineController)
        {
            CoroutineController = coroutineController;
        }
        
        public void RestoreState (Dictionary<string, object> state)
        {
            foreach (SaveableObject saveable in GlobalSaveableObjectListHolder.GlobalSavingSystemCollection)
            {
                if (state.TryGetValue(saveable.CurrentId, out object value))
                {
                    saveable.RestoreState(value);
                }
            }
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
        
        public void SaveJsonToCloud (string jsonData)
        {
            Debug.Log("Json data has been uploaded to cloud");
        }

        public void SaveBinaryToCloud (string binaryData)
        {
            Debug.Log("Binary data has been uploaded to cloud");
        }

        public void LoadDataFromCloud (string saveCloudAddress)
        {
             CoroutineController.StartCoroutine(LoadSaveFromCloudProcess(saveCloudAddress));
        }

        private IEnumerator LoadSaveFromCloudProcess (string saveCloudAddress)
        {
            using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(saveCloudAddress))
            {
                yield return unityWebRequest.SendWebRequest();

                if (unityWebRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(unityWebRequest.error);
                }
                else
                {
                    byte[] saveData = unityWebRequest.downloadHandler.data;
                    SaveDataDictionary = DeserializeDataToDictionary(saveData);
                }
            }
        }

        private Dictionary<string, object> DeserializeDataToDictionary (byte[] saveData)
        {
            Dictionary<string, object> dictionary;

            try
            {
                dictionary = DeserializeJsonData(saveData);
            }
            catch (JsonReaderException)
            {
                dictionary = DeserializeBinaryData(saveData);
            }

            return dictionary;
        }

        private Dictionary<string, object> DeserializeJsonData (byte[] saveData)
        {
            string dataString = Encoding.UTF8.GetString(saveData);
            var deserializedObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataString);

            return deserializedObject;
        }

        private Dictionary<string, object> DeserializeBinaryData (byte[] saveData)
        {
            using (MemoryStream stream = new MemoryStream(saveData))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                var deserializedObject = (Dictionary<string, object>)formatter.Deserialize(stream);

                return deserializedObject;
            }
        }
    }
}