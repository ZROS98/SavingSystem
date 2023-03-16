using System.Collections;
using UnityEngine;

namespace SavingSystem
{
    public class CloudStorageMock : MonoBehaviour
    {
        private SaveToCloudSystem CurrentSaveToCloudSystem { get; set; }
        private LoadFromCloudSystem CurrentLoadFromCloudSystem { get; set; }
        private SaveLoadSystem CurrentSaveLoadSystem { get; set; }
        
        private const string SAVE_CLOUD_ADDRESS = "https://drive.google.com/uc?export=download&id=1RonmRzcvwv1eHmb9rNYEnSdNePyUseYM";
        
        protected virtual void Awake ()
        {
            Initialize();
        }
        
        public void Load ()
        {
            CurrentLoadFromCloudSystem.LoadDataFromCloud(SAVE_CLOUD_ADDRESS);
            StartCoroutine(WaitForDataProcess());
        }
        
        public void SaveBinary ()
        {
            var state = CurrentSaveLoadSystem.LoadFile();
            CurrentSaveToCloudSystem.CaptureState(state);
            string binaryData = CurrentSaveToCloudSystem.SaveFileAsBinary(state);
            CurrentSaveToCloudSystem.SaveBinaryToCloud(binaryData);
        }

        public void SaveJson ()
        {
            var state = CurrentSaveLoadSystem.LoadFile();
            CurrentSaveToCloudSystem.CaptureState(state);
            string jsonData = CurrentSaveToCloudSystem.SaveFileAsJson(state);
            CurrentSaveToCloudSystem.SaveJsonToCloud(jsonData);
        }

        private IEnumerator WaitForDataProcess ()
        {
            yield return new WaitUntil(() => CurrentLoadFromCloudSystem.SaveDataDictionary != null);

            CurrentLoadFromCloudSystem.RestoreState(CurrentLoadFromCloudSystem.SaveDataDictionary);
        }

        private void Initialize ()
        {
            CurrentSaveToCloudSystem = new SaveToCloudSystem();
            CurrentLoadFromCloudSystem = new LoadFromCloudSystem(this);
            CurrentSaveLoadSystem = new SaveLoadSystem();
        }
    }
}