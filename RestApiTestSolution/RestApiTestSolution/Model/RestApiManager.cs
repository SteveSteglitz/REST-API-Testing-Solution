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

        public async Task<string> SendHttpRequest(string accessToken, RestApiCall restApiCallProject, RestApiCallItem restApiCallItem,
            CancellationToken cancellationToken)
        {
            ServicePointManager.ServerCertificateValidationCallback = (message, cert, chain, errors) => true;
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(GetHttpMethod(restApiCallItem.HttpVerb), $"{restApiCallProject.BaseUrl}{restApiCallItem.Route}"))
            using (var httpContent = CreateHttpContent(restApiCallItem.Body))
            {
                request.Content = httpContent;
                if (!string.IsNullOrEmpty(restApiCallProject.AuthorizationScheme) &&
                    !string.IsNullOrEmpty(restApiCallProject.AuthorizationParameter) &&
                    !string.IsNullOrEmpty(accessToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(restApiCallProject.AuthorizationScheme, accessToken);
                }

                using (var response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }

                //using (var response = await client
                //    .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                //    .ConfigureAwait(false))
                //{
                //    response.EnsureSuccessStatusCode();
                //    return await response.Content.ReadAsStringAsync();
                //}
            }
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
