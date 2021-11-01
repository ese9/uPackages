using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace NineGames.Storage
{
    public class DataConfig : ScriptableObject
    {
        private const string ConfigFolder = "DataContainer";
        private const string Name = "Config";
        private static string ConfigPath => Path.Combine(ConfigFolder, Name);

        private static DataConfig current;

        [SerializeField] private string version = "1.0";
        [SerializeField, HideInInspector] private int major;
        [SerializeField, HideInInspector] private int minor;
        [SerializeField] private List<DataPatch> patches = new List<DataPatch>();
        
        public static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            NullValueHandling = NullValueHandling.Ignore,
            ObjectCreationHandling = ObjectCreationHandling.Replace
        };

        public DataVersion Version => new DataVersion(major, minor);
        public IReadOnlyCollection<DataPatch> Patches => patches;

        public static DataConfig Current
        {
            get
            {
                if (!ReferenceEquals(current, null))
                    return current;

                current = Resources.Load<DataConfig>(ConfigPath);

                if (!ReferenceEquals(current, null))
                    return current;

                throw new NotImplementedException("Can't find Data Config asset in Resources folder");
            }
        }

        private void OnValidate()
        {
            CheckVersion();
            SortPatches();
        }

        private void CheckVersion()
        {
            if (DataVersion.TryParse(version, out var dataVersion))
            {
                major = dataVersion.Major;
                minor = dataVersion.Minor;
            }
        }

        private void SortPatches()
        {
            patches = patches
                .Where(x => !ReferenceEquals(x, null))
                .OrderBy(x => x.Version).ToList();
        }
    }
}