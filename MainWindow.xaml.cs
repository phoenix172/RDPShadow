using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RDPShadow.Entities;

namespace RDPShadow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = _viewModel = viewModel;
        }

        private void SessionItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _viewModel.ShadowConnect();
        }

        private void ComputerItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _viewModel.Connect();
        }
    }
}
