using Newtonsoft.Json;

namespace SavingSystem
{
    public class SaveableDeserializer
    {
        public SaveableDeserializer ()
        {
        }

        public SaveData DeserializeFileToSaveData (object state)
        {
            SaveData saveData;

            try
            {
                saveData = DeserializeAsJson(state);
            }
            catch (JsonReaderException)
            {
                saveData = DeserializeAsBinary(state);
            }

            return saveData;
        }

        private SaveData DeserializeAsJson (object state)
        {
            SaveData saveData = JsonConvert.DeserializeObject<SaveData>(state.ToString());
            return saveData;
        }

        private SaveData DeserializeAsBinary (object state)
        {
            SaveData saveData = (SaveData)state;
            return saveData;
        }
    }
}