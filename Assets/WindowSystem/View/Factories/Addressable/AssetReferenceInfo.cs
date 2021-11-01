using UnityEngine.AddressableAssets;

namespace IdleCivilization.Client
{
    public readonly struct AssetReferenceInfo
    {
        public readonly AssetReference AssetReference;
        public readonly bool CacheInMemory;

        public AssetReferenceInfo(AssetReference assetReference, bool cacheInMemory)
        {
            CacheInMemory = cacheInMemory;
            AssetReference = assetReference;
        }
    }
}