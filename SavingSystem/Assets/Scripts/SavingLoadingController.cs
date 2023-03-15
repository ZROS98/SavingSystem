using UnityEngine;

namespace SavingSystem
{
    public class SavingLoadingController : MonoBehaviour
    {
        private SaveLoadSystem CurrentSaveLoadSystem {get; set; }
        
        public void SaveBinary ()
        {
            var state = CurrentSaveLoadSystem.LoadFile();
            CurrentSaveLoadSystem.CaptureState(state);
            CurrentSaveLoadSystem.SaveFileAsBinary(state);
        }

        public void SaveJson ()
        {
            var state = CurrentSaveLoadSystem.LoadFile();
            CurrentSaveLoadSystem.CaptureState(state);
            CurrentSaveLoadSystem.SaveFileAsJson(state);
        }

        public void Load ()
        {
            var state = CurrentSaveLoadSystem.LoadFile();
            CurrentSaveLoadSystem.RestoreState(state);
        }
        
        protected virtual void Awake ()
        {
            Initialize();
        }

        private void Initialize ()
        {
            CurrentSaveLoadSystem = new SaveLoadSystem();
        }
    }
}