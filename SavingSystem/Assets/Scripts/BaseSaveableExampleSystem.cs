using SavingSystem;
using UnityEngine;

public class BaseSaveableExampleSystem : MonoBehaviour
{
    protected SaveableExampleDeserializer CurrentSaveableExampleDeserializer { get; set; }
    
    protected virtual void Awake ()
    {
        Initialize();
    }

    private void Initialize ()
    {
        CurrentSaveableExampleDeserializer = new SaveableExampleDeserializer();
    }
}
