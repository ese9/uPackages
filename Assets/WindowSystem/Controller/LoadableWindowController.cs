using Cysharp.Threading.Tasks;
using Zenject;

namespace IdleCivilization.Client.UI
{
    public abstract class LoadableWindowController<TWindowView> : WindowControllerBase<TWindowView>, IWindowLoader
        where TWindowView : WindowViewBase
    {
        private IWindowViewFactory viewFactory;

        [Inject]
        private void InjectViewFactory(IWindowViewFactory viewFactory)
        {
            this.viewFactory = viewFactory;
        }

        protected virtual UniTask<bool> DoLoadAsync() => UniTask.FromResult(true);

        async UniTask<bool> IWindowLoader.LoadWindowAsync()
        {
            var viewLoadOperation = await viewFactory.CreateAsync<TWindowView>();

            if (viewLoadOperation.IsCompleted)
                SetView(viewLoadOperation.Result);

            var result = await DoLoadAsync();
            await UniTask.WaitForEndOfFrame();
            return viewLoadOperation.IsCompleted && result;
        }

        protected sealed override void OnViewDidHidden() => viewFactory.Release(View);
    }
}