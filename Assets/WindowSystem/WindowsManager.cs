using System;
using System.Diagnostics;
using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;

namespace IdleCivilization.Client.UI
{
    public class WindowsManager : IWindowsManager
    {
        private readonly WindowControllerFactory windowControllerFactory;
        private readonly EventSystem eventSystem;

        public WindowsManager(
            WindowControllerFactory windowControllerFactory,
            EventSystem eventSystem)
        {
            this.windowControllerFactory = windowControllerFactory;
            this.eventSystem = eventSystem;
        }

        public async UniTask<TWindowController> OpenAsync<TWindowController>(bool animated = true, params object[] args)
            where TWindowController : class, IWindowController, IWindowLoader
        {
            var controller = await InternalOpenWindowAsync<TWindowController>(animated, args);
            WindowsStorage.AddOpenedController(controller);
            return controller;
        }

        public UniTask<TWindowController> OpenSingleAsync<TWindowController>(bool animated = true, params object[] args)
            where TWindowController : class, IWindowController, IWindowLoader
        {
            if (IsOpen<TWindowController>())
                return UniTask.FromResult(GetOpened<TWindowController>());

            return OpenAsync<TWindowController>(animated, args);
        }

        public bool IsOpen<TWindowController>() where TWindowController : class, IWindowController =>
            WindowsStorage.HasOpenedController<TWindowController>();

        public TWindowController GetOpened<TWindowController>()
            where TWindowController : class, IWindowController, IWindowLoader
        {
            if (!IsOpen<TWindowController>())
                throw new IndexOutOfRangeException(
                    $"Can't find opened window controller with type {typeof(TWindowController)}");

            return WindowsStorage.GetOpenedController<TWindowController>();
        }

        public UniTask CloseAsync(IWindowController windowController, bool animated = true) =>
            InternalCloseAsync(windowController, animated)
                .ContinueWith(() => WindowsStorage.RemoveOpenedController(windowController));

        public static void Log(string message) => Debug.Log($"[WM] {message}");

        private async UniTask<TWindowController> InternalOpenWindowAsync<TWindowController>(bool animated,
            object[] args)
            where TWindowController : class, IWindowController, IWindowLoader
        {
            var stopwatch = Stopwatch.StartNew();
            eventSystem.enabled = false;
            var controller = windowControllerFactory.Create<TWindowController>(args);
            var loadingResult = await controller.LoadWindowAsync();
            var controllerName = controller.GetType().Name;

            if (!loadingResult)
                throw new Exception($"Window {controllerName} loading failed!");

            stopwatch.Stop();
            Log($"{controllerName}. loading time {stopwatch.ElapsedMilliseconds}");
            controller.Initialize();
            await controller.ShowWindowAsync(animated);
            eventSystem.enabled = true;
            return controller;
        }

        private UniTask InternalCloseAsync(IWindowController windowController, bool animated = true)
        {
            eventSystem.enabled = false;
            return windowController.HideWindowAsync(animated).ContinueWith(() =>
            {
                eventSystem.enabled = true;
                windowController.Dispose();
            });
        }
    }
}