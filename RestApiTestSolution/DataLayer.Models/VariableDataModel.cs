namespace DataLayer.Models
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using DataLayer.Models.Annotations;

    public class VariableDataModel : INotifyPropertyChanged
    {
        private string _name;
        private string _value;

        public VariableDataModel(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public String Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public String Value
        {
            get => _value;
            set
            {
                _value = value;
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
