using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestApiTestSolution.Annotations;

namespace RestApiTestSolution.Model
{
    public class RestApiCall
    {
        private ObservableCollection<ApiVariable> _variables;
        public string Project { get; set; }

        public ObservableCollection<string> ProjectUrls { get; set; }

        public string ContentType { get; set; }

        public string AuthorizationScheme { get; set; }

        public string AuthorizationParameter { get; set; }

        public string Description { get; set; }

        public ObservableCollection<RestApiCallItem> Items { get; set; }

        public ObservableCollection<ApiVariable> Variables
        {
            get => _variables ?? (_variables = new ObservableCollection<ApiVariable>());
            set => _variables = value;
        }

        public string GetStringReplacedWithVariable(string inputString)
        {
            foreach (var variable in Variables)
            {
                if (inputString.Contains(variable.Name))
                {
                    return inputString.Replace(variable.Name, variable.Value);
                }
            }
            return inputString;
        }

        public void SaveVariableValueWhenFoundVariableName(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                return;
            }

            foreach (var variable in Variables)
            {
                var varName = variable.Name.Replace("$", "");
                if (inputString.Contains(varName))
                {
                    dynamic dynObj = JsonConvert.DeserializeObject(inputString);

                    //JContainer is the base class
                    var jObj = (JObject)dynObj;

                    foreach (JToken token in jObj.Children())
                    {
                        if (token is JProperty)
                        {
                            var prop = token as JProperty;
                            if (prop.Name.Equals(varName))
                            {
                                variable.Value = prop.Value.ToString();
                            }
                        }
                    }
                }
            }
        }
    }
}
