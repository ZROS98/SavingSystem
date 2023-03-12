using System;
using System.Collections.Generic;
using UnityEngine;

namespace SavingSystem
{
    public class SaveableObject : MonoBehaviour, ISaveable
    {
        [field: SerializeField]
        private string ObjectId { get; set; } = string.Empty;

        public string CurrentId
        {
            get
            {
                return ObjectId;
            }
            set
            {
                ObjectId = value;
            }
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
            var stateDictionary = new Dictionary<string, object>();

            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                string typeName = saveable.GetType().ToString();

                if (stateDictionary.TryGetValue(typeName, out object value))
                {
                    saveable.RestoreState(value);
                }
            }
        }
        
        [ContextMenu("Generate Id")]
        private void GenerateId ()
        {
            ObjectId = Guid.NewGuid().ToString();
        }
    }
}