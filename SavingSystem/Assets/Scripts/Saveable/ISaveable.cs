namespace SavingSystem
{
    public interface ISaveable
    {
        public SaveData CaptureState ();
        public void RestoreState (SaveData state);
    }
}