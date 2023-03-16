using System.Collections;
using UnityEngine;

namespace SavingSystem
{
    public class CloudStorageMock : MonoBehaviour
    {
        private SaveLoadToCloudSystem CurrentSaveLoadToCloudSystem { get; set; }
        private SaveLoadSystem CurrentSaveLoadSystem { get; set; }
        
        private const string SAVE_CLOUD_ADDRESS = "https://drive.google.com/uc?export=download&id=1RonmRzcvwv1eHmb9rNYEnSdNePyUseYM";
        
        protected virtual void Awake ()
        {
            Initialize();
        }
        
        public void Load ()
        {
            CurrentSaveLoadToCloudSystem.LoadDataFromCloud(SAVE_CLOUD_ADDRESS);
            StartCoroutine(WaitForDataProcess());
        }
        
        public void SaveBinary ()
        {
            var state = CurrentSaveLoadSystem.LoadFile();
            CurrentSaveLoadToCloudSystem.CaptureState(state);
            string binaryData = CurrentSaveLoadToCloudSystem.SaveFileAsBinary(state);
            CurrentSaveLoadToCloudSystem.SaveBinaryToCloud(binaryData);
        }

        public void SaveJson ()
        {
            var state = CurrentSaveLoadSystem.LoadFile();
            CurrentSaveLoadToCloudSystem.CaptureState(state);
            string jsonData = CurrentSaveLoadToCloudSystem.SaveFileAsJson(state);
            CurrentSaveLoadToCloudSystem.SaveJsonToCloud(jsonData);
        }

        private IEnumerator WaitForDataProcess ()
        {
            yield return new WaitUntil(() => CurrentSaveLoadToCloudSystem.SaveDataDictionary != null);

            CurrentSaveLoadToCloudSystem.RestoreState(CurrentSaveLoadToCloudSystem.SaveDataDictionary);
        }

        private void Initialize ()
        {
            CurrentSaveLoadSystem = new SaveLoadSystem();
            CurrentSaveLoadToCloudSystem = new SaveLoadToCloudSystem(this);
        }
    }
}