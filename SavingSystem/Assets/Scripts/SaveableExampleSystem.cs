using System;
using UnityEngine;

namespace SavingSystem
{
    public class SaveableExampleSystem : BaseSaveableExampleSystem, ISaveable
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
            SaveData saveData = CurrentSaveableExampleDeserializer.DeserializeFileToSaveData(state);
            
            Name = saveData.name;
            Level = saveData.level;
            Xp = saveData.xp;
        }
    }
}