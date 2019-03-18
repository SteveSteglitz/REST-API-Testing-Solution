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
        private ApiRoute _route;
        private ApiProject _projectProject;
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
            DeleteRouteCommand = new RelayCommand(DeleteRouteCommandExecute, o => _route != null);
            NewProjectCommand = new RelayCommand(NewProjectCommandExecute, o => true);
            SaveProjectCommand = new RelayCommand(SaveProjectCommandExecute, o => Route != null || ProjectProject != null);
            DeleteProjectCommand = new RelayCommand(DeleteProjectCommandExecute, o => ProjectProject != null);
            MoveRouteItemDownCommand = new RelayCommand(MoveRouteItemDownCommandExecute, o => Route != null);
            MoveRouteItemUpCommand = new RelayCommand(MoveRouteItemUpCommandExecute, o => Route != null);
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

        public ApiProject ProjectProject
        {
            get => _projectProject;
            set
            {
                _projectProject = value;
                OnPropertyChanged();
            }
        }

        public ApiRoute Route
        {
            get => _route;
            set
            {
                _route = value;
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
            ProjectProject.Items.Remove(Route);
            SaveProject();
            Route = ProjectProject.Items.FirstOrDefault();
        }

        private void CreateRouteCommandExecute(object obj)
        {
            Route = new ApiRoute();
            ProjectProject.Items.Add(Route);
        }

        private void NewProjectCommandExecute(object obj)
        {
            ProjectProject = new ApiProject
            {
                ProjectUrls = new ObservableCollection<string> { "https://", "https://", "https://" }
            };
        }

        private void SaveProjectCommandExecute(object obj)
        {
            if (Route != null)
            {
                var itemIdx = ProjectProject.Items.IndexOf(Route);
                var item = ProjectProject.Items[itemIdx];
                item.HttpVerb = Route.HttpVerb;
                item.Route = Route.Route;
                item.Body = Route.Body;
            }

            _manager.SaveProject(SubFolder, ProjectProject);
            GetAllProjectNames();
        }

        private void MoveRouteItemUpCommandExecute(object obj)
        {
            var indexA = ProjectProject.Items.IndexOf(Route);
            if (indexA > 1)
            {
                ProjectProject.Items.SwapItems(indexA - 1, indexA);
            }

            Route = ProjectProject.Items[indexA - 1];
        }

        private void MoveRouteItemDownCommandExecute(object obj)
        {
            var indexA = ProjectProject.Items.IndexOf(Route);
            if (indexA < ProjectProject.Items.Count)
            {
                ProjectProject.Items.SwapItems(indexA, indexA + 1);
            }
            Route = ProjectProject.Items[indexA + 1];
        }

        private void CreateVariableCommandExecute(object obj)
        {
            SelectedApiVariable = new ApiVariable(String.Empty, String.Empty);
            ProjectProject.Variables.Add(SelectedApiVariable);
        }

        private void DeleteVariableCommandExecute(object obj)
        {
            ProjectProject.Variables.Remove(SelectedApiVariable);
            SelectedApiVariable = ProjectProject.Variables.FirstOrDefault();
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
            ProjectProject = _manager.LoadProject(SubFolder, projectName);
            Route = ProjectProject.Items.FirstOrDefault();

            if (ProjectProject.ProjectUrls == null) return;

            ProjectUrls.Clear();
            foreach (var url in ProjectProject.ProjectUrls)
            {
                ProjectUrls.Add(url);
            }

            SelectedProjectUrl = ProjectUrls.First();
        }

        private void SaveProject()
        {
            _manager.SaveProject(SubFolder, ProjectProject);
            GetAllProjectNames();
        }

        private void DeleteProjectCommandExecute(object obj)
        {
            _manager.RemoveProject(SubFolder, ProjectProject);
            GetAllProjectNames();
        }

        private async void SendMessage(Object obj)
        {
            try
            {
                IsBusy = true;
                var responseMessage = await _manager.SendHttpRequest(AccessToken, SelectedProjectUrl, ProjectProject,
                    Route, CancellationToken.None);
                ReceiveMessage = responseMessage.Content?.ReadAsStringAsync().Result;
                ReceiveMessageStatusCode = $"{responseMessage.StatusCode.ToString()} ({(int)Enum.Parse(typeof(HttpStatusCode), responseMessage.StatusCode.ToString())})";
                ProjectProject.SaveVariableValueWhenFoundVariableName(ReceiveMessage);
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
