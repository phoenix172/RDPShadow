using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.Configuration;
using RDPShadow.Services;

namespace RDPShadow
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainViewModel _viewModel;
        public App()
        {
            _viewModel = Bootstrap();
        }

        private static MainViewModel Bootstrap()
        {
            ConfigurationBuilder configBuilder = new ConfigurationBuilder();

            IConfiguration config = configBuilder
                .AddJsonFile("appconfig.json")
                .AddJsonFile("appconfig.Development.json", true)
                .Build();

            string domain = config["domain"];
            string containerDc = config["containerDc"];

            var computerService = new ComputerService(domain, containerDc);
            var sessionService = new SessionService();
            var rdpService = new RDPService();

            return new MainViewModel(computerService, sessionService, rdpService);
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            MessageBox.Show($"An error occured: {e.Exception.Message}","RDPShadow", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown(1);
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = new MainWindow(_viewModel);
            mainWindow.Show();
        }
    }
}
