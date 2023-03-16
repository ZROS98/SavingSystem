using UnityEngine;

namespace SavingSystem
{
    public class SavingLoadingController : MonoBehaviour
    {
        private LoadSystem CurrentLoadSystem {get; set; }
        private SaveSystem CurrentSaveSystem { get; set; }
        
        private string SavePath => $"{Application.persistentDataPath}/save.txt";

        public void SaveBinary ()
        {
            var state = CurrentLoadSystem.LoadFile();
            CurrentSaveSystem.CaptureState(state);
            CurrentSaveSystem.SaveFileAsBinary(state);
        }

        public void SaveJson ()
        {
            var state = CurrentLoadSystem.LoadFile();
            CurrentSaveSystem.CaptureState(state);
            CurrentSaveSystem.SaveFileAsJson(state);
        }

        public void Load ()
        {
            var state = CurrentLoadSystem.LoadFile();
            CurrentLoadSystem.RestoreState(state);
        }
        
        protected virtual void Awake ()
        {
            Initialize();
        }

        private void Initialize ()
        {
            CurrentLoadSystem = new LoadSystem(SavePath);
            CurrentSaveSystem = new SaveSystem(SavePath);
        }
    }
}