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

    public void AddItem()
    {

    }

    public void RemoveItem()
    {

    }
}
