using Cysharp.Threading.Tasks;

namespace IdleCivilization.Client.UI
{
    public interface IWindowsManager
    {
        UniTask<TWindowController> OpenAsync<TWindowController>(bool animated = true, params object[] args)
            where TWindowController : class, IWindowController, IWindowLoader;

        UniTask<TWindowController> OpenSingleAsync<TWindowController>(bool animated = true, params object[] args)
            where TWindowController : class, IWindowController, IWindowLoader;

        bool IsOpen<TWindowController>() 
            where TWindowController : class, IWindowController;

        TWindowController GetOpened<TWindowController>()
            where TWindowController : class, IWindowController, IWindowLoader;

        UniTask CloseAsync(IWindowController windowController, bool animated = true);
    }
}