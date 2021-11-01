using System.Collections.Generic;
using System.IO;

namespace IdleCivilization.Client.SaveLoadSystem
{
    public class DataPersistenceIOBehaviour : IDataPersistenceBehaviour
    {
        private readonly string saveLocation;
        private readonly string fileExtension;

        private static string FileExtension => ".json";

        public static readonly DataPersistenceIOBehaviour Default =
            new DataPersistenceIOBehaviour(DataContainerPath.SavePath, FileExtension);

        public DataPersistenceIOBehaviour(string saveLocation, string fileExtension)
        {
            this.saveLocation = saveLocation;
            this.fileExtension = fileExtension;
        }

        public bool ContainerExists(string key) =>
            File.Exists(GetContainerPath(key));

        public void CreateContainer(string key, string text)
        {
            Directory.CreateDirectory(saveLocation);
            File.WriteAllText(GetContainerPath(key), text);
        }

        public void SaveContainer(string key, string text) =>
            CreateContainer(key, text);

        public string LoadContainer(string key) =>
            File.ReadAllText(GetContainerPath(key));

        public Dictionary<string, string> GetContainers()
        {
            var containers = new Dictionary<string, string>();
            var dirInfo = Directory.CreateDirectory(saveLocation);
            var files = dirInfo.GetFiles();

            foreach (var file in files)
            {
                if (!string.Equals(file.Extension, fileExtension))
                    continue;

                var key = Path.GetFileNameWithoutExtension(file.Name);
                var text = LoadContainer(key);
                containers.Add(key, text);
            }

            return containers;
        }

        public void SetContainers(Dictionary<string, string> containers)
        {
            foreach (var container in containers)
                CreateContainer(container.Key, container.Value);
        }

        public void ClearContainers()
        {
            if (Directory.Exists(saveLocation))
                Directory.Delete(saveLocation, true);
        }

        private string GetContainerPath(string key) =>
            Path.Combine(saveLocation, $"{key}{fileExtension}");
    }
}