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
public partial class TravelDetailsWindow : Window
{
    private UserManager userManager = new();
    private TravelManager travelManager = new();
    private Travel selectedTravel;
    private ItemManager itemManager = new();
    private IPackingListItem packingListItem;
    private bool isAllInclusive;
    private int arrayIndex;
    private TripTypes tripTypesEnum;
    private List<ListViewItem> itemsInPackingList = new();
    private TravelDocument passport = new("Passport", false);
    private TravelDocument passportToRemove;

    public TravelDetailsWindow(UserManager userManager, TravelManager travelManager, Travel selectedTravel)
    {
        InitializeComponent();

        this.userManager = userManager;
        this.travelManager = travelManager;
        this.selectedTravel = selectedTravel;

        string[] countries = Enum.GetNames(typeof(Countries));
        string[] tripOrVacation = new string[2] { "Trip", "Vacation" };
        string[] tripTypes = Enum.GetNames(typeof(TripTypes));

        SetDataFields(countries, tripTypes, tripOrVacation);
    }

    // Method to set content of data fields to the values of variable in the selected Travel:
    private void SetDataFields(string[] countries, string[] tripTypes, string[] tripOrVacation)
    {
        txtDestination.Text = selectedTravel.Destination;
        cbCountry.ItemsSource = countries;
        cbTripOrVacation.ItemsSource = tripOrVacation;
        cbTripType.ItemsSource = tripTypes;
        txtTravelerAmount.Text = selectedTravel.Travelers.ToString();

        int defaultCountryIndex = FindDefaultCountry(countries);

        cbCountry.SelectedIndex = defaultCountryIndex;

        bool isTrip = TripOrVacation(selectedTravel);

        if (isTrip)
        {
            cbTripOrVacation.SelectedIndex = 0;

            cbTripType.Visibility = Visibility.Visible;
            xbAllInclusive.Visibility = Visibility.Hidden;

            Trip selectedTrip = (Trip)selectedTravel;

            bool isLeisure = LeisureOrOccupational(selectedTrip);

            if (isLeisure)
            {
                cbTripType.SelectedIndex = 0;
            }
            else
            {
                cbTripType.SelectedIndex = 1;
            }
        }
        else
        {
            cbTripOrVacation.SelectedIndex = 1;

            cbTripType.Visibility = Visibility.Hidden;
            xbAllInclusive.Visibility = Visibility.Visible;

            Vacation selectedVacation = (Vacation)selectedTravel;

            bool isAllInclusive = selectedVacation.IsAllInclusive;

            if (isAllInclusive)
            {
                xbAllInclusive.IsChecked = true;
            }
            else if (!isAllInclusive)
            {
                xbAllInclusive.IsChecked = false;
            }
        }

        UpdateListView();

        dpTravelStart.Text = selectedTravel.StartDate.ToString();
        dpTravelEnd.Text = selectedTravel.EndDate.ToString();

        lblTravelLength.Content = $"{selectedTravel.TravelDays.ToString()} days";

        LockDataFields();
    }

    // Method to initially lock the data fields of the window:
    private void LockDataFields()
    {
        txtDestination.IsEnabled = false;
        cbCountry.IsEnabled = false;
        cbTripOrVacation.IsEnabled = false;
        cbTripType.IsEnabled = false;
        xbAllInclusive.IsEnabled = false;
        txtTravelerAmount.IsEnabled = false;
        btnAddItem.IsEnabled = false;
        btnRemoveItem.IsEnabled = false;
        dpTravelStart.IsEnabled = false;
        dpTravelEnd.IsEnabled = false;
    }

    // Method to find the index of the Country of the selected travel:
    private int FindDefaultCountry(string[] countries)
    {
        int defaultCountryIndex = 0;

        for (int i = 0; i < countries.Length; i++)
        {
            if (selectedTravel.Country.ToString() == countries[i])
            {
                return defaultCountryIndex = i;
            }
        }

        return -1;
    }

    // Method to check if the Travel object selectedTravel is a trip or not:
    private bool TripOrVacation(Travel selectedTravel)
    {
        if (selectedTravel is Trip)
        {
            return true;
        }

        return false;
    }

    // Method that checks inf the selectedTrip is a leisure trip or not:
    private bool LeisureOrOccupational(Trip selectedTrip)
    {
        if (selectedTrip.TripType == TripTypes.Leisure)
        {
            return true;
        }
        
        return false;
    }

