using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPal.Interfaces;

namespace TravelPal.Models;

public class OtherItem : IPackingListItem
{
    public string Name { get; set; }
    public int Quantity { get; set; }

    public OtherItem(string name, int quantity)
    {
        this.Name = name;
        this.Quantity = quantity;
    }

    // Method that returns the name of an OtherItem object:
    public string GetInfo()
    {
        return Name;
    }
}
