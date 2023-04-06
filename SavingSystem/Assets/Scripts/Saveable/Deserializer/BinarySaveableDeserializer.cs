namespace SavingSystem
{
    public class BinarySaveableDeserializer : SaveableDeserializerBase
    {
        public override SaveData Deserialize (object state)
        {
            if (state is SaveData saveData)
            {
                return saveData;
            }

            throw new SaveDataDeserializationException("Failed to deserialize SaveData from binary.");
        }
    }
}