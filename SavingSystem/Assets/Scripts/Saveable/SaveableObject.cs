using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        public IDictionary<string, SerializableObject> CaptureState ()
        {
            IDictionary<string, SerializableObject> stateDictionary = new Dictionary<string, SerializableObject>();

            foreach (ISaveable<SerializableObject> saveable in GetComponents<ISaveable<SerializableObject>>())
            {
                stateDictionary[saveable.GetType().ToString()] = saveable.CaptureState();
            }

            return stateDictionary;
        }

        public void RestoreState (IDictionary<string, SerializableObject> stateDictionary)
        {
            SaveableDeserializer saveableDeserializer = new SaveableDeserializer();
            
            foreach (ISaveable<SerializableObject> saveable in GetComponents<ISaveable<SerializableObject>>())
            {
                string typeName = saveable.GetType().ToString();

                if (stateDictionary.TryGetValue(typeName, out SerializableObject value))
                {
                    SerializableObject serializable = saveableDeserializer.DeserializeFileToSaveData(value);
                    saveable.RestoreState(serializable);
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