using System;
using IdleCivilization.Client.UI.Factories;
using UnityEngine;
using Zenject;

namespace IdleCivilization.Client.UI
{
    [CreateAssetMenu(menuName = "Game/Window/Installer")]
    public class WindowSystemInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private WindowLoadingTypes loadingTypes;
        // TODO: Add editor variables display in editor class based on loading type
        [SerializeField] private AddressableWindowViewContainer addressableWindowViewContainer;
        // TODO: Add editor logic with optional model binding
        [SerializeField] private bool shouldBindModels;
        [SerializeField] private WindowModelContainer windowModelContainer;
        [SerializeField] private Canvas rootCanvas;
        [SerializeField] private int sortingOrder;

        public override void InstallBindings()
        {
            if (shouldBindModels)
                BindModel();

            BindView();
            BindController();

            Container.Bind<IWindowsManager>().To<WindowsManager>().AsTransient();
        }

        private void BindModel()
        {
            foreach (var model in windowModelContainer.Models)
            {
                Container.Bind(model.GetType())
                    .FromInstance(model)
                    .AsSingle();

                Container.QueueForInject(model);
            }
        }

        private void BindController()
        {
            Container.Bind<WindowControllerFactory>().AsSingle();
        }

        private void BindView()
        {
            var canvas = Container.InstantiatePrefabForComponent<Canvas>(rootCanvas);
            canvas.sortingOrder = sortingOrder;

            switch (loadingTypes)
            {
                case WindowLoadingTypes.Prefab:
                    Container.Bind<IWindowViewFactory>().To<PrefabWindowViewFactory>().AsSingle();
                    break;
                case WindowLoadingTypes.Addressable:
                    addressableWindowViewContainer.Initialize();
                    Container.Bind<IWindowViewFactory>().To<AddressableWindowViewFactory>().AsSingle()
                        .WithArguments(addressableWindowViewContainer, canvas);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private enum WindowLoadingTypes
        {
            Prefab = 0,
            Addressable = 1
        }
    }
}