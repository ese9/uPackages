namespace IdleCivilization.Client.SaveLoadSystem
{
    public interface IDataContainer
    {
        T GetData<T>() where T : SaveableBase, new();
        void SaveData(SaveableBase saveableBase);
    }
}