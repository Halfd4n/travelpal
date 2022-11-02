using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPal.Interfaces;

namespace TravelPal.Managers;

public class ItemManager
{
    public List<IPackingListItem> AllPackingListItems { get; set; } = new();

    public List<IPackingListItem> GetAllPackingListItems()
    {
        return AllPackingListItems;
    }

    // Method to add new item to the AllPackingListItems list and returning a boolean:
    public bool AddItem(IPackingListItem newItem)
    {
        bool isValidItem = ValidateItem(newItem.Name);

        if (!isValidItem)
        {
            return false;
        }

        AllPackingListItems.Add(newItem);
        return true;
    }

    // Method to remove item from AllPackingListItems list:
    public void RemoveItem(IPackingListItem itemToRemove)
    {
        AllPackingListItems.Remove(itemToRemove);
    }

    // Method to check if the item is already in the list and returning a boolean:
    private bool ValidateItem(string newItem)
    {
        foreach(IPackingListItem item in AllPackingListItems)
        {
            if (item.Name.Equals(newItem))
            {
                return false;
            }
        }
        return true;
    }
}
