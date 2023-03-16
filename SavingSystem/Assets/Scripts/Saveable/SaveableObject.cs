using System;
using System.Collections.Generic;
using UnityEngine;

namespace SavingSystem
{
    public class SaveableObject : MonoBehaviour
    {
        [field: SerializeField]
        private string ObjectId { get; set; } = string.Empty;

        private DictionaryDeserializer CurrentDictionaryDeserializer { get; set; }

        public string CurrentId
        {
            get { return ObjectId; }
            set { ObjectId = value; }
        }

        public object CaptureState ()
        {
            var stateDictionary = new Dictionary<string, object>();

            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                stateDictionary[saveable.GetType().ToString()] = saveable.CaptureState();
            }

            return stateDictionary;
        }

        public void RestoreState (object state)
        {
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                var stateDictionary = CurrentDictionaryDeserializer.DeserializeFileToDictionary(state);
                string typeName = saveable.GetType().ToString();

                if (stateDictionary.TryGetValue(typeName, out object value))
                {
                    saveable.RestoreState(value);
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
            CurrentDictionaryDeserializer = new DictionaryDeserializer();
        }

        [ContextMenu("Generate Id")]
        private void GenerateId ()
        {
            ObjectId = Guid.NewGuid().ToString();
        }
    }
}