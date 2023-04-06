using Newtonsoft.Json;

namespace SavingSystem
{
    public class JsonSaveableDeserializer : SaveableDeserializerBase
    {
        public override SaveData Deserialize (object state)
        {
            try
            {
                return JsonConvert.DeserializeObject<SaveData>(state.ToString());
            }
            catch (JsonReaderException)
            {
                throw new SaveDataDeserializationException("Failed to deserialize SaveData from JSON.");
            }
        }
    }
}