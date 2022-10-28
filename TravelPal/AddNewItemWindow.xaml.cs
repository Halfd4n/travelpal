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
    /// Interaction logic for AddNewItemWindow.xaml
    /// </summary>
    public partial class AddNewItemWindow : Window
    {
        private bool isDocument;
        private bool isRequiredDocument;
        ItemManager itemManager = new();

        public AddNewItemWindow(ItemManager itemManager)
        {
            InitializeComponent();

            this.itemManager = itemManager;
        }

        private void txtNewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtNewItem.Clear();
        }

        private void txtQuantity_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtQuantity.Clear();
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            string itemName = txtNewItem.Text;
            string quantity = txtQuantity.Text;

            if (isDocument)
            {
                try
                {
                    if (string.IsNullOrEmpty(itemName))
                    {
                        throw new FormatException("No item name found! Please input desired item in the first textbox.");
                    }

                    TravelDocument travelDocument = new TravelDocument(itemName, isRequiredDocument);

                    bool isNewItem = itemManager.AddItem(travelDocument);

                    if (isNewItem)
                    {
                        MessageBox.Show($"Success! {travelDocument.Name} was added to the packing list!", "Message", MessageBoxButton.OK, MessageBoxImage.Information);

                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show($"{travelDocument.Name} is already in your packing list!", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    
                }
                catch(FormatException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (!isDocument)
            {
                try
                {
                    if (string.IsNullOrEmpty(itemName))
                    {
                        throw new FormatException("No item name found! Please input desired item in the first textbox.");
                    }
                    else if (string.IsNullOrEmpty(quantity))
                    {
                        throw new FormatException("No quantity found! Please input item quantity in the second textbox.");
                    }

                    bool isCorrectInput = int.TryParse(quantity, out int quantityInteger);

                    if (!isCorrectInput)
                    {
                        MessageBox.Show("Quantity wasn't given in an integer! Please try again", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                        txtQuantity.Clear();
                    }
                    else
                    {
                        OtherItem otherItem = new(itemName, quantityInteger);

                        bool isNewItem = itemManager.AddItem(otherItem);

                        if (isNewItem)
                        {
                            MessageBox.Show($"Success! {otherItem.Name} was added to the packing list!", "Message", MessageBoxButton.OK, MessageBoxImage.Information);

                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show($"{otherItem.Name} is already in your packing list!", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                    }
                }
                catch(FormatException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void xbIsDocument_Checked(object sender, RoutedEventArgs e)
        {
            txtQuantity.Visibility = Visibility.Hidden;
            xbIsDocumentRequired.Visibility = Visibility.Visible;
            isDocument = true;
        }

        private void xbIsDocument_Unchecked(object sender, RoutedEventArgs e)
        {
            txtQuantity.Visibility = Visibility.Visible;
            xbIsDocumentRequired.Visibility = Visibility.Hidden;
            isDocument = false;
        }

        private void xbIsDocumentRequired_Checked(object sender, RoutedEventArgs e)
        {
            isRequiredDocument = true;
        }

        private void xbIsDocumentRequired_Unchecked(object sender, RoutedEventArgs e)
        {
            isRequiredDocument = false;
        }

    }
}
