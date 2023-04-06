namespace SavingSystem
{
    public interface ISaveable<T>
    {
        public T CaptureState ();
        public void RestoreState (T state);
    }
}