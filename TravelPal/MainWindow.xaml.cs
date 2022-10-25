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
        private List<IUser> allUsers = new();
        UserManager userManager = new();

        public MainWindow()
        {
            InitializeComponent();

            userManager.SeedDefaultUsers();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            allUsers = userManager.GetAllUsers();
            
            string username = txtUsername.Text;
            string password = pswPassword.Password;

            bool isFoundUser = false;

            foreach (IUser user in allUsers)
            {
                if (user.Username == username && user.Password == password)
                {
                    isFoundUser = true;

                    if (user is User)
                    { 
                        TravelsWindow travelsWindow = new(user, userManager);

                        travelsWindow.Show();
                    }
                    else if(user is Admin)
                    {
                        TravelsWindow travelsWindow = new(user, userManager);

                        travelsWindow.Show();
                    }
                }
            }

            if (!isFoundUser)
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
