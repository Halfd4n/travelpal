using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPal.Enums;
using TravelPal.Interfaces;

namespace TravelPal.Models;

public class Travel
{
    public string Destination { get; set; }
    public Countries Country { get; set; }
    public int Travelers { get; set; }
    public List<IPackingListItem> PackingList { get; set; } = new();
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TravelDays { get; set; }


    public Travel(string destination, Countries country, int travelers, List<IPackingListItem> packingListItems, DateTime startDate, DateTime endDate, int travelDays)
    {
        Destination = destination;
        Country = country;
        Travelers = travelers;
        PackingList = packingListItems;
        StartDate = startDate;
        EndDate = endDate;
        TravelDays = travelDays;
    }

    public virtual string GetInfo()
    {
        return $"{Destination}, {Country}";
    }

    public List<IPackingListItem> GetPackingList()
    {
        return PackingList;
    }
}