    // Method to unlock data field to allow for editing the selected Travel, called via click action on the Edit Travel button:
    private void btnEditTravel_Click(object sender, RoutedEventArgs e)
    {
        txtDestination.IsEnabled = true;
        cbCountry.IsEnabled = true;
        cbTripOrVacation.IsEnabled = true;
        cbTripType.IsEnabled = true;
        xbAllInclusive.IsEnabled = true;
        txtTravelerAmount.IsEnabled = true;
        btnAddItem.IsEnabled = true;
        btnRemoveItem.IsEnabled = true;
        dpTravelStart.IsEnabled = true;
        dpTravelEnd.IsEnabled = true;

        btnEditTravel.Visibility = Visibility.Hidden;
        btnSaveTravel.Visibility = Visibility.Visible;
    }

    // Method to cancel the edit action and in doing so closing the TravelDetailsWindow:
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    // Method to remove item from the packing list:
    private void btnRemoveItem_Click(object sender, RoutedEventArgs e)
    {
        if (packingListItem == null)
        {
            MessageBox.Show("No item selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
            itemManager.RemoveItem(packingListItem);
            selectedTravel.PackingList.Remove(packingListItem);
            

            UpdateListView();
        }
    }

    // Method to open the AddNewItemWindow, allowing for adding items to the packing list: 
    private void btnAddItem_Click(object sender, RoutedEventArgs e)
    {
        AddNewItemWindow addNewItemWindow = new(itemManager);

        addNewItemWindow.Show();

        addNewItemWindow.Closed += AddNewItemWindow_Closed;
    }

    // Method to update the lvTravelItems ListView upon closing of the AddNewItemWindow: 
    private void AddNewItemWindow_Closed(object? sender, EventArgs e)
    {
        UpdateListView();
    }

    // Method to notice changes in selection in the lvTravelItems ListView:
    private void lvTravelItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ListViewItem selectedItem = (ListViewItem)lvTravelItems.SelectedItem;

        if (selectedItem != null)
        {
            IPackingListItem selectedPackingListItem = (IPackingListItem)selectedItem.Tag;
            packingListItem = selectedPackingListItem;
        }
    }

