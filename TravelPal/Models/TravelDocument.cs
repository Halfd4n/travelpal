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
    public bool isRequired { get; set; }

    public TravelDocument()
    {
        Name = Name;
        isRequired = false;
    }
    public string GetInfo()
    {
        return Name;
    }
}
