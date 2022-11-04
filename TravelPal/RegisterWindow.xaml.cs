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

namespace TravelPal;
public partial class RegisterWindow : Window
{
    UserManager userManager = new();
    TravelManager travelManager = new();
    public RegisterWindow(UserManager userManager, TravelManager travelManager)
    {
        InitializeComponent();

        this.userManager = userManager;
        this.travelManager = travelManager;

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

    // Method to read input from the data fields in the RegisterWindow and creating a new user, called via a click action on the Register button:
    private void btnRegister_Click(object sender, RoutedEventArgs e)
    {
        string newUsername = txtUsername.Text;
        string newPassword = txtPassword.Text;
        string confirmPassword = txtConfirmPassword.Text;
        string newLocation = (string)cbLocations.SelectedItem;

        try
        {
            if (newUsername.Length < 3|| newUsername.Length > 12)
            {
                throw new ArgumentException("Username must be between 3-12 characters long!");
            }
            else if (newPassword.Length < 5)
            {
                throw new ArgumentException("Password must be at least 5 characters long!");
            }
            else if (newPassword != confirmPassword)
            {
                throw new ArgumentException("Confirm password doesn't match input in New password!");
            }
            else if (newLocation is null)
            {
                throw new ArgumentException("Please choose a country of origin!");
            }

            lblErrorMessage.Visibility = Visibility.Hidden;

            Countries locationEnum = (Countries)Enum.Parse(typeof(Countries), newLocation);

            User newUser = new(newUsername, newPassword, locationEnum);

            bool isValidNewUser = userManager.AddUser(newUser);

            if (isValidNewUser)
            {
                MessageBox.Show($"Success! {newUser.Username} registered as new user!", "Message", MessageBoxButton.OK, MessageBoxImage.Information);

                MainWindow mainWindow = new(userManager, travelManager);
                mainWindow.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show($"{newUser.Username} is already in use.\nPlease pick another username!", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtUsername.Clear();
            }

        }
        catch(ArgumentException ex)
        {
            lblErrorMessage.Content = ex.Message;
            lblErrorMessage.Visibility = Visibility.Visible;
        }
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new(userManager, travelManager);

        mainWindow.Show();
        
        this.Close();
    }

    // Method to make FontWeight bold upon mouse entering the area of the button: 
    private void btnCancel_MouseEnter(object sender, MouseEventArgs e)
    {
        btnCancel.FontWeight = FontWeights.Bold;
    }

    // Method to make FontWeight regular upon mouse leaving the area of the button:
    private void btnCancel_MouseLeave(object sender, MouseEventArgs e)
    {
        btnCancel.FontWeight = FontWeights.Regular;
    }

    // Method to make FontWeight bold upon mouse entering the area of the button: 
    private void btnRegister_MouseEnter(object sender, MouseEventArgs e)
    {
        btnRegister.FontWeight = FontWeights.Bold;
    }

    // Method to make FontWeight regular upon mouse leaving the area of the button:
    private void btnRegister_MouseLeave(object sender, MouseEventArgs e)
    {
        btnRegister.FontWeight = FontWeights.Regular;
    }
}
