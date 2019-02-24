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

        public string Authorization { get; set; }

        public List<RestApiCallItem> Items { get; set; }
    }
}
