using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal;

public partial class UserDetailsWindow : Window
{
    private UserManager userManager = new();


    public UserDetailsWindow(UserManager userManager)
    {
        InitializeComponent();
        
        this.userManager = userManager;

        SetDataFields();
    }

    private void txtUsername_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        txtUsername.Clear();

    }

    // Method to cancel editing user information, called from a click event on Cancel button:
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
    {
        User currentUser = (User)userManager.SignedInUser;
        
        string newUsername = txtUsername.Text;
        string newPassword = pswPassword.Password;
        string newCountryOfOrigin = (string)cbLocations.SelectedItem;

        try
        {
            if (newUsername.Length < 3 || newUsername.Length > 12)
            {
                throw new FormatException("Username must be between 3-12 characters long");
            }
            else if (newPassword.Length < 5)
            {
                throw new ArgumentException("Password must be atleast 5 characters long");
            }
            else if (newPassword != pswConfirmPassword.Password)
            {
                throw new ArgumentException("The input in password boxes don't match!");
            }


            currentUser.Username = newUsername;
            currentUser.Password = newPassword;

            Countries newCountryOfOriginEnum = (Countries)Enum.Parse(typeof(Countries), newCountryOfOrigin);
        }
        catch
        {

        }
    }

    private void btnEditUser_Click(object sender, RoutedEventArgs e)
    {
        txtUsername.IsEnabled = true;
        pswPassword.IsEnabled = true;
        pswConfirmPassword.IsEnabled = true;
        cbLocations.IsEnabled = true;

        btnEditUser.Visibility = Visibility.Hidden;
        btnSaveChanges.Visibility = Visibility.Hidden;
    }

    private void SetDataFields()
    {
        txtUsername.IsEnabled = false;
        pswPassword.IsEnabled = false;
        pswConfirmPassword.IsEnabled = false;
        cbLocations.IsEnabled = false;

        User currentUser = (User)userManager.SignedInUser;

        txtUsername.Text = currentUser.Username;

        cbLocations.ItemsSource = Enum.GetNames(typeof(Countries));

        cbLocations.Text = currentUser.Location.ToString();
    }
}
