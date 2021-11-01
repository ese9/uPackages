namespace IdleCivilization.Client.UI
{
    public interface IWindowControllerOwner
    {
        void AddOwnership(IWindowController windowController);
        void RemoveOwnership(IWindowController windowController);
    }
}