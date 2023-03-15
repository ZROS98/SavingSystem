using System;
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
    public class CloudStorageMock : MonoBehaviour
    {
        public Dictionary<string, object> SaveDataDictionary { get; set; }

        public void SaveJsonToCloud (string fileName, object data, string cloudAddress, string apiKey)
        {
            string jsonData = JsonConvert.SerializeObject(data);

            // Send the data to the cloud storage API
            string apiEndpoint = $"{cloudAddress}/save?apiKey={apiKey}&fileName={fileName}";
            // In a real implementation, here would be logic to send the request
            Debug.Log($"Sending JSON data to {apiEndpoint}: {jsonData}");
        }

        public void SaveBinaryToCloud (string fileName, byte[] data, string cloudAddress, string apiKey)
        {
            // Send the binary data to the cloud storage API
            string apiEndpoint = $"{cloudAddress}/save?apiKey={apiKey}&fileName={fileName}";
            // In a real implementation, here would be logic to send the request
            Debug.Log($"Sending binary data to {apiEndpoint}: {data.Length} bytes");
        }

        [ContextMenu("LoadDataFromCloud")]
        public void LoadDataFromCloud (/*string saveCloudAddress*/)
        {
            string saveCloudAddress = "https://drive.google.com/uc?export=download&id=1RonmRzcvwv1eHmb9rNYEnSdNePyUseYM";
            StartCoroutine(LoadSaveFromCloudProcess(saveCloudAddress));
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