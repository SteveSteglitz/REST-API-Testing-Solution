using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RestApiTestSolution.Model
{
    public interface IRestApiManager
    {
        IList<String> GetAllProjects(string path);

        void SaveProject(string path, ApiProject project);

        ApiProject LoadProject(string path, string projectName);

        void RemoveProject(string path, ApiProject apiProject);

        Task<HttpResponseMessage> SendHttpRequest(string accessToken, string baseUrl, ApiProject apiProjectProject, ApiRoute apiRoute, CancellationToken cancellationToken);
    }
}
