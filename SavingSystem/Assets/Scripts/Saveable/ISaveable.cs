using System.Collections.Generic;

namespace SavingSystem
{
    public interface ISaveable
    {
        public SaveData CaptureState ();
        public void RestoreState (object state);
    }
}