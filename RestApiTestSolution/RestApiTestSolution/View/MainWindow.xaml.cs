using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using RestApiTestSolution.Model;
using RestApiTestSolution.ViewModel;

namespace RestApiTestSolution.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private RestApiViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new RestApiViewModel(new RestApiManager(new RestApiRepository()));
            this.DataContext = _viewModel;
        }

        private void ProjectSetter_OnHandler(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                _viewModel.LoadProject(item.Content.ToString());
                _viewModel.ProjectsIsVisible = false;
            }
        }

        private void ListViewRESTCallItem_OnHandler(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                _viewModel.RESTCallItem = item.Content as RestApiCallItem;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ProjectsIsVisible = !_viewModel.ProjectsIsVisible;
        }
    }
}
