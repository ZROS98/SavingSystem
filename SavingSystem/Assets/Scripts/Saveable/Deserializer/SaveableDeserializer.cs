namespace SavingSystem
{
    public class SaveableDeserializer
    {
        private readonly SaveableDeserializerBase jsonDeserializer = new JsonSaveableDeserializer();
        private readonly SaveableDeserializerBase binaryDeserializer = new BinarySaveableDeserializer();

        public SaveableDeserializer ()
        {
        }

        public SaveData DeserializeFileToSaveData (object state)
        {
            try
            {
                return jsonDeserializer.Deserialize(state);
            }
            catch (SaveDataDeserializationException)
            {
                return binaryDeserializer.Deserialize(state);
            }
        }
    }
}