using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IdleCivilization.Client.SaveLoadSystem
{
    public class DataContainer : IDataContainer
    {
        public static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            NullValueHandling = NullValueHandling.Ignore,
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            Error = HandleSerializationError
        };

        private static readonly Dictionary<Type, SaveableBase> RegisteredSaveable = new Dictionary<Type, SaveableBase>();
        private static IDataPersistenceBehaviour dataPersistenceBehaviour = DataPersistenceIOBehaviour.Default;

        T IDataContainer.GetData<T>() => GetData<T>();
        void IDataContainer.SaveData(SaveableBase saveableBase) => Save(saveableBase);

        public static void ChangeBehaviour(IDataPersistenceBehaviour dataPersistenceBehaviour) =>
            DataContainer.dataPersistenceBehaviour = dataPersistenceBehaviour;

        public static T GetData<T>() where T : SaveableBase, new()
        {
            var originalType = typeof(T);
            if (!RegisteredSaveable.ContainsKey(originalType))
            {
                RegisterSaveable(new T());
            }

            return (T) RegisteredSaveable[originalType];
        }

        public static void SaveAll()
        {
            foreach (var data in RegisteredSaveable)
            {
                Save(data.Value);
            }
        }

        public static void Save(SaveableBase saveableBase) =>
            dataPersistenceBehaviour.SaveContainer(saveableBase.SaveableKey, saveableBase.SerializeData());

        public static string TakeSnapshot()
        {
            var containers = dataPersistenceBehaviour.GetContainers();
            return JsonConvert.SerializeObject(containers, Formatting.Indented, SerializerSettings);
        }

        public static void ApplySnapshot(string snapshot)
        {
            var containers = JsonConvert.DeserializeObject<Dictionary<string, string>>(snapshot);
            dataPersistenceBehaviour.SetContainers(containers);
        }

        public static void Clear()
        {
            dataPersistenceBehaviour.ClearContainers();
            RegisteredSaveable.Clear();
        }

        private static void RegisterSaveable<T>(T saveable) where T : SaveableBase
        {
            var originalType = typeof(T);
            RegisteredSaveable.Add(originalType, saveable);
            Load(originalType);
        }

        private static void Load(Type type)
        {
            var saveableBase = RegisteredSaveable[type];
            if (dataPersistenceBehaviour.ContainerExists(saveableBase.SaveableKey))
            {
                RegisteredSaveable[type] = LoadSaveContainer(saveableBase);
            }
            else
            {
                CreateSaveContainer(saveableBase);
            }
        }

        private static SaveableBase LoadSaveContainer(SaveableBase container)
        {
            container.InitDefaultData();
            var json = dataPersistenceBehaviour.LoadContainer(container.SaveableKey);
            JsonConvert.PopulateObject(json, container, SerializerSettings);
            container.SavedDataRestored();
            return container;
        }

        private static void CreateSaveContainer(SaveableBase container)
        {
            container.InitDefaultData();
            dataPersistenceBehaviour.CreateContainer(container.SaveableKey, container.SerializeData());
            container.SavedDataRestored();
        }

        private static void HandleSerializationError(object sender, ErrorEventArgs errorArgs)
        {
            var container = errorArgs.CurrentObject as SaveableBase;
            errorArgs.ErrorContext.Handled = container?.HandleSerializationError(errorArgs.ErrorContext) ?? false;
        }
    }
}