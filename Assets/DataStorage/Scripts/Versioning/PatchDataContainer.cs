using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DS.Core
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PatchDataContainer : DataContainerBase
    {
        public const string PatchVersion = "patch-version";
        
        [JsonProperty(PatchVersion)] private string dataVersion;
        public override string SaveableKey => PatchVersion;
        public DataVersion DataVersion { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (DataVersion.TryParse(dataVersion, out var version))
                DataVersion = version;
        }

        public void SetDataVersion(DataVersion version)
        {
            DataVersion = version;
            dataVersion = version.ToString();
        }
    }
}