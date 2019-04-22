namespace DataLayer.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using DataLayer.Interfaces;
    using DataLayer.Models;
    using Newtonsoft.Json;

    public class RestApiRepository : IRestApiRepository
    {
        public IList<String> GetAllProjects(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var files = Directory.GetFiles(path, "*.json");
            return files.Select(Path.GetFileNameWithoutExtension).ToList();
        }

        public ProjectDataModel ReadRestCallFile(string path, string projectName)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var jsonData = System.IO.File.ReadAllText($"{path}\\{projectName}.json");
            return JsonConvert.DeserializeObject<ProjectDataModel>(jsonData);
        }

        public void WriteRestCallFile(string path, ProjectDataModel project)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var jsonData = JsonConvert.SerializeObject(project);
            System.IO.File.WriteAllText($"{path}\\{project.Project}.json", jsonData);
        }

        public void DeleteRestCallFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
