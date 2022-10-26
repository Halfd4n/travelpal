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

            UpdateListView();
        }

        private void UpdateListView()
        {
            lvTravelItems.Items.Clear();

            itemManager.AllPackingListItems = itemManager.GetAllPackingListItems();

            foreach(IPackingListItem packingItem in itemManager.AllPackingListItems)
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
            AddNewItemWindow addNewItemWindow = new();

            addNewItemWindow.Show();

            //itemManager.AllPackingListItems.Add(packingListItem);
        }

        private void btnRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (packingListItem == null || String.IsNullOrEmpty(packingListItem.ToString()))
            {
                MessageBox.Show("No item selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                itemManager.AllPackingListItems.Remove(packingListItem);

                UpdateListView();
            }
        }

        private void btnSaveTravel_Click(object sender, RoutedEventArgs e)
        {
            string destination = txtDestination.Text;
            string country = (string)cbCountry.SelectedItem;
            string tripOrVacation = (string)cbTripOrVacation.SelectedItem;
            string tripType = (string)cbTripType.SelectedItem;
            bool isAllInclusive = (bool)xbAllInclusive.IsChecked;
            string noOfTravels = txtTravelerAmount.Text;

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

            IPackingListItem packingListItem = (IPackingListItem)selectedItem;
        }

        private void cbTripOrVacation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ComboBoxItem selectedItem = (ComboBoxItem)cbTripOrVacation.SelectedItem;

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
