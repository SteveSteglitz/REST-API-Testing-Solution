using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestApiTestSolution.Model
{
    public interface IRestApiRepository
    {
        IList<String> GetAllProjects(string path);

        RestApiCall ReadRestCallFile(string path, string projectName);

        void WriteRestCallFile(string path, RestApiCall restCall);

        void DeleteRestCallFile(string path);
    }
}
