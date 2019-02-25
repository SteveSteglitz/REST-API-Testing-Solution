using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestApiTestSolution.Model
{
    public class RestApiCall
    {
        public string Project { get; set; }

        public string BaseUrl { get; set; }

        public string ContentType { get; set; }

        public string AuthorizationScheme { get; set; }

        public string AuthorizationParameter { get; set; }

        public List<RestApiCallItem> Items { get; set; }
    }
}
