using Newtonsoft.Json.Serialization;

namespace DS.Core
{
    public abstract class DataContainerBase
    {
        public abstract string SaveableKey { get; }
        public virtual void InitDefaultData() { }
        public virtual bool HandleSerializationError(ErrorContext errorContext) => false;
        
        public void Save() => DataStorage.SaveContainer(this);
        public void Load() => DataStorage.ResolveContainer(this);
    }
}