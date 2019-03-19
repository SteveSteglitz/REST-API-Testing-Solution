using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
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
        private ApiRoute _selectedRoute;
        private ApiProject _selectedProject;
        private string _receiveMessage;
        private string _selectedProjectUrl;
        private string _selectedProjectName;
        private bool _isBusy;
        private string _receiveMessageStatusCode;
        private ApiVariable _selectedApiVariable;

        public RestApiViewModel()
        {
        }

        public RestApiViewModel(IRestApiManager manager)
        {
            _manager = manager;
            ShowProjectsCommand = new RelayCommand(o => { ProjectsIsVisible = ProjectsIsVisible != true; });
            SendMessageCommand = new RelayCommand(SendMessage, o => true);
            CreateRouteCommand = new RelayCommand(CreateRouteCommandExecute, o => true);
            DeleteRouteCommand = new RelayCommand(DeleteRouteCommandExecute, o => SelectedRoute != null);
            NewProjectCommand = new RelayCommand(NewProjectCommandExecute, o => true);
            SaveProjectCommand = new RelayCommand(SaveProjectCommandExecute, o => SelectedRoute != null || SelectedProject != null);
            DeleteProjectCommand = new RelayCommand(DeleteProjectCommandExecute, o => SelectedProject != null);
            MoveRouteItemDownCommand = new RelayCommand(MoveRouteItemDownCommandExecute, o => SelectedRoute != null);
            MoveRouteItemUpCommand = new RelayCommand(MoveRouteItemUpCommandExecute, o => SelectedRoute != null);
            CreateVariableCommand = new RelayCommand(CreateVariableCommandExecute);
            DeleteVariableCommand = new RelayCommand(DeleteVariableCommandExecute);
            AllProjectNames = new ObservableCollection<string>();
            ProjectUrls = new ObservableCollection<string>();
            SubFolder = "Projects";
            GetAllProjectNames();
            ProjectsIsVisible = true;
            IsProjectDeleteFieldEnable = true;
            IsProjectNewFieldEnable = true;
            IsProjectSaveFieldEnable = false;
            IsBusy = false;
        }


        #region Commands
        public ICommand ShowProjectsCommand { get; set; }

        public ICommand SendMessageCommand { get; set; }

        public ICommand CreateRouteCommand { get; set; }

        public ICommand DeleteRouteCommand { get; set; }

        public ICommand NewProjectCommand { get; set; }

        public ICommand SaveProjectCommand { get; set; }

        public ICommand DeleteProjectCommand { get; set; }

        public ICommand MoveRouteItemDownCommand { get; set; }

        public ICommand MoveRouteItemUpCommand { get; set; }

        public ICommand CreateVariableCommand { get; set; }

        public ICommand DeleteVariableCommand { get; set; }
        #endregion

        #region Properties
        public IEnumerable<string> HttpVerbsEnumValues => new List<string> { "GET", "POST", "PUT", "DELETE" };

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

        public ObservableCollection<Tuple<string, string>> Variables { get; set; }

        public string SelectedProjectUrl
        {
            get => _selectedProjectUrl;
            set
            {
                _selectedProjectUrl = value;
                OnPropertyChanged();
            }
        }

        public ApiProject SelectedProject
        {
            get => _selectedProject;
            set
            {
                _selectedProject = value;
                OnPropertyChanged();
            }
        }

        public ApiRoute SelectedRoute
        {
            get => _selectedRoute;
            set
            {
                _selectedRoute = value;
                OnPropertyChanged();
            }
        }

        public ApiVariable SelectedApiVariable
        {
            get => _selectedApiVariable;
            set
            {
                _selectedApiVariable = value; 
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

        public string ReceiveMessageStatusCode
        {
            get => _receiveMessageStatusCode;
            set
            {
                _receiveMessageStatusCode = value; 
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

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value; 
                OnPropertyChanged();
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

        #region CommandExecutes
        private void DeleteRouteCommandExecute(object obj)
        {
            SelectedProject.Items.Remove(SelectedRoute);
            SaveProject();
            SelectedRoute = SelectedProject.Items.FirstOrDefault();
        }

        private void CreateRouteCommandExecute(object obj)
        {
            SelectedRoute = new ApiRoute();
            SelectedProject.Items.Add(SelectedRoute);
        }

        private void NewProjectCommandExecute(object obj)
        {
            SelectedProject = new ApiProject
            {
                ProjectUrls = new ObservableCollection<string> { "https://", "https://", "https://" }
            };
        }

        private void SaveProjectCommandExecute(object obj)
        {
            if (SelectedRoute != null)
            {
                var itemIdx = SelectedProject.Items.IndexOf(SelectedRoute);
                var item = SelectedProject.Items[itemIdx];
                item.HttpVerb = SelectedRoute.HttpVerb;
                item.Route = SelectedRoute.Route;
                item.Body = SelectedRoute.Body;
            }

            _manager.SaveProject(SubFolder, SelectedProject);
            GetAllProjectNames();
        }

        private void MoveRouteItemUpCommandExecute(object obj)
        {
            var indexA = SelectedProject.Items.IndexOf(SelectedRoute);
            if (indexA > 1)
            {
                SelectedProject.Items.SwapItems(indexA - 1, indexA);
            }

            SelectedRoute = SelectedProject.Items[indexA - 1];
        }

        private void MoveRouteItemDownCommandExecute(object obj)
        {
            var indexA = SelectedProject.Items.IndexOf(SelectedRoute);
            if (indexA < SelectedProject.Items.Count)
            {
                SelectedProject.Items.SwapItems(indexA, indexA + 1);
            }
            SelectedRoute = SelectedProject.Items[indexA + 1];
        }

        private void CreateVariableCommandExecute(object obj)
        {
            SelectedApiVariable = new ApiVariable(String.Empty, String.Empty);
            SelectedProject.Variables.Add(SelectedApiVariable);
        }

        private void DeleteVariableCommandExecute(object obj)
        {
            SelectedProject.Variables.Remove(SelectedApiVariable);
            SelectedApiVariable = SelectedProject.Variables.FirstOrDefault();
        } 
        #endregion

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
            SelectedProject = _manager.LoadProject(SubFolder, projectName);
            SelectedRoute = SelectedProject.Items.FirstOrDefault();

            if (SelectedProject.ProjectUrls == null) return;

            ProjectUrls.Clear();
            foreach (var url in SelectedProject.ProjectUrls)
            {
                ProjectUrls.Add(url);
            }

            SelectedProjectUrl = ProjectUrls.First();
        }

        private void SaveProject()
        {
            _manager.SaveProject(SubFolder, SelectedProject);
            GetAllProjectNames();
        }

        private void DeleteProjectCommandExecute(object obj)
        {
            _manager.RemoveProject(SubFolder, SelectedProject);
            GetAllProjectNames();
        }

        private async void SendMessage(Object obj)
        {
            try
            {
                IsBusy = true;
                var responseMessage = await _manager.SendHttpRequest(AccessToken, SelectedProjectUrl, SelectedProject, SelectedRoute, CancellationToken.None);
                ReceiveMessage = responseMessage.Content?.ReadAsStringAsync().Result;
                ReceiveMessageStatusCode = $"{responseMessage.StatusCode.ToString()} ({(int)Enum.Parse(typeof(HttpStatusCode), responseMessage.StatusCode.ToString())})";
                SelectedProject.SaveVariableValueWhenFoundVariableName(ReceiveMessage);
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
            finally
            {
                IsBusy = false;
            }

        }
       
    }
}
