using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using RDPShadow.Annotations;
using RDPShadow.Entities;

namespace RDPShadow
{
    public class ComputerModel : INotifyPropertyChanged
    {
        private LoadingStatus _status;

        public ComputerModel(Computer computer)
        {
            Computer = computer;
        }

        public Computer Computer { get; }
        public string Name => Computer.Name;
        public string DistinguishedName => Computer.DistinguishedName;

        public LoadingStatus Status
        {
            get => _status;
            set
            {
                if (value == _status) return;
                _status = value;
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

    public enum LoadingStatus
    {
        None,
        Loading,
        Failed,
        Done
    }
}
