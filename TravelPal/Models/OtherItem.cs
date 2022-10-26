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

    public OtherItem()
    {
        Name = Name;
        Quantity = Quantity;
    }
    public string GetInfo()
    {
        return Name;
    }
}
