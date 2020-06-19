using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using RDPShadow.Entities;
using RDPShadow.Services;
using TinyMVVM;
using TinyMVVM.Commands;

namespace RDPShadow
{
    public class MainViewModel : ObservableObject
    {
        private readonly RDPService _rdpService;

        public MainViewModel(RDPService rdpService)
        {
            _rdpService = rdpService;
            ConnectCommand = new RelayCommand(Connect, CanConnect);
            ShadowConnectCommand = new RelayCommand(ShadowConnect, CanShadowConnect);
        }

        public MainModel Model { get; }  = new MainModel();

        public ICommand ConnectCommand { get; }

        public ICommand ShadowConnectCommand { get; }

        private async Task LoadSessionsAsync()
        {
            Model.EnableSessionSelect = false;
            Model.Sessions.Clear();

            var queryComputer = Model.SelectedComputer;
            var previousStatus = queryComputer.Status;
            queryComputer.Status = LoadingStatus.Loading;
            var remoteSessions = await _rdpService.GetRemoteSessionsAsync(queryComputer.Computer);
            if (Model.SelectedComputer != queryComputer)
            {
                queryComputer.Status = previousStatus;
                return;
            }

            Model.Sessions = new ObservableCollection<Session>(remoteSessions);
            var hasSessions = Model.Sessions.Any();
            Model.EnableSessionSelect = hasSessions;
            queryComputer.Status = hasSessions ? LoadingStatus.Done : LoadingStatus.Failed;
        }

        public void Load()
        {
            LoadComputers();
        }

        private void LoadComputers()
        {
            IEnumerable<Computer> computers = Enumerable.Empty<Computer>();
            try
            {
                computers = _rdpService.GetComputers();
            }
            catch
            {
                Dispatcher.CurrentDispatcher.Invoke(() =>
                    MessageBox.Show("Error loading computers. Check configuration"));
            }

            Model.Computers = new ObservableCollection<ComputerModel>(computers
                .Select(x=>new ComputerModel(x)));
            Model.ComputersView.SortDescriptions.Add(new SortDescription(nameof(ComputerModel.DistinguishedName),
                ListSortDirection.Ascending));
            Model.ComputersView.CurrentChanged += async (s, e)
                => await LoadSessionsAsync();
        }

        private void ShadowConnect()
        {
            _rdpService.ShadowConnect(Model.SelectedComputer.Computer, Model.SelectedSession);
        }

        private bool CanShadowConnect()
        {
            return Model.SelectedComputer != null &&
                   Model.SelectedSession != null;
        }

        private void Connect()
        {
            _rdpService.Connect(Model.SelectedComputer.Computer);
        }

        private bool CanConnect()
        {
            return Model.SelectedComputer != null;
        }
    }
}
