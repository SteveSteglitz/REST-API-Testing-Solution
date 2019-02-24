using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestApiTestSolution.Model
{
    public class RestApiManager : IRestApiManager
    {
        private IRestApiRepository _repository;

        public RestApiManager(IRestApiRepository repository)
        {
            _repository = repository;
        }

        public IList<string> GetAllProjects(string path)
        {
            return _repository.GetAllProjects(path);
        }

        public void SaveProject(RestApiCall restApiCall)
        {
            _repository.WriteRestCallFile(restApiCall);
        }

        public RestApiCall LoadProject(string projectName)
        {
            return _repository.ReadRestCallFile(projectName);
        }
    }
}
