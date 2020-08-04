using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Data;
using RDPShadow.Entities;
using TinyMVVM;

namespace RDPShadow
{
    public class MainModel : ObservableObject
    {
        private bool _enableSessionSelect;

        private ObservableCollection<Session> _sessions;
        private ObservableCollection<ComputerModel> _computers;

        private ICollectionView _computersView;
        private ICollectionView _sessionsView;
        private bool _allowControl;

        public MainModel()
        {
            Computers = new ObservableCollection<ComputerModel>();
            Sessions = new ObservableCollection<Session>();
        }


        public bool AllowControl
        {
            get => _allowControl;
            set
            {
                _allowControl = value;
                OnPropertyChanged();
            }
        }

        public ComputerModel SelectedComputer => ComputersView.CurrentItem as ComputerModel;
        public Session SelectedSession => SessionsView.CurrentItem as Session;

        public ICollectionView ComputersView
        {
            get => _computersView;
            set
            {
                _computersView = value;
                OnPropertyChanged();
            }
        }
        public ICollectionView SessionsView
        {
            get => _sessionsView;
            set
            {
                _sessionsView = value;
                OnPropertyChanged();
            }
        }
        public bool EnableSessionSelect
        {
            get => _enableSessionSelect;
            set
            {
                _enableSessionSelect = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ComputerModel> Computers
        {
            get => _computers;
            set 
            {
                if (NotNullGuard(value)) return;

                _computers = value;
                ComputersView = CollectionViewSource.GetDefaultView(_computers);

                OnPropertyChanged();
            }
        }

        public ObservableCollection<Session> Sessions
        {
            get => _sessions;
            set
            {
                if(NotNullGuard(value))return;

                _sessions = value;
                SessionsView = CollectionViewSource.GetDefaultView(_sessions);

                OnPropertyChanged();
            }
        }

        private bool NotNullGuard(object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(Computers));
            if (_computers == value) return true;
            return false;
        }
    }
}
