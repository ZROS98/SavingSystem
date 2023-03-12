using System;
using UnityEngine;

namespace SavingSystem
{
    public class SevableExampleSystem : MonoBehaviour, ISaveable
    {
        [field: SerializeField]
        private string Name { get; set; } = String.Empty;
        [field: SerializeField]
        private int Level { get; set; } = 1;
        [field: SerializeField]
        private int Xp { get; set; } = 0;
        [field: SerializeField]
        private Vector3 Position { get; set; } = Vector3.zero; 
        [field: SerializeField]
        private Quaternion Rotation { get; set; } = Quaternion.identity;

        public object CaptureState ()
        {
            return new SaveData
            {
                name = Name,
                level = Level,
                xp = Xp,
                position = Position,
                rotation = Rotation
            };
        }

        public void RestoreState (object state)
        {
            SaveData saveData = (SaveData)state;

            Name = saveData.name;
            Level = saveData.level;
            Xp = saveData.xp;
            Position = saveData.position;
            Rotation = saveData.rotation;
        }

        [Serializable]
        private struct SaveData
        {
            public string name;
            public int level;
            public int xp;
            public Vector3 position;
            public Quaternion rotation;
        }
    }
}