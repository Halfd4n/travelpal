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


    public Travel(string destination, Countries country, int travelers)
    {
        Destination = destination;
        Country = country;
        Travelers = travelers;
        StartDate = StartDate;
        EndDate = EndDate;
        TravelDays = TravelDays;
    }

    public virtual string GetInfo()
    {
        return $"{Destination}, {Country}";
    }
}
