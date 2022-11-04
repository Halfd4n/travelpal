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
    private User currentUser;

    public UserDetailsWindow(UserManager userManager)
    {
        InitializeComponent();
        
        this.userManager = userManager;
        currentUser = (User)userManager.SignedInUser;

        bool isAdmin = IsUserAdmin();

        if (isAdmin)
        {
            btnEditUser.IsEnabled = false;
        }
        else if (!isAdmin)
        {
            SetDataFields();
        }
    }

    // Method that clears the content of TextBox txtUsername, called upon via a double click on the textbox:
    private void txtUsername_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        txtUsername.Clear();
    }

    // Method to cancel editing user information, called from a click event on the Cancel button:
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    // Method to save the updated details from the user:
    private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
    {        
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
                throw new Exception("The input in password boxes don't match!");
            }

            lblErrorMessage.Visibility = Visibility.Hidden;
           
            Countries newCountryOfOriginEnum = (Countries)Enum.Parse(typeof(Countries), newCountryOfOrigin);

            bool isNewValidUserDetails = userManager.UpdateUser(newUsername);

            if (isNewValidUserDetails)
            {
                userManager.SignedInUser.Username = newUsername;
                userManager.SignedInUser.Password = newPassword;
                userManager.SignedInUser.Location = newCountryOfOriginEnum;

                MessageBox.Show("Success! Your details where updated.", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                lblErrorMessage.Content = $"{newUsername} is not available!";
                lblErrorMessage.Visibility = Visibility.Visible;
                txtUsername.Clear();
            }
        }
        catch(FormatException ex)
        {
            lblErrorMessage.Content = ex.Message;
            lblErrorMessage.Visibility = Visibility.Visible;
        }
        catch (ArgumentException ex)
        {
            lblErrorMessage.Content = ex.Message;
            lblErrorMessage.Visibility = Visibility.Visible;
        }
        catch(Exception ex)
        {
            lblErrorMessage.Content = ex.Message;
            lblErrorMessage.Visibility = Visibility.Visible;
        }
    }

    // Method to enable data fields, called upon from a click event in the Edit button: 
    private void btnEditUser_Click(object sender, RoutedEventArgs e)
    {
        txtUsername.IsEnabled = true;
        pswPassword.IsEnabled = true;
        pswConfirmPassword.IsEnabled = true;
        cbLocations.IsEnabled = true;

        btnEditUser.Visibility = Visibility.Hidden;
        btnSaveChanges.Visibility = Visibility.Visible;
    }

    // Method that fills the data fields with the users data and disableing interaction with the fields.
    private void SetDataFields()
    {
        txtUsername.IsEnabled = false;
        pswPassword.IsEnabled = false;
        pswConfirmPassword.IsEnabled = false;
        cbLocations.IsEnabled = false;

        txtUsername.Text = currentUser.Username;

        cbLocations.ItemsSource = Enum.GetNames(typeof(Countries));

        cbLocations.Text = currentUser.Location.ToString();
    }

    // Method that checks if the signed in user is an admin and returns a boolean value:
    private bool IsUserAdmin()
    {
        if (currentUser is Admin)
        {
            return true;
        }

        return false;
    }
}
