using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Documents;
using System.Windows.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestApiTestSolution.Model;

namespace RestApiTestSolution.ViewModel
{
    public class RestApiViewModel : BaseViewModel
    {
        private IRestApiManager _manager;
        private bool _projectsIsVisible;
        private RestApiCallItem _restCallItem;
        private RestApiCall _restCallProject;
        private string _receiveMessage;

        public RestApiViewModel()
        {
        }

        public RestApiViewModel(IRestApiManager manager)
        {
            _manager = manager;
            ShowProjectsCommand = new RelayCommand(o =>
            {
                ProjectsIsVisible = ProjectsIsVisible != true;
            });
            SendMessageCommand = new RelayCommand(SendMessage, o => true);
            AllProjectNames = new ObservableCollection<string>();
            RESTCallItems = new ObservableCollection<RestApiCallItem>();
            GetAllProjectNames();
            ProjectsIsVisible = true;
            IsProjectDeleteFieldEnable = true;
            IsProjectNewFieldEnable = true;
            IsProjectSaveFieldEnable = false;
        }

        private void SendMessage(Object obj)
        {
            HttpMethod httpMethod;
            switch (RESTCallItem.HttpVerb)
            {
                case HttpVerb.GET:
                    PostStreamAsync(RESTCallItem.Body, HttpMethod.Get, CancellationToken.None).Wait();
                    break;
                case HttpVerb.POST:
                    PostStreamAsync(RESTCallItem.Body, HttpMethod.Post, CancellationToken.None).Wait();
                    break;
                case HttpVerb.PUT:
                    httpMethod = HttpMethod.Put;
                    break;
                case HttpVerb.DELETE:
                    httpMethod = HttpMethod.Delete;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            //PostStreamAsync(RESTCallItem.Body, httpMethod, CancellationToken.None).Wait();
            if (!string.IsNullOrEmpty(ReceiveMessage))
            {
                dynamic d = JObject.Parse(ReceiveMessage);
                if (d.AccessToken != null)
                {
                    AccessToken = d.AccessToken;
                }
            }
        }

        private HttpContent CreateHttpContent(string content)
        {
            if (content == null) return null;
            HttpContent httpContent = new StringContent(content);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return httpContent;
        }

        private async Task PostStreamAsync(string content, HttpMethod httpMethod, CancellationToken cancellationToken)
        {
            ServicePointManager.ServerCertificateValidationCallback = (message, cert, chain, errors) => true;
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(httpMethod, $"{RESTCallProject.BaseUrl}{RESTCallItem.Route}"))
            using (var httpContent = CreateHttpContent(content))
            {
                request.Content = httpContent;
                if (!string.IsNullOrEmpty(RESTCallProject.AuthorizationScheme) &&
                    !string.IsNullOrEmpty(RESTCallProject.AuthorizationParameter) &&
                    !string.IsNullOrEmpty(AccessToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(RESTCallProject.AuthorizationScheme, AccessToken);
                }

                using (var response = await client
                    .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                    .ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                    ReceiveMessage = response.Content.ReadAsStringAsync().Result;
                }
            }
        }

        public ICommand ShowProjectsCommand { get; set; }

        public ICommand SendMessageCommand { get; set; }

        public IEnumerable<HttpVerb> HttpVerbsEnumValues => Enum.GetValues(typeof(HttpVerb)).Cast<HttpVerb>();

        public ObservableCollection<string> AllProjectNames { get; set; }

        public ObservableCollection<RestApiCallItem> RESTCallItems { get; set; }

        public RestApiCall RESTCallProject
        {
            get => _restCallProject;
            set
            {
                _restCallProject = value;
                OnPropertyChanged();
            }
        }

        public RestApiCallItem RESTCallItem
        {
            get => _restCallItem;
            set
            {
                _restCallItem = value;
                OnPropertyChanged();
            }
        }

        public string AccessToken { get; set; }

        public string ReceiveMessage
        {
            get { return _receiveMessage; }
            set
            {
                _receiveMessage = value; 
                OnPropertyChanged();
            }
        }

        public bool ProjectsIsVisible
        {
            get => _projectsIsVisible;
            set
            {
                _projectsIsVisible = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ProjectsIsNotVisible));
            }
        }

        public bool ProjectsIsNotVisible
        {
            get => !_projectsIsVisible;
            set
            {
                _projectsIsVisible = !value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ProjectsIsVisible));
            }
        }


        public bool IsProjectNewFieldEnable { get; set; }

        public bool IsProjectDeleteFieldEnable { get; set; }

        public bool IsProjectSaveFieldEnable { get; set; }

        public bool IsRouteNewFieldEnable { get; set; }

        public bool IsRouteDeleteFieldEnable { get; set; }

        public bool IsRouteSaveFieldEnable { get; set; }

        private void GetAllProjectNames()
        {
            foreach (var project in _manager.GetAllProjects(@"Projects\"))
            {
                AllProjectNames.Add(project);
            }
        }

        public void LoadProject(string projectName)
        {
            RESTCallProject = _manager.LoadProject($"Projects\\{projectName}");
            foreach (var restCallItem in RESTCallProject.Items)
            {
                RESTCallItems.Add(restCallItem);
            }

            RESTCallItem = RESTCallProject.Items.FirstOrDefault();
        }

        private void SaveProject()
        {
            _manager.SaveProject(RESTCallProject);
        }
    }
}
