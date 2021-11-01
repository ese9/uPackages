using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IdleCivilization.Client.SaveLoadSystem
{
    public class DataPersistencePrefsBehaviour : IDataPersistenceBehaviour
    {
        private const string SaveablePrefsKey = "saveable_key";

        public static readonly DataPersistencePrefsBehaviour Default = new DataPersistencePrefsBehaviour();

        public bool ContainerExists(string key) => PlayerPrefs.HasKey(key);

        public DataPersistencePrefsBehaviour() => TryAddSaveableKey();

        public void CreateContainer(string key, string text)
        {
            var wrapper = GetContainerWrapper();
            if (wrapper.Add(key))
            {
                PlayerPrefs.SetString(SaveablePrefsKey, wrapper.Serialize());
                PlayerPrefs.SetString(key, text);
                PlayerPrefs.Save();
            }
        }

        public void SaveContainer(string key, string text)
        {
            PlayerPrefs.SetString(key, text);
            PlayerPrefs.Save();
        }

        public string LoadContainer(string key) => PlayerPrefs.GetString(key);

        public Dictionary<string, string> GetContainers()
        {
            var wrapper = GetContainerWrapper();
            return wrapper.SaveableKeys.ToDictionary(key => key, PlayerPrefs.GetString);
        }

        public void SetContainers(Dictionary<string, string> containers)
        {
            ClearContainers();
            TryAddSaveableKey();
            
            foreach (var container in containers) 
                CreateContainer(container.Key, container.Value);
        }

        public void ClearContainers()
        {
            var wrapper = GetContainerWrapper();

            foreach (var key in wrapper.SaveableKeys)
                PlayerPrefs.DeleteKey(key);

            PlayerPrefs.DeleteKey(SaveablePrefsKey);
            PlayerPrefs.Save();
        }

        private static void TryAddSaveableKey()
        {
            if (!PlayerPrefs.HasKey(SaveablePrefsKey))
            {
                PlayerPrefs.SetString(SaveablePrefsKey, new SaveableKeysWrapper().Serialize());
                PlayerPrefs.Save();
            }
        }

        private static SaveableKeysWrapper GetContainerWrapper() =>
            SaveableKeysWrapper.FromJson(PlayerPrefs.GetString(SaveablePrefsKey));

        [Serializable]
        private class SaveableKeysWrapper
        {
            [SerializeField] private List<string> saveableKeys = new List<string>();

            public IReadOnlyCollection<string> SaveableKeys => saveableKeys;

            public bool HasKey(string key) => saveableKeys.Contains(key);

            public bool Add(string key)
            {
                if (!HasKey(key))
                {
                    saveableKeys.Add(key);
                    return true;
                }

                return false;
            }

            public bool Remove(string key) => saveableKeys.Remove(key);

            public string Serialize() => ToJson(this);
            public static string ToJson(SaveableKeysWrapper wrapper) => JsonUtility.ToJson(wrapper);
            public static SaveableKeysWrapper FromJson(string json) => JsonUtility.FromJson<SaveableKeysWrapper>(json);
        }
    }
}