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
            var files = Directory.GetFiles(path, "*.json");
            return files.Select(Path.GetFileNameWithoutExtension).ToList();
        }

        public RestApiCall ReadRestCallFile(string projectName)
        {
            var jsonData = System.IO.File.ReadAllText($"{projectName}.json");
            return JsonConvert.DeserializeObject<RestApiCall>(jsonData);
        }

        public void WriteRestCallFile(RestApiCall restCall)
        {
            var jsonData = JsonConvert.SerializeObject(restCall);
            System.IO.File.WriteAllText($"{restCall.Project}.json", jsonData);
        }
    }
}
