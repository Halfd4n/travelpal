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
using TravelPal.Interfaces;
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserManager userManager = new();

        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(UserManager userManager)
        {
            InitializeComponent();

            this.userManager = userManager;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            userManager.AllUsers = userManager.GetAllUsers();
            
            string username = txtUsername.Text;
            string password = pswPassword.Password;

            bool isSignedInUser = userManager.SignInUser(username, password);

            if (isSignedInUser)
            {
                TravelsWindow travelsWindow = new(userManager);

                travelsWindow.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show("Username or password is incorrect!", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new(userManager);

            registerWindow.Show();

            this.Close();
        }

        private void txtUsername_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtUsername.Clear();
        }

        private void lblPasswordWatermark_MouseDown(object sender, MouseButtonEventArgs e)
        {
            lblPasswordWatermark.Visibility = Visibility.Collapsed;
        }
    }
}
