using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPal.Enums;
using TravelPal.Interfaces;

namespace TravelPal.Models;
public class Vacation : Travel
{
    public bool IsAllInclusive { get; set; }
    public Vacation(string destination, Countries country, int travelers, List<IPackingListItem> packingListItems, DateTime startDate, DateTime endDate, int travelDays, bool isAllInclusive) : base(destination, country, travelers, packingListItems, startDate, endDate, travelDays)
    {
        IsAllInclusive = isAllInclusive;
    }

    // Method to return the base GetInfo method:
    public override string GetInfo()
    {
        return base.GetInfo();
    }
}
