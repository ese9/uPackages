using System;
using Cysharp.Threading.Tasks;
using UniRx;
using Zenject;

namespace IdleCivilization.Client.UI
{
    public abstract class WindowControllerBase<TWindowView> : IWindowController, IDisposableOwner
        where TWindowView : WindowViewBase
    {
        private readonly CompositeDisposable compositeDisposable = new CompositeDisposable();
        private IInstantiator instantiator;
        protected TWindowView View { get; private set; }
        protected IWindowsManager WindowsManager { get; private set; }

        [Inject]
        private void InjectDependencies(
            IInstantiator instantiator,
            IWindowsManager windowsManager)
        {
            this.instantiator = instantiator;
            WindowsManager = windowsManager;
        }

        public void Initialize() => DoInitialize();
        public void AddOwnership(IDisposable disposable) => compositeDisposable.Add(disposable);
        public void RemoveOwnership(IDisposable disposable) => compositeDisposable.Remove(disposable);

        protected virtual void DoInitialize() { }
        protected virtual void DoDispose() { }
        protected virtual UniTask DoShowAsync(bool animated = true) => UniTask.CompletedTask;
        protected virtual UniTask DoHideAsync(bool animated = true) => UniTask.CompletedTask;
        protected virtual void OnViewDidShown() { }
        protected virtual void OnViewDidHidden() { }

        protected void SetView(TWindowView view) => View = view;

        protected TController CreateSubController<TController>(params object[] args)
            where TController : class, IInitializable, IDisposable
        {
            var controller = instantiator.Instantiate<TController>(args);
            AddOwnership(controller);
            return controller;
        }

        UniTask IWindowController.ShowWindowAsync(bool animated)
        {
            return UniTask.WhenAll(View.AppearAsync(animated), DoShowAsync(animated))
                .ContinueWith(OnViewDidShown);
        }

        UniTask IWindowController.HideWindowAsync(bool animated) =>
            UniTask.WhenAll(View.DisappearAsync(animated), DoHideAsync(animated))
                .ContinueWith(OnViewDidHidden);

        void IDisposable.Dispose()
        {
            compositeDisposable.Dispose();
            DoDispose();
        }
    }
}