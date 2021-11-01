using System;
using UnityEngine.AddressableAssets;

namespace IdleCivilization.Client.UI
{
    [Serializable]
    public class WindowViewAsset
    {
#if UNITY_EDITOR
        public WindowViewBase view;
#endif
        public string viewType;
        public bool cacheInMemory;
        public AssetReference assetReference;
    }
}