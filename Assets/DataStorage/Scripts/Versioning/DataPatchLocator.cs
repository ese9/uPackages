using System.Collections.Generic;
using System.Linq;

namespace NineGames.Storage
{
    public static class DataPatchLocator
    {
        public static IList<DataPatch> GetPatchListToVersion(DataVersion version) =>
            DataConfig.Current.Patches
                .Where(x => !ReferenceEquals(x, null) && x.Version <= version)
                .ToArray();
        
        public static IList<DataPatch> GetPatchListFromToVersion(DataVersion from, DataVersion to) =>
            DataConfig.Current.Patches
                .Where(x => !ReferenceEquals(x, null) && x.Version > from && x.Version <= to)
                .ToArray();
    }
}