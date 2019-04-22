namespace BusinessLayer.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using BusinessLayer.Models;

    public interface IRestApiManager
    {
        IList<String> GetAllProjects(string path);

        void SaveProject(string path, ProjectBusinessModel project);

        ProjectBusinessModel LoadProject(string path, string projectName);

        void RemoveProject(string path, ProjectBusinessModel apiProject);

        Task<HttpResponseMessage> SendHttpRequest(string accessToken, string baseUrl, ProjectBusinessModel apiProjectProject, RouteBusinessModel apiRoute, CancellationToken cancellationToken);
    }
}
