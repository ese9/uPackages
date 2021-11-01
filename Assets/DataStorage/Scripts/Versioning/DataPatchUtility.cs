using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NineGames.Storage
{
    public static class DataPatchUtility
    {
        private static readonly PatchDataContainer PatchInfo = new PatchDataContainer();
        private static string PatchInfoKey => PatchDataContainer.PatchVersion;

        public static void Log(string message) => UnityEngine.Debug.Log($"<b>[PATCH] {message}</b>");

        public static bool TryPatchData(string json, DataVersion targetVersion, out string patchedJson)
        {
            patchedJson = json;

            var containers = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            var dataVersion = GetPatchVersion(containers);
            var patchList = DataPatchLocator.GetPatchListFromToVersion(dataVersion, targetVersion);
            var shouldUpdatePatchVersion = false;

            if (dataVersion < targetVersion && patchList.Count == 0)
                return false;

            foreach (var patch in patchList)
            {
                if (dataVersion >= patch.Version)
                    continue;

                patch.Patch(containers);
                dataVersion = patch.Version;
                shouldUpdatePatchVersion = true;
            }

            if (shouldUpdatePatchVersion)
            {
                UpdatePatchVersion(containers, dataVersion);
                patchedJson = JsonConvert.SerializeObject(containers, DataConfig.SerializerSettings);
            }

            return true;
        }

        public static void ForcePatchData(string json, DataVersion dataVersion, out string patchedJson)
        {
            TryPatchData(json, dataVersion, out patchedJson);
            var containers = JsonConvert.DeserializeObject<Dictionary<string, string>>(patchedJson);
            UpdatePatchVersion(containers, dataVersion);
            patchedJson = JsonConvert.SerializeObject(containers, DataConfig.SerializerSettings);
        }

        private static void UpdatePatchVersion(IDictionary<string, string> containers, DataVersion version)
        {
            var patchJson = containers[PatchInfoKey];
            var appJObject = JObject.Parse(patchJson);
            appJObject[PatchInfoKey] = version.ToString();
            containers[PatchInfoKey] = appJObject.ToString();
        }

        private static DataVersion GetPatchVersion(IDictionary<string, string> containers)
        {
            if (!containers.TryGetValue(PatchInfoKey, out _))
            {
                PatchInfo.SetDataVersion(DataVersion.MinValue);
                var patchContainerJson = JsonConvert.SerializeObject(PatchInfo, Formatting.Indented, DataConfig.SerializerSettings);
                containers.Add(PatchInfoKey, patchContainerJson);
                return DataVersion.MinValue;
            }

            var patchJson = containers[PatchInfoKey];
            var appJObject = JObject.Parse(patchJson);
            return DataVersion.Parse(appJObject[PatchInfoKey].ToString());
        }
    }
}