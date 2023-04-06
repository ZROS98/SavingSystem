using System;
using System.Collections.Generic;
using UnityEngine;

namespace SavingSystem
{
    public class SaveableObject : MonoBehaviour
    {
        [field: SerializeField]
        private string ObjectId { get; set; } = string.Empty;

        public string CurrentId
        {
            get { return ObjectId; }
            set { ObjectId = value; }
        }

        public Dictionary<string, object> CaptureState ()
        {
            var stateDictionary = new Dictionary<string, object>();

            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                stateDictionary[saveable.GetType().ToString()] = saveable.CaptureState();
            }

            return stateDictionary;
        }

        public void RestoreState (Dictionary<string, object> stateDictionary)
        {
            SaveableDeserializer saveableDeserializer = new SaveableDeserializer();
            
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                string typeName = saveable.GetType().ToString();

                if (stateDictionary.TryGetValue(typeName, out object value))
                {
                    SaveData saveData = saveableDeserializer.DeserializeFileToSaveData(value);
                    saveable.RestoreState(saveData);
                }
            }
        }

        protected virtual void Awake ()
        {
            Initialize();
        }

        private void Initialize ()
        {
            GlobalSaveableObjectListHolder.GlobalSavingSystemCollection.Add(this);
        }

        [ContextMenu("Generate Id")]
        private void GenerateId ()
        {
            ObjectId = Guid.NewGuid().ToString();
        }
    }
}