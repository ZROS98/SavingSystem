using Newtonsoft.Json;
using Unity.VisualScripting;

namespace SavingSystem
{
    public class JsonSaveableDeserializer : SaveableDeserializerBase
    {
        public override SerializableObject Deserialize (SerializableObject state)
        {
            try
            {
                return JsonConvert.DeserializeObject<SerializableObject>(state.ToString());
            }
            catch (JsonReaderException)
            {
                throw new SaveDataDeserializationException("Failed to deserialize SaveData from JSON.");
            }
        }
    }
}