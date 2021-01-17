using System;
using System.Windows;
using Whispr;

namespace whispr_client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ClientProgram CLP = new ClientProgram();

        public MainWindow()
        {
            InitializeComponent();

            Loaded += LoadedWindows;
        }

        private void LoadedWindows(object sender, RoutedEventArgs e)
        {
            //MainFrame.NavigationService.Navigate(new MainInterface());
            MainFrame.NavigationService.Navigate(new LoginPage(CLP));
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            CLP.EndConnection();
        }
    }
}
