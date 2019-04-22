namespace BusinessLayer.Models
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using BusinessLayer.Models.Annotations;

    public class RouteBusinessModel : INotifyPropertyChanged
    {
        private string _body;
        private string _route;
        private string _httpVerb;
        public int Id { get; set; }

        public string HttpVerb
        {
            get => _httpVerb;
            set
            {
                _httpVerb = value;
                OnPropertyChanged();
            }
        }

        public string Route
        {
            get => _route ?? String.Empty;
            set
            {
                _route = value;
                OnPropertyChanged();
            }
        }

        public string Body
        {
            get => _body ?? String.Empty;
            set
            {
                _body = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
