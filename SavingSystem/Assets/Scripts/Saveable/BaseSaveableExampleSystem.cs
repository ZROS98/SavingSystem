using SavingSystem;
using UnityEngine;

public class BaseSaveableExampleSystem : MonoBehaviour
{
    protected SaveableDeserializer CurrentSaveableDeserializer { get; set; }
    
    protected virtual void Awake ()
    {
        Initialize();
    }

    private void Initialize ()
    {
        CurrentSaveableDeserializer = new SaveableDeserializer();
    }
}
