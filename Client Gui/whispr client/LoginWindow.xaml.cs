using System.Windows;
using System.Windows.Controls;
using Whispr;
using Whispr.Client.ResourceManagers;

namespace whispr_client
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private ClientProgram CLP;

        public LoginPage(ClientProgram cLP)
        {
            InitializeComponent();
            CLP = cLP;
         
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox.Clear();
        }        

        private bool LoginUser()
        {
            UserResourceManager.GetInstance().Username = UsernameTB.Text;
            CLP.Login(UsernameTB.Text, PasswordBox.Password);
            //CLP.RegisterPublicKey();
            //CLP.GetPublicKeys();
            //CLP.GetUsers();
            CLP.WaitForConnect();
            return true;
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (LoginUser())
                this.NavigationService.Navigate(new MainInterface(CLP));

        }
    }
}
