using System;

namespace SavingSystem
{
    public class SaveDataDeserializationException : Exception
    {
        public SaveDataDeserializationException (string message) : base(message)
        {
        }
    }
}