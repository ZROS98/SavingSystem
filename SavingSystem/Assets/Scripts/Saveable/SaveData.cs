using System;

namespace SavingSystem
{
    [Serializable]
    public struct SaveData : SerializableObject
    {
        public string name;
        public int level;
        public int xp;
    }
}