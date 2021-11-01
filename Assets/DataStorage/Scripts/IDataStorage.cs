namespace NineGames.Storage
{
    public interface IDataStorage
    {
        T GetContainer<T>() where T : DataContainerBase, new();
        void SaveContainer(DataContainerBase dataContainerBase);
        void SaveAllContainers();
    }
}