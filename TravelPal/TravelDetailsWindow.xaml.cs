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
    public partial class TravelDetailsWindow : Window
    {
        private UserManager userManager = new();
        private TravelManager travelManager = new();
        private Travel selectedTravel;

        public TravelDetailsWindow(UserManager userManager, TravelManager travelManager, Travel selectedTravel)
        {
            InitializeComponent();

            this.userManager = userManager;
            this.travelManager = travelManager;
            this.selectedTravel = selectedTravel;

            string[] countries = Enum.GetNames(typeof(Countries));
            string[] tripOrVacation = new string[2] { "Trip", "Vacation" };
            string[] tripTypes = Enum.GetNames(typeof(TripTypes));

            SetUI(countries, tripTypes, tripOrVacation);
            LockDataFields();
        }

        private void SetUI(string[] countries, string[] tripTypes, string[] tripOrVacation)
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

            foreach(IPackingListItem packingListItem in selectedTravel.PackingList)
            {
                ListViewItem item = (ListViewItem)packingListItem;

                item.Name = packingListItem.GetInfo();
                item.Tag = packingListItem;

                lvTravelItems.Items.Add(item);
            }

            dpTravelStart.DisplayDateStart = selectedTravel.StartDate;
            dpTravelEnd.DisplayDateStart = selectedTravel.EndDate;

        }
        private void LockDataFields()
        {
            txtDestination.IsReadOnly = true;
            cbCountry.IsReadOnly = true;
            cbTripOrVacation.IsReadOnly = true;
            cbTripType.IsReadOnly = true;
            xbAllInclusive.IsEnabled = false;
            btnAddItem.IsEnabled = false;
            btnRemoveItem.IsEnabled = false;
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

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRemoveItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lvTravelItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtTravelerAmount_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void cbTripType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbTripOrVacation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtDestination_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void xbAllInclusive_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void xbAllInclusive_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}
