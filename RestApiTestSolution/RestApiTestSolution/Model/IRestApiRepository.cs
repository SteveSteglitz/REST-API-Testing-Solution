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

        RestApiCall ReadRestCallFile(string projectName);

        void WriteRestCallFile(RestApiCall restCall);
    }
}
