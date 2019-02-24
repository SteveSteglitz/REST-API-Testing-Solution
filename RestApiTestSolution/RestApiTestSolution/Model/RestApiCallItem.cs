using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestApiTestSolution.Model
{
    public class RestApiCallItem
    {
        public int Id { get; set; }

        public HttpVerb HttpVerb { get; set; }

        public string Route { get; set; }

        public string Body { get; set; }
    }
}
