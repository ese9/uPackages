using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;
using Zenject;

namespace IdleCivilization.Client.UI
{
    [CreateAssetMenu(fileName = "Addressable Window View Container",
        menuName = "Game/Window/Addressable View Container")]
    public class AddressableWindowViewContainer : ScriptableObject, IInitializable
    {
        [SerializeField] private List<WindowViewAsset> windowAssets;

        private Dictionary<Type, AssetReferenceInfo> windowReferences;
        public IReadOnlyList<WindowViewAsset> WindowAssets => windowAssets;


        public void Initialize()
        {
            windowReferences = new Dictionary<Type, AssetReferenceInfo>();

            foreach (var windowAsset in windowAssets)
            {
                var type = Type.GetType(windowAsset.viewType);
                if (type == null)
                    throw new Exception($"Can't restore view with type {windowAsset.viewType}");

                windowReferences[type] = new AssetReferenceInfo(windowAsset.assetReference, windowAsset.cacheInMemory);
            }
        }

        public AssetReferenceInfo GetReferenceInfo<TWindowView>() where TWindowView : WindowViewBase =>
            GetReferenceInfo(typeof(TWindowView));

        public AssetReferenceInfo GetReferenceInfo(Type type)
        {
            Assert.That(windowReferences.ContainsKey(type), $"!windowReferences.ContainsKey {type}");
            return windowReferences[type];
        }
    }
}