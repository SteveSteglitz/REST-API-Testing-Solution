﻿using System;
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

        void SaveProject(string path, RestApiCall restCall);

        RestApiCall LoadProject(string path, string projectName);

        void RemoveProject(string path, RestApiCall restApiCall);

        Task<HttpResponseMessage> SendHttpRequest(string accessToken, string baseUrl, RestApiCall restApiCallProject, RestApiCallItem restApiCallItem, CancellationToken cancellationToken);
    }
}
