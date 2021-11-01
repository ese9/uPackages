using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IdleCivilization.Client.SaveLoadSystem
{
    public abstract class SaveableBase
    {
        public abstract string SaveableKey { get; }
        public virtual void InitDefaultData() { }
        public virtual void SavedDataRestored() { }
        public virtual bool HandleSerializationError(ErrorContext errorContext) => false;
        public void Save() => DataContainer.Save(this);

        public string SerializeData() =>
            JsonConvert.SerializeObject(this, Formatting.Indented, DataContainer.SerializerSettings);
    }
}