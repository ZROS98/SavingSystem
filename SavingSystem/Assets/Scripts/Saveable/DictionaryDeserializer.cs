using System.Collections.Generic;
using Newtonsoft.Json;

namespace SavingSystem
{
    public class DictionaryDeserializer
    {
        public IDictionary<string, SerializableObject> DeserializeFileToDictionary (object state)
        {
            try
            {
                return DeserializeAsJson(state);
            }
            catch (JsonReaderException)
            {
                return DeserializeAsBinary(state);
            }
        }

        private IDictionary<string, SerializableObject> DeserializeAsJson (object state)
        {
            var stateDictionary = JsonConvert.DeserializeObject<Dictionary<string, SerializableObject>>(state.ToString());
            return stateDictionary;
        }

        private IDictionary<string, SerializableObject> DeserializeAsBinary (object state)
        {
            IDictionary<string, SerializableObject> stateDictionary = (Dictionary<string, SerializableObject>)state;
            return stateDictionary;
        }
    }
}