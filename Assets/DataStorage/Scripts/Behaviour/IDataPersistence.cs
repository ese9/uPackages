using System.Collections.Generic;

namespace DS.Core
{
    public interface IDataPersistence
    {
        bool ContainerExists(string key);
        
        void CreateContainer(string key, string text);
        void SaveContainer(string key, string text);
        string LoadContainer(string key);
        
        Dictionary<string, string> GetContainers();
        void SetContainers(Dictionary<string, string> containers);
        
        void ClearContainers();
    }
}