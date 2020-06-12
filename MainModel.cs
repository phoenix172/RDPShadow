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
        private ObservableCollection<Computer> _computers;

        private ICollectionView _computersView;
        private ICollectionView _sessionsView;

        public MainModel()
        {
            Computers = new ObservableCollection<Computer>();
            Sessions = new ObservableCollection<Session>();
        }

        public Computer SelectedComputer => ComputersView.CurrentItem as Computer;
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

        public ObservableCollection<Computer> Computers
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
