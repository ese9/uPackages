using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NineGames.Storage
{
    public class DataStorage : IDataStorage
    {
        private static readonly Dictionary<Type, DataContainerBase> RegisteredSaveable = new Dictionary<Type, DataContainerBase>();

        private static IDataPersistence dataPersistence;

        T IDataStorage.GetContainer<T>() => GetContainer<T>();

        void IDataStorage.SaveContainer(DataContainerBase dataContainerBase) => SaveContainer(dataContainerBase);

        void IDataStorage.SaveAllContainers() => SaveAllContainers();

        public static void ChangeBehaviour(IDataPersistence persistence) => dataPersistence = persistence;

        public static string TakeSnapshot()
        {
            var containers = dataPersistence.GetContainers();
            return JsonConvert.SerializeObject(containers, Formatting.Indented, ResolveSettings());
        }

        public static void ApplySnapshot(string snapshot)
        {
            var containers = JsonConvert.DeserializeObject<Dictionary<string, string>>(snapshot);
            dataPersistence.SetContainers(containers);
        }

        public static void ClearStorage()
        {
            dataPersistence.ClearContainers();
            RegisteredSaveable.Clear();
        }

        public static T GetContainer<T>() where T : DataContainerBase, new()
        {
            var originalType = typeof(T);

            if (!RegisteredSaveable.ContainsKey(originalType))
                ResolveContainer(new T());

            return (T)RegisteredSaveable[originalType];
        }

        public static void SaveContainer(DataContainerBase dataContainer) =>
            dataPersistence.SaveContainer(dataContainer.SaveableKey, SerializeContainer(dataContainer));

        public static void SaveAllContainers()
        {
            foreach (var data in RegisteredSaveable)
                SaveContainer(data.Value);
        }

        public static void ResolveContainer<T>(T container) where T : DataContainerBase
        {
            if (dataPersistence.ContainerExists(container.SaveableKey))
            {
                var json = dataPersistence.LoadContainer(container.SaveableKey);
                JsonConvert.PopulateObject(json, container, ResolveSettings());
            }
            else
            {
                container.InitDefaultData();
                dataPersistence.CreateContainer(container.SaveableKey, SerializeContainer(container));
            }

            RegisteredSaveable[typeof(T)] = container;
        }

        private static string SerializeContainer(DataContainerBase dataContainer) =>
            JsonConvert.SerializeObject(dataContainer, Formatting.Indented, ResolveSettings());

        private static JsonSerializerSettings ResolveSettings()
        {
            var settings = DataConfig.SerializerSettings;
            settings.Error = HandleSerializationError;
            return settings;
        }

        private static void HandleSerializationError(object sender, ErrorEventArgs errorArgs)
        {
            var container = errorArgs.CurrentObject as DataContainerBase;
            errorArgs.ErrorContext.Handled = container?.HandleSerializationError(errorArgs.ErrorContext) ?? false;
        }
    }
}