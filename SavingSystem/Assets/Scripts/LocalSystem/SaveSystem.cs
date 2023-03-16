using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

namespace SavingSystem
{
    public class SaveSystem
    {
        private string SavePath { get; set; }

        public SaveSystem (string savePath)
        {
            SavePath = savePath;
        }

        public void SaveFileAsBinary (object state)
        {
            using (FileStream stream = File.Open(SavePath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        public void SaveFileAsJson (object state)
        {
            using (StreamWriter writer = new StreamWriter(SavePath))
            {
                string json = JsonConvert.SerializeObject(state);
                writer.Write(json);
            }
        }

        public void CaptureState (Dictionary<string, object> state)
        {
            foreach (SaveableObject saveable in GlobalSaveableObjectListHolder.GlobalSavingSystemCollection)
            {
                state[saveable.CurrentId] = saveable.CaptureState();
            }
        }
    }
}