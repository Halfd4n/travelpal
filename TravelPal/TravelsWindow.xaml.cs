using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using TravelPal.Interfaces;
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal;
public partial class TravelsWindow : Window
{
    private UserManager userManager = new();
    private TravelManager travelManager = new();
    private Travel selectedTravel;
    private List<ListViewItem> travelsInListView = new();

    public TravelsWindow(UserManager userManager, TravelManager travelManager)
    {
        InitializeComponent();

        this.userManager = userManager;
        this.travelManager = travelManager;

        UpdateUI();
    }

    // Method to update UI of ListView lvTravels:
    private void UpdateUI()
    {
        lvTravels.ItemsSource = null;
        travelsInListView.Clear();

        travelManager.AllTravels = travelManager.GetAllTravels();

        lblCurrentUser.Content = "";
        lblCurrentUser.Content = $"{userManager.SignedInUser.Username}";

        if (userManager.SignedInUser is Admin)
        {
            Admin admin = (Admin)userManager.SignedInUser;

            foreach (Travel travel in travelManager.AllTravels)
            {
                ListViewItem item = new();

                item.Content = travel;
                item.Tag = travel;

                travelsInListView.Add(item);
            }
        }
        else if(userManager.SignedInUser is User)
        {
            User user = (User)userManager.SignedInUser;

            foreach (Travel travel in user.Travels)
            {
                ListViewItem item = new();

                item.Content = travel;
                item.Tag = travel;

                travelsInListView.Add(item);
            }
        }

        lvTravels.ItemsSource = travelsInListView;
    }

    // Method to sign out currently logged in user, called upon via click event on btnSignOut:
    private void btnSignOut_Click(object sender, RoutedEventArgs e)
    {
        userManager.SignOutUser();

        MainWindow mainWindow = new(userManager, travelManager);

        mainWindow.Show();

        this.Close();
    }

    // Method to open a window displaying the user details, called upon via click event on btnMyDetails:
    private void btnMyDetails_Click(object sender, RoutedEventArgs e)
    {
        UserDetailsWindow userDetailsWindow = new(userManager);

        userDetailsWindow.Show();

        userDetailsWindow.Closed += Window_Closed;
    }

    // Method to open AddTravelWindow, called upon by clicking on the Add Travel button:
    private void btnAddTravel_Click(object sender, RoutedEventArgs e)
    {
        AddTravelWindow addTravelWindow = new(userManager, travelManager);

        addTravelWindow.Show();

        addTravelWindow.Closed += Window_Closed;
    }

    // Method to remove a selected Travel object from the list:
    private void btnRemoveTravel_Click(object sender, RoutedEventArgs e)
    {

        if (selectedTravel == null)
        {
            MessageBox.Show("No travel currently selected. Select one and press Remove-button again if you wish to remove a travel from the list.", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to remove your travel to {selectedTravel.Destination}? This action can't be reversed.", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                travelManager.RemoveTravel(selectedTravel);
                
                foreach (IUser user in userManager.AllUsers)
                {
                    if(user is User)
                    {
                        User userTravel = (User)user;

                        if (userTravel.Travels.Contains(selectedTravel))
                        {
                            userTravel.Travels.Remove(selectedTravel);
                        }
                    }
                }

                UpdateUI();
                selectedTravel = null;

            }
        }
    }

    // Method to open TravelDetailsWindow, called via click action on the Travel Details button:
    private void btnTravelDetails_Click(object sender, RoutedEventArgs e)
    {
        if (selectedTravel == null)
        {
            MessageBox.Show("You haven't selected any travel from the list. Select one and press Details-button again to read details of the travel", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            TravelDetailsWindow travelDetailsWindow = new(userManager, travelManager, selectedTravel);

            travelDetailsWindow.Show();

            travelDetailsWindow.Closed += Window_Closed;
        }
    }

    // Method to open the HowToWindow, called via click action on the How To button:
    private void btnInfoHowTo_Click(object sender, RoutedEventArgs e)
    {
        HowToWindow howToWindow = new();

        howToWindow.Show();
    }

    // Method to notice selection changes in the lvTravels ListView:
    private void lvTravels_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ListViewItem selectedItem = (ListViewItem)lvTravels.SelectedItem;

        if (selectedItem is not null)
        {
            selectedTravel = (Travel)selectedItem.Tag;
        }
    }

    // Method to update the UI, activated when closing a window leading out from the TravelsWindow:
    private void Window_Closed(object sender, EventArgs e)
    {
        UpdateUI();
    }
}
