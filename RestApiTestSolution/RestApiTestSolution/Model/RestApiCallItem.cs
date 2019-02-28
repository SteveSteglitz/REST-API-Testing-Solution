using System;
using System.Collections.Generic;
using System.ComponentModel;
namespace RestApiTestSolution.Model
{
    public class RestApiCallItem 
    {
        public int Id { get; set; }

        public string HttpVerb { get; set; }

        public string Route { get; set; }

        public string Body { get; set; }
    }
}
