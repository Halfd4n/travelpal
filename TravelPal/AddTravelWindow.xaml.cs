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
    /// Interaction logic for AddTravelWindow.xaml
    /// </summary>

    public partial class AddTravelWindow : Window
    {
        private UserManager userManager = new();
        private TravelManager travelManager = new();
        private ItemManager itemManager = new();
        private IPackingListItem packingListItem;
        private int arrayIndex;
        private User currentUser;

        public AddTravelWindow(UserManager userManager, TravelManager travelManager)
        {
            InitializeComponent();

            this.userManager = userManager;
            this.travelManager = travelManager;

            string[] countries = Enum.GetNames(typeof(Countries));
            cbCountry.ItemsSource = countries;

            string[] tripOrVacation = new string[2] { "Trip", "Vacation" };
            cbTripOrVacation.ItemsSource = tripOrVacation;

            string[] tripType = new string[2] { "Leisure", "Occupational" };
            cbTripType.ItemsSource = tripType;

            if(userManager.SignedInUser is User)
            {
                currentUser = (User)userManager.SignedInUser;
            }

            UpdateListView();
        }

        private void UpdateListView()
        {
            lvTravelItems.Items.Clear();

            itemManager.AllPackingListItems = itemManager.GetAllPackingListItems();

            foreach (IPackingListItem packingItem in itemManager.AllPackingListItems)
            {
                if (packingItem is TravelDocument)
                {
                    ListViewItem item = new();

                    item.Content = packingItem.GetInfo();
                    item.Tag = packingItem;

                    lvTravelItems.Items.Add(item);
                }
                else if(packingItem is OtherItem)
                {
                    ListViewItem item = new();
                    item.Content = packingItem.GetInfo();
                    item.Tag = packingItem;

                    lvTravelItems.Items.Add(item);
                }
            }
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            AddNewItemWindow addNewItemWindow = new(itemManager);

            addNewItemWindow.Show();

            addNewItemWindow.Closed += AddNewItemWindow_Closed;
        }

        private void AddNewItemWindow_Closed(object? sender, EventArgs e)
        {
            UpdateListView();
        }

        private void btnRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (packingListItem == null)
            {
                MessageBox.Show("No item selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                itemManager.RemoveItem(packingListItem);

                UpdateListView();
            }
        }

        private void btnAddTravel_Click(object sender, RoutedEventArgs e)
        {

            string destination = txtDestination.Text;
            string country = (string)cbCountry.SelectedItem;
            string tripOrVacation = (string)cbTripOrVacation.SelectedItem;
            string tripType = (string)cbTripType.SelectedItem;
            bool isAllInclusive = (bool)xbAllInclusive.IsChecked;
            string noOfTravelers = txtTravelerAmount.Text;

            string[] textInputsToTry = new string[4] {destination, country, tripOrVacation, noOfTravelers};
            arrayIndex = 0;

            try
            {
                foreach (string input in textInputsToTry)
                {
                    if (String.IsNullOrEmpty(input))
                    {
                        throw new FormatException("The form wasn't filled in correctly.");
                    }
                    
                    arrayIndex++;
                }

                Countries countryEnum = (Countries)Enum.Parse(typeof(Countries), country);

                if (tripOrVacation == "Trip")
                {
                    if (String.IsNullOrEmpty(tripType))
                    {
                        throw new Exception("The form wasn't filled in correctly.");
                    }
                    
                    TripTypes tripTypeEnum = (TripTypes)Enum.Parse(typeof(TripTypes), tripType);

                    bool isInteger = int.TryParse(noOfTravelers, out int travelersInteger);

                    if (isInteger)
                    {
                        DateTime startDate = (DateTime)dpTravelStart.SelectedDate;

                        DateTime endDate = (DateTime)dpTravelEnd.SelectedDate;

                        int travelDays = (int)(endDate - startDate).TotalDays;

                        Trip newTrip = new(destination, countryEnum, travelersInteger, itemManager.AllPackingListItems, startDate, endDate, travelDays, tripTypeEnum);

                        travelManager.AddTravel(newTrip);

                        currentUser.Travels.Add(newTrip);

                    }
                    else
                    {
                        MessageBox.Show("Number of travelers wasn't given in an integer! Please try again", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                        txtTravelerAmount.Clear();
                    }

                }
                else if (tripOrVacation == "Vacation")
                {

                }


            }
            catch (FormatException ex)
            {
                switch (arrayIndex)
                {
                    case 0:
                        {
                            MessageBox.Show($"{ex.Message} Please input a destination.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    case 1:
                        {
                            MessageBox.Show($"{ex.Message} Please pick a country.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    case 2:
                        {
                            MessageBox.Show($"{ex.Message} Please choose if you're going on a trip or vacation.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    case 3:
                        {
                            MessageBox.Show($"{ex.Message} Please input number of travelers", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show($"{ex.Message} Please choose what type of trip your taking!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            itemManager.AllPackingListItems.Clear();
            Close();
        }

        private void txtDestination_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtDestination.Clear();
        }

        private void txtTravelerAmount_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtTravelerAmount.Clear();
        }

        private void lvTravelItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewItem selectedItem = (ListViewItem)lvTravelItems.SelectedItem;

            if (selectedItem != null)
            {
                IPackingListItem selectedPackingListItem = (IPackingListItem)selectedItem.Tag;

                packingListItem = selectedPackingListItem;
            }
        }

        private void cbTripOrVacation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string selectedItem = (string)cbTripOrVacation.SelectedItem;

            bool isTrip = CheckIfTripOrVacation(selectedItem);

            if (isTrip)
            {
                cbTripType.Visibility = Visibility.Visible;
                xbAllInclusive.Visibility = Visibility.Hidden;
            }
            else if (!isTrip)
            {
                cbTripType.Visibility = Visibility.Hidden;
                xbAllInclusive.Visibility = Visibility.Visible;
            }
        }

        private bool CheckIfTripOrVacation(string selectedItem)
        {
            if (selectedItem.Equals("Trip"))
            {
                return true;
            }

            return false;
        }
    }
}
