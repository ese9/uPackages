using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IdleCivilization.Client.SaveLoadSystem
{
    public static class DataPatchUtility
    {
        private static readonly PatchSaveData PatchInfo = new PatchSaveData();
        private static string PatchInfoKey => PatchSaveData.PatchVersion;

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
                patchedJson = JsonConvert.SerializeObject(containers, DataContainer.SerializerSettings);
            }

            return true;
        }

        public static void ForcePatchData(string json, DataVersion dataVersion, out string patchedJson)
        {
            TryPatchData(json, dataVersion, out patchedJson);
            var containers = JsonConvert.DeserializeObject<Dictionary<string, string>>(patchedJson);
            UpdatePatchVersion(containers, dataVersion);
            patchedJson = JsonConvert.SerializeObject(containers, DataContainer.SerializerSettings);
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
                containers.Add(PatchInfoKey, PatchInfo.SerializeData());
                return DataVersion.MinValue;
            }

            var patchJson = containers[PatchInfoKey];
            var appJObject = JObject.Parse(patchJson);
            return DataVersion.Parse(appJObject[PatchInfoKey].ToString());
        }
    }
}