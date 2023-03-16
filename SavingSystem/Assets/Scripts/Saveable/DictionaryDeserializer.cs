using System.Collections.Generic;
using Newtonsoft.Json;

namespace SavingSystem
{
    public class DictionaryDeserializer
    {
        public DictionaryDeserializer ()
        {
        }

        public Dictionary<string, object> DeserializeFileToDictionary (object state)
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
    }
}