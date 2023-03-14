using System;
using Newtonsoft.Json;
using UnityEngine;

namespace SavingSystem
{
    public class SaveableExampleSystem : MonoBehaviour, ISaveable
    {
        [field: SerializeField]
        private string Name { get; set; } = String.Empty;
        [field: SerializeField]
        private int Level { get; set; } = 1;
        [field: SerializeField]
        private int Xp { get; set; } = 0;

        public object CaptureState ()
        {
            return new SaveData
            {
                name = Name,
                level = Level,
                xp = Xp,
            };
        }

        public void RestoreState (object state)
        {
            SaveData saveData = DeserializeFileToSaveData(state);
            
            Name = saveData.name;
            Level = saveData.level;
            Xp = saveData.xp;
        }

        private SaveData DeserializeFileToSaveData (object state)
        {
            SaveData saveData;

            try
            {
                saveData = DeserializeAsJson(state);
            }
            catch (JsonReaderException)
            {
                saveData = DeserializeAsBinary(state);
            }

            return saveData;
        }

        private SaveData DeserializeAsJson (object state)
        {
            SaveData saveData = JsonConvert.DeserializeObject<SaveData>(state.ToString());
            return saveData;
        }

        private SaveData DeserializeAsBinary (object state)
        {
            SaveData saveData = (SaveData)state;
            return saveData;
        }
    }
}