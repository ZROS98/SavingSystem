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
    public class LoadFromCloudSystem
    {
        public Dictionary<string, object> SaveDataDictionary { get; set; }

        private MonoBehaviour CoroutineController { get; set; }

        public LoadFromCloudSystem (MonoBehaviour coroutineController)
        {
            CoroutineController = coroutineController;
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