using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RestApiTestSolution.Annotations;

namespace RestApiTestSolution.Model
{
    public class RestApiCall
    {
        public string Project { get; set; }

        public ObservableCollection<string> ProjectUrls { get; set; }

        public string ContentType { get; set; }

        public string AuthorizationScheme { get; set; }

        public string AuthorizationParameter { get; set; }

        public string Description { get; set; }

        public ObservableCollection<RestApiCallItem> Items { get; set; }
    }
}
