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
            Dictionary<string, object> stateDictionary = null;

            try
            {
                stateDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(state.ToString());
            }
            catch (Exception ex)
            {
                stateDictionary = (Dictionary<string, object>)state;
            }

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