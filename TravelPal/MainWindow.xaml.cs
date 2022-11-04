using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

namespace TravelPal;
public partial class MainWindow : Window
{
    private UserManager userManager = new();
    private TravelManager travelManager = new();

    public MainWindow()
    {
        InitializeComponent();

        PopulateAllTravels();
    }
    public MainWindow(UserManager userManager, TravelManager travelManager)
    {
        InitializeComponent();

        this.userManager = userManager;
        this.travelManager = travelManager;
    }

    // Method to log in user based on credentials, called via a click event on the Login button:
    private void btnLogin_Click(object sender, RoutedEventArgs e)
    {
        userManager.AllUsers = userManager.GetAllUsers();
        
        string username = txtUsername.Text;
        string password = pswPassword.Password;

        bool isSignedInUser = userManager.SignInUser(username, password);

        if (isSignedInUser)
        {
            TravelsWindow travelsWindow = new(userManager, travelManager);

            travelsWindow.Show();

            this.Close();
        }
        else
        {
            MessageBox.Show("Username or password is incorrect!", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
        }
       
    }

    // Method to open RegisterWindow, called via click on the Register button:
    private void btnRegister_Click(object sender, RoutedEventArgs e)
    {
        RegisterWindow registerWindow = new(userManager, travelManager);

        registerWindow.Show();

        this.Close();
    }

    // Method to clear the txtUsername TextBox, called via a double click action on the textbox:
    private void txtUsername_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        txtUsername.Clear();
    }

    // Method to clear the lvlPasswordWatermark Label, called via a click action on the label:
    private void lblPasswordWatermark_MouseDown(object sender, MouseButtonEventArgs e)
    {
        lblPasswordWatermark.Visibility = Visibility.Collapsed;
    }

    // Method to populate the default Travel objects into the AllTravels list:
    private void PopulateAllTravels()
    {
        foreach(IUser defaultUser in userManager.AllUsers)
        {
            if(defaultUser is User)
            {
                User user = (User)defaultUser;

                foreach(Travel travel in user.Travels)
                {
                    travelManager.AddTravel(travel);
                }
            }
        }
    }

    // Method to read Enter button on keyboard to be able to click Login button without using mouse.
    private void pswPassword_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            btnLogin_Click(sender, e);
        }
    }
}
