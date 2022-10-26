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

        public TravelsWindow(UserManager userManager)
        {
            InitializeComponent();

            this.userManager = userManager;

            lblCurrentUser.Content = $"{userManager.SignedInUser.Username}";

            UpdateUI();
        }

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

        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {
            userManager.SignOutUser();

            MainWindow mainWindow = new(userManager);

            mainWindow.Show();

            this.Close();
        }

        private void btnMyDetails_Click(object sender, RoutedEventArgs e)
        {
            // Öppna nytt fönster som visar detaljer om den inloggade användare
        }

        private void btnAddTravel_Click(object sender, RoutedEventArgs e)
        {
            AddTravelWindow addTravelWindow = new(userManager, travelManager);

            addTravelWindow.Show();
        }

        private void btnRemoveTravel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnTravelDetails_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
