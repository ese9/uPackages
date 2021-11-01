using Cysharp.Threading.Tasks;

namespace IdleCivilization.Client.UI
{
    public interface IWindowViewFactory
    {
        UniTask<OperationResult<TWindowView>> CreateAsync<TWindowView>()
            where TWindowView : WindowViewBase;

        void Release(WindowViewBase windowView);
    }
}