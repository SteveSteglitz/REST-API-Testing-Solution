using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RestApiTestSolution.Model
{
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

        public ApiProject ReadRestCallFile(string path, string projectName)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var jsonData = System.IO.File.ReadAllText($"{path}\\{projectName}.json");
            return JsonConvert.DeserializeObject<ApiProject>(jsonData);
        }

        public void WriteRestCallFile(string path, ApiProject project)
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
