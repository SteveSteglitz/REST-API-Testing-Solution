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
using BusinessLayer.Implementations;
using DataLayer.Implementations;
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
        private BasicModusViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new BasicModusViewModel(new RestApiManager(new RestApiRepository()));
            this.DataContext = _viewModel;
        }
    }
}
