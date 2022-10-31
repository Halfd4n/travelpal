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

    private void SetDataFields(string[] countries, string[] tripTypes, string[] tripOrVacation)
    {
        txtDestination.Text = selectedTravel.Destination;
        cbCountry.ItemsSource = countries;
        cbTripOrVacation.ItemsSource = tripOrVacation;
        cbTripType.ItemsSource = tripTypes;
        txtTravelerAmount.Text = selectedTravel.Travelers.ToString();

        int defaultIndex = FindDefaultCountry(countries);

        cbCountry.SelectedIndex = defaultIndex;

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

    private int FindDefaultCountry(string[] countries)
    {
        int defaultIndex = 0;

        for (int i = 0; i < countries.Length; i++)
        {
            if (selectedTravel.Country.ToString() == countries[i])
            {
                return defaultIndex = i;
            }
        }

        return -1;
    }

    private bool TripOrVacation(Travel selectedTravel)
    {
        if (selectedTravel is Trip)
        {
            return true;
        }

        return false;
    }
    private bool LeisureOrOccupational(Trip selectedTrip)
    {
        if (selectedTrip.TripType == TripTypes.Leisure)
        {
            return true;
        }
        
        return false;
    }

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

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
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
            selectedTravel.PackingList.Remove(packingListItem);
            

            UpdateListView();
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

    private void lvTravelItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ListViewItem selectedItem = (ListViewItem)lvTravelItems.SelectedItem;

        if (selectedItem != null)
        {
            IPackingListItem selectedPackingListItem = (IPackingListItem)selectedItem.Tag;
            packingListItem = selectedPackingListItem;
        }
    }

    private void txtTravelerAmount_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        txtTravelerAmount.Clear();
    }

    private void cbTripType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        string selectedItem = (string)cbTripType.SelectedItem;
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
    private void cbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (cbCountry.SelectedItem is not null && userManager.SignedInUser is not null)
        {
            bool isUserCountryOfOriginInEurope = Enum.IsDefined(typeof(EuropeanCountries), userManager.SignedInUser.Location.ToString());
            bool isSelectedCountryOfDestinationInEurope = Enum.IsDefined(typeof(EuropeanCountries), cbCountry.SelectedItem);

            if (isUserCountryOfOriginInEurope && !isSelectedCountryOfDestinationInEurope)
            {
                TravelDocument passport = new("Passport", true);

                bool isAddingPassport = itemManager.AddItem(passport);

                if (isAddingPassport)
                {
                    UpdateListView();
                }
            }
            else if (!isUserCountryOfOriginInEurope && isSelectedCountryOfDestinationInEurope)
            {
                TravelDocument passport = new("Passport", true);

                bool isAddingPassport = itemManager.AddItem(passport);

                if (isAddingPassport)
                {
                    UpdateListView();
                }
            }
            else if (isUserCountryOfOriginInEurope && isSelectedCountryOfDestinationInEurope)
            {
                foreach (IPackingListItem item in itemManager.AllPackingListItems)
                {
                    if (item.Name.ToLower().Equals("passport"))
                    {
                        TravelDocument document = (TravelDocument)item;

                        if (document.isRequired == true)
                        {
                            document.isRequired = false;

                            UpdateListView();
                        }
                    }
                }
            }
        }
    }

    private void txtDestination_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        txtDestination.Clear();
    }

    private void xbAllInclusive_Checked(object sender, RoutedEventArgs e)
    {
        isAllInclusive = true;
    }

    private void xbAllInclusive_Unchecked(object sender, RoutedEventArgs e)
    {
        isAllInclusive = false;
    }
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

    private void AddNewItemsToPackList()
    {
        foreach (IPackingListItem newPackingItem in itemManager.AllPackingListItems)
        {
            selectedTravel.PackingList.Add(newPackingItem);
        }
    }

    private void UpdateListView()
    {
        lvTravelItems.Items.Clear();

        itemManager.AllPackingListItems = itemManager.GetAllPackingListItems();

        foreach (IPackingListItem currentTravelPackingItem in selectedTravel.PackingList)
        {
            if (currentTravelPackingItem is TravelDocument)
            {
                ListViewItem item = new();

                item.Content = currentTravelPackingItem.GetInfo();
                item.Tag = currentTravelPackingItem;

                lvTravelItems.Items.Add(item);
            }
            else if (currentTravelPackingItem is OtherItem)
            {
                ListViewItem item = new();
                item.Content = currentTravelPackingItem.GetInfo();
                item.Tag = currentTravelPackingItem;

                lvTravelItems.Items.Add(item);
            }
        }

        foreach (IPackingListItem packingListItem in itemManager.AllPackingListItems)
        {
            if (packingListItem is TravelDocument)
            {
                ListViewItem item = new();

                item.Content = packingListItem.GetInfo();
                item.Tag = packingListItem;

                lvTravelItems.Items.Add(item);
            }
            else if (packingListItem is OtherItem)
            {
                ListViewItem item = new();
                item.Content = packingListItem.GetInfo();
                item.Tag = packingListItem;

                lvTravelItems.Items.Add(item);
            }
        }
    }
}