    // Method to clear the content of txtTravelerAmount TextBox via a double click action on the textbox:
    private void txtTravelerAmount_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        txtTravelerAmount.Clear();
    }

    // Method to notice changes in selection in the cbTripType ComboBox:
    private void cbTripType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        string selectedItem = (string)cbTripType.SelectedItem;
    }

    // Method to notice changes in selection in the cbTripOrVacation ComboBox:
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

    // Method to check the selected item is a Trip or not:
    private bool CheckIfTripOrVacation(string selectedItem)
    {
        if (selectedItem.Equals("Trip"))
        {
            return true;
        }

        return false;
    }

    // Method to notice changes in selection in the cbCountry ComboBox:
    private void cbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

        if (cbCountry.SelectedItem != null)
        {
            bool isUserCountryOfOriginInEurope = Enum.IsDefined(typeof(EuropeanCountries), userManager.SignedInUser.Location.ToString());
            bool isSelectedCountryOfDestinationInEurope = Enum.IsDefined(typeof(EuropeanCountries), cbCountry.SelectedItem);

            bool isPassportRequired = IsPassportRequired(isUserCountryOfOriginInEurope, isSelectedCountryOfDestinationInEurope);

            bool isPassportInPackinList = IsPassportInPackingList();

            if (!isPassportInPackinList)
            {
                selectedTravel.PackingList.Add(passport);
                
                UpdateListView();
            }
            else
            {
                foreach (IPackingListItem item in selectedTravel.PackingList)
                {
                    if (item.Name.ToLower().Equals("passport"))
                    {
                        passportToRemove = (TravelDocument)item;
                    }
                }

                selectedTravel.PackingList.Remove(passportToRemove);

                selectedTravel.PackingList.Add(passport);

                UpdateListView();
            }


        }
    }

    // Method to check if passport is already in packing list:
    private bool IsPassportInPackingList()
    {
        foreach (IPackingListItem packingItem in selectedTravel.PackingList)
        {
            if (packingItem.Name.ToLower().Equals("passport"))
            {
                return true;
            }
        }
        return false;
    }

    // Method to check if passport is required:
    private bool IsPassportRequired(bool isUserCountryOfOriginInEurope, bool isSelectedCountryOfDestinationInEurope)
    {
        if (isUserCountryOfOriginInEurope && isSelectedCountryOfDestinationInEurope)
        {
            passport.IsRequired = false;
        }
        else if (isUserCountryOfOriginInEurope && !isSelectedCountryOfDestinationInEurope)
        {
            passport.IsRequired = true;
        }
        else
        {
            passport.IsRequired = true;
        }

        return false;
    }

    // Method to clear the content of txtDestination TextBox via a double click action on the textbox:
    private void txtDestination_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtDestination.Clear();
        }

        // Method to check if the xbAllInclusive CheckBox is checked:
        private void xbAllInclusive_Checked(object sender, RoutedEventArgs e)
        {
            isAllInclusive = true;
        }

        // Method to check if the xbAllInclusive CheckBox is unchecked:
        private void xbAllInclusive_Unchecked(object sender, RoutedEventArgs e)
        {
            isAllInclusive = false;
        }

        // Method to save all the changes made to the Travel:
        private void btnSaveTravel_Click(object sender, RoutedEventArgs e)
        {
            string destination = txtDestination.Text;
            string country = (string)cbCountry.SelectedItem;
            string tripOrVacation = (string)cbTripOrVacation.SelectedItem;
            string tripType = (string)cbTripType.SelectedItem;
            bool isAllInclusive = (bool)xbAllInclusive.IsChecked;
            string noOfTravelers = txtTravelerAmount.Text;

            string[] textInputsToTry = new string[4] { destination, country, tripOrVacation, noOfTravelers };
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

                    tripTypesEnum = (TripTypes)Enum.Parse(typeof(TripTypes), tripType);

                }

                AddNewItemsToPackList();

                bool isInteger = int.TryParse(noOfTravelers, out int travelersInteger);

                if (isInteger)
                {
                    DateTime startDate = (DateTime)dpTravelStart.SelectedDate;

                    DateTime endDate = (DateTime)dpTravelEnd.SelectedDate;

                    bool isCorrectDates = travelManager.IsCorrectTravelDate(startDate, endDate);

                    if (isCorrectDates)
                    {
                        int travelDays = (int)(endDate - startDate).TotalDays;

                        Travel updatedTravel = travelManager.UpdateTravel(selectedTravel, destination, countryEnum, selectedTravel.PackingList, tripOrVacation, tripTypesEnum, travelersInteger, startDate, endDate, travelDays, isAllInclusive);

                        MessageBox.Show("Success! Your travel was updated!", "Message", MessageBoxButton.OK);

                        User currentUser = (User)userManager.SignedInUser;

                        currentUser.Travels.Add(updatedTravel);
                        currentUser.Travels.Remove(selectedTravel);

                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect dates for your travel, please try again!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Number of travelers wasn't given in an integer! Please try again", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    txtTravelerAmount.Clear();
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
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message} Please choose what type of trip your taking!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Method to add new items to packing list:
        private void AddNewItemsToPackList()
        {
            foreach (IPackingListItem newPackingItem in itemManager.AllPackingListItems)
            {
                selectedTravel.PackingList.Add(newPackingItem);
            }
        }

        // Method to update lvTravelItems ListView:
        private void UpdateListView()
        {
            lvTravelItems.ItemsSource = null;
            itemsInPackingList.Clear();

            itemManager.AllPackingListItems = itemManager.GetAllPackingListItems();

            foreach (IPackingListItem currentTravelPackingItem in selectedTravel.PackingList)
            {
                if (currentTravelPackingItem is TravelDocument)
                {
                    ListViewItem item = new();

                    item.Content = currentTravelPackingItem;
                    item.Tag = currentTravelPackingItem;

                    itemsInPackingList.Add(item);
                }
                else if (currentTravelPackingItem is OtherItem)
                {
                    ListViewItem item = new();

                    item.Content = currentTravelPackingItem;
                    item.Tag = currentTravelPackingItem;

                    itemsInPackingList.Add(item);
                }
            }

            foreach (IPackingListItem packingListItem in itemManager.AllPackingListItems)
            {
                if (packingListItem is TravelDocument)
                {
                    ListViewItem item = new();

                    item.Content = packingListItem;
                    item.Tag = packingListItem;

                    itemsInPackingList.Add(item);
                }
                else if (packingListItem is OtherItem)
                {
                    ListViewItem item = new();

                    item.Content = packingListItem;
                    item.Tag = packingListItem;

                    itemsInPackingList.Add(item);
                }
            }

            lvTravelItems.ItemsSource = itemsInPackingList;
        }
}
