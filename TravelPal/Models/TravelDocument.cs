using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPal.Interfaces;

namespace TravelPal.Models;
public class TravelDocument : IPackingListItem
{
    public string Name { get; set; }
    public bool IsRequired { get; set; }

    public TravelDocument(string itemName, bool isRequired)
    {
        this.Name = itemName;
        this.IsRequired = isRequired;
    }

    // Method that returns the name of a TravelDocument object:
    public string GetInfo()
    {
        return Name;
    }

    
}
