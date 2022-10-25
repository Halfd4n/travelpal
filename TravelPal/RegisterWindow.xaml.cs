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
using System.Windows.Shapes;
using TravelPal.Enums;
using TravelPal.Interfaces;
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private UserManager userManager = new();
        private List<IUser> allUsers = new();

        public RegisterWindow(UserManager userManager)
        {
            InitializeComponent();

            this.userManager = userManager;
            this.allUsers = userManager.GetAllUsers();

            string[] countries = Enum.GetNames(typeof(Countries));
            cbLocations.ItemsSource = countries;

        }
        
        // Method to reset content of TextBox on double click: 
        private void txtUsername_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtUsername.Clear();
        }

        // Method to reset content of TextBox on double click: 
        private void txtPassword_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Clear();
        }

        // Method to reset content of TextBox on double click: 
        private void txtConfirmPassword_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtConfirmPassword.Clear();
        }

    
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string newUsername = txtUsername.Text;
            string newPassword = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;
            string newLocation = (string)cbLocations.SelectedItem;

            try
            {
                if (newUsername.Length < 5 || newUsername.Length > 12)
                {
                    throw new FormatException("Username must be between 5-12 characters long!");
                }
                else if (newPassword != confirmPassword)
                {
                    throw new ArgumentException("Set password and Confirm password don't match!");
                }
                else if (String.IsNullOrEmpty(newLocation))
                {
                    throw new ArgumentException("Please choose a country of origin!");
                }

                lblErrorMessage.Visibility = Visibility.Hidden;

                Countries locationEnum = (Countries)Enum.Parse(typeof(Countries), newLocation);

                User newUser = new(newUsername, newPassword, locationEnum);

                bool isValidNewUser = userManager.AddUser(newUser);

                if (isValidNewUser)
                {
                    allUsers.Add(newUser);
                    MessageBox.Show($"Success! {newUser.Username} registered as new user!", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"{newUser.Username} is allready in use.\nPlease pick another username!", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    txtUsername.Clear();
                }

            }
            catch(FormatException ex)
            {
                lblErrorMessage.Content = ex.Message;
                lblErrorMessage.Visibility = Visibility.Visible;
            }
            catch(ArgumentException ex)
            {
                lblErrorMessage.Content = ex.Message;
                lblErrorMessage.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                lblErrorMessage.Content = ex.Message;
                lblErrorMessage.Visibility = Visibility.Visible;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();

            mainWindow.Show();
            
            this.Close();
        }

        private void btnCancel_MouseEnter(object sender, MouseEventArgs e)
        {
            btnCancel.FontWeight = FontWeights.Bold;
        }

        private void btnCancel_MouseLeave(object sender, MouseEventArgs e)
        {
            btnCancel.FontWeight = FontWeights.Regular;
        }

        private void btnRegister_MouseEnter(object sender, MouseEventArgs e)
        {
            btnRegister.FontWeight = FontWeights.Bold;
        }

        private void btnRegister_MouseLeave(object sender, MouseEventArgs e)
        {
            btnRegister.FontWeight = FontWeights.Regular;
        }
    }
}
