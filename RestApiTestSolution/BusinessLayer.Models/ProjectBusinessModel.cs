namespace BusinessLayer.Models
{
    using System.Collections.ObjectModel;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class ProjectBusinessModel
    {
        private ObservableCollection<VariableBusinessModel> _variables;
        public string Project { get; set; }

        public ObservableCollection<string> ProjectUrls { get; set; }

        public string ContentType { get; set; }

        public string AuthorizationScheme { get; set; }

        public string AuthorizationParameter { get; set; }

        public string Description { get; set; }

        public ObservableCollection<RouteBusinessModel> Items { get; set; }

        public ObservableCollection<VariableBusinessModel> Variables
        {
            get => _variables ?? (_variables = new ObservableCollection<VariableBusinessModel>());
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
