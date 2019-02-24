using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using RestApiTestSolution.Model;

namespace RestApiTestSolution.ViewModel
{
    public class RestApiViewModel : BaseViewModel
    {
        private IRestApiManager _manager;
        private bool _projectsIsVisible;

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
            AllProjectNames = new ObservableCollection<string>();
            RESTCallItems = new ObservableCollection<RestApiCallItem>();
            GetAllProjectNames();
            ProjectsIsVisible = true;
            IsProjectDeleteFieldEnable = true;
            IsProjectNewFieldEnable = true;
            IsProjectSaveFieldEnable = false;
        }


        public ICommand ShowProjectsCommand { get; set; }

        public ObservableCollection<string> AllProjectNames { get; set; }

        public ObservableCollection<RestApiCallItem> RESTCallItems { get; set; }

        public RestApiCall RESTCallProject { get; set; }

        public RestApiCallItem RESTCallItem { get; set; }

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
