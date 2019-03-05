using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using RestApiTestSolution.Model;

namespace RestApiTestSolution.ViewModel
{
    public class RestApiViewModel : BaseViewModel
    {
        private readonly IRestApiManager _manager;
        private bool _projectsIsVisible;
        private RestApiCallItem _restCallItem;
        private RestApiCall _restCallProject;
        private string _receiveMessage;
        private string _selectedProjectUrl;
        private string _selectedProjectName;

        public RestApiViewModel()
        {
        }

        public RestApiViewModel(IRestApiManager manager)
        {
            _manager = manager;
            ShowProjectsCommand = new RelayCommand(o => { ProjectsIsVisible = ProjectsIsVisible != true; });
            SendMessageCommand = new RelayCommand(SendMessage, o => true);
            CreateRouteCommand = new RelayCommand(CreateRouteCommandExecute, o => true);
            DeleteRouteCommand = new RelayCommand(DeleteRouteCommandExecute, o => _restCallItem != null);
            NewProjectCommand = new RelayCommand(NewProjectCommandExecute, o => true);
            SaveProjectCommand = new RelayCommand(SaveProjectCommandExecute, o => RESTCallItem != null || RESTCallProject != null);
            DeleteProjectCommand = new RelayCommand(DeleteProjectCommandExecute, o => RESTCallProject != null);
            AllProjectNames = new ObservableCollection<string>();
            ProjectUrls = new ObservableCollection<string>();
            SubFolder = "Projects";
            GetAllProjectNames();
            ProjectsIsVisible = true;
            IsProjectDeleteFieldEnable = true;
            IsProjectNewFieldEnable = true;
            IsProjectSaveFieldEnable = false;
        }


        #region Commands
        public ICommand ShowProjectsCommand { get; set; }

        public ICommand SendMessageCommand { get; set; }

        public ICommand CreateRouteCommand { get; set; }

        public ICommand DeleteRouteCommand { get; set; }

        public ICommand NewProjectCommand { get; set; }

        public ICommand SaveProjectCommand { get; set; }

        public ICommand DeleteProjectCommand { get; set; }
        #endregion

        #region Properties
        public IEnumerable<string> HttpVerbsEnumValues => new List<string> { "GET", "POST" };

        public ObservableCollection<string> AllProjectNames { get; set; }

        public string SelectedProjectName
        {
            get => _selectedProjectName;
            set
            {
                _selectedProjectName = value;
                if (value != null)
                {
                    LoadProject(value);
                }
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> ProjectUrls { get; set; }

        public string SelectedProjectUrl
        {
            get => _selectedProjectUrl;
            set
            {
                _selectedProjectUrl = value;
                OnPropertyChanged();
            }
        }

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
            get => _receiveMessage;
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

        #region Visual State Properties
        public bool IsProjectNewFieldEnable { get; set; }

        public bool IsProjectDeleteFieldEnable { get; set; }

        public bool IsProjectSaveFieldEnable { get; set; }

        public bool IsRouteDeleteFieldEnable { get; set; }


        public bool IsRouteSaveFieldEnable { get; set; }  
        #endregion
        
        #endregion

        private void DeleteRouteCommandExecute(object obj)
        {
            RESTCallProject.Items.Remove(RESTCallItem);
            SaveProject();
            RESTCallItem = RESTCallProject.Items.FirstOrDefault();
        }

        private void CreateRouteCommandExecute(object obj)
        {
            RESTCallItem = new RestApiCallItem();
            RESTCallProject.Items.Add(RESTCallItem);
        }

        private void NewProjectCommandExecute(object obj)
        {
            RESTCallProject = new RestApiCall
            {
                ProjectUrls = new ObservableCollection<string> {"https://", "https://", "https://"}
            };
        }

        private void SaveProjectCommandExecute(object obj)
        {
            if (RESTCallItem != null)
            {
                var itemIdx = RESTCallProject.Items.IndexOf(RESTCallItem);
                var item = RESTCallProject.Items[itemIdx];
                item.HttpVerb = RESTCallItem.HttpVerb;
                item.Route = RESTCallItem.Route;
                item.Body = RESTCallItem.Body;
            }
            
            _manager.SaveProject(SubFolder, RESTCallProject);
            GetAllProjectNames();
        }

        private void GetAllProjectNames()
        {
            AllProjectNames.Clear();
            foreach (var project in _manager.GetAllProjects(@"Projects\"))
            {
                AllProjectNames.Add(project);
            }
        }

        private void LoadProject(string projectName)
        {
            RESTCallProject = _manager.LoadProject(SubFolder, projectName);
            RESTCallItem = RESTCallProject.Items.FirstOrDefault();

            if (RESTCallProject.ProjectUrls == null) return;

            ProjectUrls.Clear();
            foreach (var url in RESTCallProject.ProjectUrls)
            {
                ProjectUrls.Add(url);
            }

            SelectedProjectUrl = ProjectUrls.First();
        }

        private void SaveProject()
        {
            _manager.SaveProject(SubFolder, RESTCallProject);
            GetAllProjectNames();
        }

        private void DeleteProjectCommandExecute(object obj)
        {
            _manager.RemoveProject(SubFolder, RESTCallProject);
            GetAllProjectNames();
        }

        private async void SendMessage(Object obj)
        {
            try
            {
                var responseMessage = await _manager.SendHttpRequest(AccessToken, SelectedProjectUrl, RESTCallProject,
                    RESTCallItem, CancellationToken.None);
                ReceiveMessage = responseMessage;
                if (!string.IsNullOrEmpty(ReceiveMessage) && responseMessage.Contains("AccessToken"))
                {
                    dynamic d = JObject.Parse(ReceiveMessage);
                    if (d.AccessToken != null)
                    {
                        AccessToken = d.AccessToken;
                    }
                }
            }
            catch (System.Net.Http.HttpRequestException exception)
            {
                var sb = new StringBuilder();
                sb.AppendLine(exception.Message);
                sb.AppendLine("");
                sb.AppendLine("===========================");
                sb.AppendLine("Details:");
                if (exception.InnerException != null)
                {
                    sb.AppendLine(exception.InnerException.Message);
                }

                ReceiveMessage = sb.ToString();
            }

        }
       
    }
}
