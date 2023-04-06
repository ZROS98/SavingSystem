namespace SavingSystem
{
    public abstract class SaveableDeserializerBase
    {
        public abstract SaveData Deserialize (object state);
    }
}