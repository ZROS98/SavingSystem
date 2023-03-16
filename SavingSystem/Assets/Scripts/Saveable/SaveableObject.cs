using System;
using System.Collections.Generic;
using Newtonsoft.Json;
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
                var stateDictionary = DeserializeFileToDictionary(state);
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
        }

        private Dictionary<string, object> DeserializeFileToDictionary (object state)
        {
            Dictionary<string, object> stateDictionary;

            try
            {
                stateDictionary = DeserializeAsJson(state);
            }
            catch (JsonReaderException)
            {
                stateDictionary = DeserializeAsBinary(state);
            }

            return stateDictionary;
        }

        private Dictionary<string, object> DeserializeAsJson (object state)
        {
           var stateDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(state.ToString());
           return stateDictionary;
        }

        private Dictionary<string, object> DeserializeAsBinary (object state)
        {
           var stateDictionary = (Dictionary<string, object>)state;
           return stateDictionary;
        }

        [ContextMenu("Generate Id")]
        private void GenerateId ()
        {
            ObjectId = Guid.NewGuid().ToString();
        }
    }
}