namespace SavingSystem
{
    public class SaveableDeserializer
    {
        private readonly SaveableDeserializerBase jsonDeserializer = new JsonSaveableDeserializer();
        private readonly SaveableDeserializerBase binaryDeserializer = new BinarySaveableDeserializer();

        public SaveableDeserializer ()
        {
        }

        public SerializableObject DeserializeFileToSaveData (SerializableObject state)
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