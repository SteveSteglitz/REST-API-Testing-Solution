using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using DataLayer.Interfaces;
using DataLayer.Models;

namespace BusinessLayer.Implementations
{
    public class RestApiManager : IRestApiManager
    {
        public RestApiManager(IRestApiRepository repository)
        {
            Repository = repository;
            Mapper = InitMapper();
        }

        public IRestApiRepository Repository { get; }

        public IMapper Mapper { get; }

        private IMapper InitMapper()
        {
            var config = new MapperConfiguration(x =>
            {
                x.CreateMap<ProjectBusinessModel, ProjectDataModel>();
                x.CreateMap<RouteBusinessModel, RouteDataModel>();
                x.CreateMap<VariableBusinessModel, VariableDataModel>();
            });

            return config.CreateMapper();
        }

        public IList<string> GetAllProjects(string path)
        {
            return Repository.GetAllProjects(path);
        }

        public void SaveProject(string path, ProjectBusinessModel project)
        {
            Repository.WriteRestCallFile(path, Mapper.Map<ProjectDataModel>(project));
        }

        public ProjectBusinessModel LoadProject(string path, string projectName)
        {
            return Mapper.Map<ProjectBusinessModel>(Repository.ReadRestCallFile(path, projectName));
        }

        public void RemoveProject(string path, ProjectBusinessModel apiProject)
        {
            Repository.DeleteRestCallFile($"{path}\\{apiProject.Project}.json");
        }

        public async Task<HttpResponseMessage> SendHttpRequest(string accessToken, string baseUrl, ProjectBusinessModel apiProjectProject, RouteBusinessModel apiRoute,
            CancellationToken cancellationToken)
        {
            ServicePointManager.ServerCertificateValidationCallback = (message, cert, chain, errors) => true;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (!string.IsNullOrEmpty(apiProjectProject.AuthorizationScheme) &&
                        !string.IsNullOrEmpty(apiProjectProject.AuthorizationParameter))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(apiProjectProject.AuthorizationScheme, apiProjectProject.GetStringReplacedWithVariable(apiProjectProject.AuthorizationParameter));
                }

                if (apiRoute.HttpVerb == "GET")
                {
                    HttpResponseMessage response = await client.GetAsync($"{baseUrl}/{apiProjectProject.GetStringReplacedWithVariable(apiRoute.Route)}");
                    if (response.IsSuccessStatusCode)
                    {
                        return response;
                    }
                }else if (apiRoute.HttpVerb == "POST")
                {
                    HttpResponseMessage response = await client.PostAsync($"{baseUrl}/{apiProjectProject.GetStringReplacedWithVariable(apiRoute.Route)}", CreateHttpContent(apiRoute.Body));
                    return response;
                }else if (apiRoute.HttpVerb == "PUT")
                {
                    HttpResponseMessage response = await client.PutAsync($"{baseUrl}/{apiProjectProject.GetStringReplacedWithVariable(apiRoute.Route)}", CreateHttpContent(apiRoute.Body));
                    return response;
                }
                else if (apiRoute.HttpVerb == "DELETE")
                {
                    HttpResponseMessage response = await client.DeleteAsync($"{baseUrl}/{apiProjectProject.GetStringReplacedWithVariable(apiRoute.Route)}");
                    return response;
                }
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
            //return String.Empty;

        }

        private HttpMethod GetHttpMethod(string httpVerb)
        {
            switch (httpVerb)
            {
                case "GET":
                    return HttpMethod.Get;
                case "POST":
                    return HttpMethod.Post;
                case "PUT":
                    return HttpMethod.Put;
                case "DELETE":
                    return HttpMethod.Delete;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private HttpContent CreateHttpContent(string content)
        {
            if (content == null)
            {
                return null;
            }

            HttpContent httpContent = new StringContent(content);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return httpContent;
        }
    }
}
