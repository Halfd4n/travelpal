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

    public void RemoveItem(IPackingListItem itemToRemove)
    {
        AllPackingListItems.Remove(itemToRemove);
    }

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
