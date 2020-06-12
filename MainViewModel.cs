using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using RDPShadow.Entities;
using RDPShadow.Services;
using TinyMVVM;
using TinyMVVM.Commands;

namespace RDPShadow
{
    public class MainViewModel : ObservableObject
    {
        private ObservableCollection<Computer> _computersSource;
        private ObservableCollection<Session> _sessionsSource;
        private readonly RDPService _rdpService;
        private ICollectionView _sessions;
        private ICollectionView _computers;
        private bool _enableSessionSelect;
        private Computer _selectedComputer;
        private Session _selectedSession;

        public MainViewModel(RDPService rdpService)
        {
            _rdpService = rdpService;
            ConnectCommand = new RelayCommand(Connect, CanConnect);
            ShadowConnectCommand = new RelayCommand(ShadowConnect, CanShadowConnect);

            Load();
        }

        public MainModel Model { get; }  = new MainModel();

        public ICommand ConnectCommand { get; }

        public ICommand ShadowConnectCommand { get; }



        public void Load()
        {
            LoadComputers();
        }

        private async Task LoadSessionsAsync(Computer computer)
        {
            var remoteSessions = await _rdpService.GetRemoteSessionsAsync(computer);
            Model.Sessions = new ObservableCollection<Session>(remoteSessions);
            Model.EnableSessionSelect = Model.Sessions.Any();
        }

        private void LoadComputers()
        {
            Model.Computers = new ObservableCollection<Computer>(_rdpService.GetComputers());
            Model.ComputersView.SortDescriptions.Add(new SortDescription(nameof(Computer.DistinguishedName),
                ListSortDirection.Ascending));
            Model.ComputersView.CurrentChanged += async (s, e)
                => await LoadSessionsAsync(Model.SelectedComputer);
        }

        private void ShadowConnect()
        {
            _rdpService.ShadowConnect(Model.SelectedComputer, Model.SelectedSession);
        }

        private bool CanShadowConnect()
        {
            return Model.SelectedComputer != null &&
                   Model.SelectedSession != null;
        }

        private void Connect()
        {
            _rdpService.Connect(Model.SelectedComputer);
        }

        private bool CanConnect()
        {
            return Model.SelectedComputer != null;
        }
    }
}
