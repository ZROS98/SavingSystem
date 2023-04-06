namespace SavingSystem
{
    public abstract class SaveableDeserializerBase
    {
        public abstract SerializableObject Deserialize (SerializableObject state);
    }
}