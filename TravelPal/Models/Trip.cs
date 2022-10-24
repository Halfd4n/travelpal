using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPal.Enums;

namespace TravelPal.Models;

public class Trip : Travel
{
    public TripTypes TripType { get; set; }
    public Trip(string destination, Countries country, int travelers, TripTypes tripType) : base(destination, country, travelers)
    {
        TripType = tripType;
    }
}
