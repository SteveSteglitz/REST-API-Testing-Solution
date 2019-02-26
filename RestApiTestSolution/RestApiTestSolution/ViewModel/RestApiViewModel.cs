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
            ShowProjectsCommand = new RelayCommand(o => { ProjectsIsVisible = ProjectsIsVisible != true; });
            SendMessageCommand = new AsyncCommand(async () => {
                var responseMessage = _manager.SendHttpRequest(AccessToken, RESTCallProject, RESTCallItem, CancellationToken.None).ConfigureAwait(false);
                ReceiveMessage = await responseMessage;
                if (!string.IsNullOrEmpty(ReceiveMessage))
                {
                    dynamic d = JObject.Parse(ReceiveMessage);
                    if (d.AccessToken != null)
                    {
                        AccessToken = d.AccessToken;
                    }
                }
            } );
            SaveRouteCommand = new RelayCommand(SaveRouteCommandExecute, o => true);
            SaveRESTCAllItemCommand = new RelayCommand(SaveRESTCAllItemCommandExecute, o => true);
            AllProjectNames = new ObservableCollection<string>();
            RESTCallItems = new ObservableCollection<RestApiCallItem>();
            SubFolder = "Projects";
            GetAllProjectNames();
            ProjectsIsVisible = true;
            IsProjectDeleteFieldEnable = true;
            IsProjectNewFieldEnable = true;
            IsProjectSaveFieldEnable = false;
        }

        //private async Task Send()
        //{
        //    var responseMessage = _manager.SendHttpRequest(AccessToken, RESTCallProject, RESTCallItem, CancellationToken.None).ConfigureAwait(false);
        //    ReceiveMessage = await responseMessage;
        //    if (!string.IsNullOrEmpty(ReceiveMessage))
        //    {
        //        dynamic d = JObject.Parse(ReceiveMessage);
        //        if (d.AccessToken != null)
        //        {
        //            AccessToken = d.AccessToken;
        //        }
        //    }
        //}

        private void SaveRESTCAllItemCommandExecute(object obj)
        {
            throw new NotImplementedException();
        }

        private void SaveRouteCommandExecute(object obj)
        {
            _manager.SaveProject(SubFolder, RESTCallProject);
        }

        private void SendMessage(Object obj)
        {
            Task.Run(async () => { await Send(); });
            //var responseMessage = _manager.SendHttpRequest(AccessToken, RESTCallProject, RESTCallItem, CancellationToken.None);
            //ReceiveMessage = await responseMessage;
            //if (!string.IsNullOrEmpty(ReceiveMessage))
            //{
            //    dynamic d = JObject.Parse(ReceiveMessage);
            //    if (d.AccessToken != null)
            //    {
            //        AccessToken = d.AccessToken;
            //    }
            //}
        }

        private async Task Send()
        {
            var responseMessage = _manager.SendHttpRequest(AccessToken, RESTCallProject, RESTCallItem, CancellationToken.None).ConfigureAwait(false);
            ReceiveMessage = await responseMessage;
            if (!string.IsNullOrEmpty(ReceiveMessage))
            {
                dynamic d = JObject.Parse(ReceiveMessage);
                if (d.AccessToken != null)
                {
                    AccessToken = d.AccessToken;
                }
            }
        }

        public ICommand ShowProjectsCommand { get; set; }

        public ICommand SendMessageCommand { get; set; }

        public ICommand SaveRouteCommand { get; set; }

        public ICommand SaveRESTCAllItemCommand { get; set; }

        public IEnumerable<string> HttpVerbsEnumValues => new List<string>{"GET", "POST"};

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

        public string SubFolder { get; set; }

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
            RESTCallProject = _manager.LoadProject(SubFolder, projectName);
            foreach (var restCallItem in RESTCallProject.Items)
            {
                RESTCallItems.Add(restCallItem);
            }

            RESTCallItem = RESTCallProject.Items.FirstOrDefault();
        }

        private void SaveProject()
        {
            _manager.SaveProject(SubFolder, RESTCallProject);
        }
    }
}
