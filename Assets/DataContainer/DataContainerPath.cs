using System.IO;
using UnityEngine;

namespace IdleCivilization.Client.SaveLoadSystem
{
    public static class DataContainerPath
    {
#if UNITY_EDITOR
        public static string SaveLocation => Application.dataPath;
#else
        public static string SaveLocation => Application.persistentDataPath;
#endif

        public static string SavePath => Path.Combine(SaveLocation, "Saves");
    }
}