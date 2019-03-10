using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
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

        public void SaveProject(string path, RestApiCall restApiCall)
        {
            _repository.WriteRestCallFile(path, restApiCall);
        }

        public RestApiCall LoadProject(string path, string projectName)
        {
            return _repository.ReadRestCallFile(path, projectName);
        }

        public void RemoveProject(string path, RestApiCall restApiCall)
        {
            _repository.DeleteRestCallFile($"{path}\\{restApiCall.Project}.json");
        }

        public async Task<HttpResponseMessage> SendHttpRequest(string accessToken, string baseUrl, RestApiCall restApiCallProject, RestApiCallItem restApiCallItem,
            CancellationToken cancellationToken)
        {
            ServicePointManager.ServerCertificateValidationCallback = (message, cert, chain, errors) => true;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (!string.IsNullOrEmpty(restApiCallProject.AuthorizationScheme) &&
                        !string.IsNullOrEmpty(restApiCallProject.AuthorizationParameter) &&
                        !string.IsNullOrEmpty(accessToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(restApiCallProject.AuthorizationScheme, accessToken);
                }

                if (restApiCallItem.HttpVerb == "GET")
                {
                    HttpResponseMessage response = await client.GetAsync($"slmobileApi/{restApiCallItem.Route}");
                    if (response.IsSuccessStatusCode)
                    {
                        return response;
                    }
                }else if (restApiCallItem.HttpVerb == "POST")
                {
                    HttpResponseMessage response = await client.PostAsync($"slmobileApi/{restApiCallItem.Route}", CreateHttpContent(restApiCallItem.Body));
                    return response;
                }else if (restApiCallItem.HttpVerb == "PUT")
                {
                    HttpResponseMessage response = await client.PutAsync($"slmobileApi/{restApiCallItem.Route}", CreateHttpContent(restApiCallItem.Body));
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
