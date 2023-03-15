using System.Collections.Generic;

namespace SavingSystem
{
    public static class GlobalSaveableObjectListHolder
    {
        public static List<SaveableObject> GlobalSavingSystemCollection { get; set; } = new List<SaveableObject>();
    }
}