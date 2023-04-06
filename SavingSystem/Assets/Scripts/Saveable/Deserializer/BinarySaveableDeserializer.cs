namespace SavingSystem
{
    public class BinarySaveableDeserializer : SaveableDeserializerBase
    {
        public override SerializableObject Deserialize (SerializableObject state)
        {
            return state;
        }
    }
}