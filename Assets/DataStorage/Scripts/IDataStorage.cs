namespace DS.Core
{
    public interface IDataStorage
    {
        T GetContainer<T>() where T : DataContainerBase, new();
        void SaveContainer(DataContainerBase dataContainerBase);
        void SaveAllContainers();
    }
}