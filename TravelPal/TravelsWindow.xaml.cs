﻿using System;
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
using TravelPal.Interfaces;
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for TravelsWindow.xaml
    /// </summary>
    public partial class TravelsWindow : Window
    {
        private UserManager userManager = new();
        private TravelManager travelManager = new();
        private Travel selectedTravel;

        public TravelsWindow(UserManager userManager, TravelManager travelManager)
        {
            InitializeComponent();

            this.userManager = userManager;
            this.travelManager = travelManager;

            lblCurrentUser.Content = $"{userManager.SignedInUser.Username}";

            UpdateUI();
        }

        // Method to update UI of ListView lvTravels:
        private void UpdateUI()
        {
            lvTravels.Items.Clear();
            travelManager.AllTravels = travelManager.GetAllTravels();

            if (userManager.SignedInUser is Admin)
            {
                Admin admin = (Admin)userManager.SignedInUser;

                foreach (Travel travel in travelManager.AllTravels)
                {
                    ListViewItem item = new();

                    item.Content = travel.GetInfo();
                    item.Tag = travel;

                    lvTravels.Items.Add(item);
                }
            }
            else if(userManager.SignedInUser is User)
            {
                User user = (User)userManager.SignedInUser;

                foreach (Travel travel in user.Travels)
                {
                    ListViewItem item = new();

                    item.Content = travel.GetInfo();
                    item.Tag = travel;

                    lvTravels.Items.Add(item);
                }
            }
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
            // Öppna nytt fönster som visar detaljer om den inloggade användare
        }

        private void btnAddTravel_Click(object sender, RoutedEventArgs e)
        {
            AddTravelWindow addTravelWindow = new(userManager, travelManager);

            addTravelWindow.Show();

            addTravelWindow.Closed += AddTravelWindow_Closed;
        }

        private void AddTravelWindow_Closed(object sender, EventArgs e)
        {
            UpdateUI();
        }

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
            }
        }

        private void btnInfoHowTo_Click(object sender, RoutedEventArgs e)
        {
            HowToWindow howToWindow = new();

            howToWindow.Show();
        }

        private void lvTravels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewItem selectedItem = (ListViewItem)lvTravels.SelectedItem;

            if (selectedItem != null)
            {
                selectedTravel = (Travel)selectedItem.Tag;
            }
        }
    }
}
