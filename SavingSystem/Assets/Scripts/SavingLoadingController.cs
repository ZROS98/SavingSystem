using System.Collections.Generic;
using UnityEngine;

namespace SavingSystem
{
    public class SavingLoadingController : MonoBehaviour
    {
        private SaveLoadSystem CurrentSaveLoadSystem {get; set; }
        
        protected virtual void Awake ()
        {
            Initialize();
        }

        public void SaveBinary ()
        {
            var state = CurrentSaveLoadSystem.LoadFile();
            CaptureState(state);
            CurrentSaveLoadSystem.SaveFileAsBinary(state);
        }

        public void SaveJson ()
        {
            var state = CurrentSaveLoadSystem.LoadFile();
            CaptureState(state);
            CurrentSaveLoadSystem.SaveFileAsJson(state);
        }

        public void Load ()
        {
            var state = CurrentSaveLoadSystem.LoadFile();
            RestoreState(state);
        }

        private void Initialize ()
        {
            CurrentSaveLoadSystem = new SaveLoadSystem();
        }

        private void CaptureState (Dictionary<string, object> state)
        {
            foreach (SaveableObject saveable in GlobalSaveableObjectListHolder.GlobalSavingSystemCollection)
            {
                state[saveable.CurrentId] = saveable.CaptureState();
            }
        }

        private void RestoreState (Dictionary<string, object> state)
        {
            foreach (SaveableObject saveable in GlobalSaveableObjectListHolder.GlobalSavingSystemCollection)
            {
                if (state.TryGetValue(saveable.CurrentId, out object value))
                {
                    saveable.RestoreState(value);
                }
            }
        }
    }
}