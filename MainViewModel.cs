using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using RDPShadow.Entities;
using RDPShadow.Services;
using TinyMVVM;

namespace RDPShadow
{
    public class MainViewModel : ObservableObject
    {
        private ObservableCollection<Computer> _computersSource;
        private ObservableCollection<Session> _sessionsSource;
        private readonly ComputerService _computerService;
        private readonly SessionService _sessionService;
        private readonly RDPService _rdpService;
        private ICollectionView _sessions;
        private ICollectionView _computers;
        private bool _enableSessionSelect;

        public MainViewModel(
            ComputerService computerService,
            SessionService sessionService,
            RDPService rdpService)
        {
            _computerService = computerService;
            _sessionService = sessionService;
            _rdpService = rdpService;

            Load();
        }

        public ICollectionView Computers
        {
            get => _computers;
            private set
            {
                _computers = value;
                OnPropertyChanged();
            }
        }

        public ICollectionView Sessions
        {
            get => _sessions;
            private set
            {
                _sessions = value;
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

        public void Load()
        {
            LoadComputers();
        }

        private async Task LoadSessionsAsync(Computer computer)
        {
            var remoteSessions = await _sessionService.GetRemoteSessionsAsync(computer);
            _sessionsSource = new ObservableCollection<Session>(remoteSessions);
            EnableSessionSelect = _sessionsSource.Any();
            Sessions = CollectionViewSource.GetDefaultView(_sessionsSource);
        }

        private void LoadComputers()
        {
            _computersSource = new ObservableCollection<Computer>(_computerService.Get());
            Computers = CollectionViewSource.GetDefaultView(_computersSource);
            Computers.SortDescriptions.Add(new SortDescription(nameof(Computer.DistinguishedName),
                ListSortDirection.Ascending));
            Computers.CurrentChanged += async (s, e)
                => await LoadSessionsAsync(Computers.CurrentItem as Computer);
        }

        public void ShadowConnect()
        {
            var computer = Computers.CurrentItem as Computer;
            var session = Sessions.CurrentItem as Session;
            if (session == null) return;
            _rdpService.ShadowConnect(computer, session);
        }

        public void Connect()
        {
            var computer = Computers.CurrentItem as Computer;
            _rdpService.Connect(computer);
        }
    }
}
