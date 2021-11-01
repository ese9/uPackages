using System;
using System.Collections.Generic;

namespace IdleCivilization.Client.UI
{
    internal static class WindowsStorage
    {
        private static readonly Dictionary<Type, IWindowController> OpenedWindows;
        private static readonly Dictionary<Type, IList<IWindowController>> OpenedModalWindows;

        static WindowsStorage()
        {
            OpenedWindows = new Dictionary<Type, IWindowController>();
            OpenedModalWindows = new Dictionary<Type, IList<IWindowController>>();
        }

        internal static void AddOpenedController(IWindowController controller) =>
            OpenedWindows[controller.GetType()] = controller;

        internal static bool HasOpenedController<TWindowController>()
            where TWindowController : class, IWindowController =>
            OpenedWindows.ContainsKey(typeof(TWindowController));

        internal static bool HasOpenedController(IWindowController windowController) =>
            OpenedWindows.ContainsKey(windowController.GetType());

        public static void RemoveOpenedController(IWindowController windowController) =>
            OpenedWindows.Remove(windowController.GetType());

        public static void AddOpenedModalController(IWindowController controller)
        {
            var type = controller.GetType();

            if (OpenedModalWindows.ContainsKey(type))
                OpenedModalWindows[type].Add(controller);
            else
                OpenedModalWindows[type] = new List<IWindowController> { controller };
        }

        public static void RemoveOpenedModalController(IWindowController windowController) =>
            OpenedModalWindows[windowController.GetType()].Remove(windowController);

        public static TWindowController GetOpenedController<TWindowController>()
            where TWindowController : class, IWindowController =>
            (TWindowController)OpenedWindows[typeof(TWindowController)];
    }
}