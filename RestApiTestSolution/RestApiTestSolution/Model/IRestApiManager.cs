using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestApiTestSolution.Model
{
    public interface IRestApiManager
    {
        IList<String> GetAllProjects(string path);

        void SaveProject(string path, RestApiCall restCall);

        RestApiCall LoadProject(string path, string projectName);
    }
}
