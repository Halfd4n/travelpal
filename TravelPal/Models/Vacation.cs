using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPal.Enums;

namespace TravelPal.Models;

public class Vacation : Travel
{
    public bool AllInclusive { get; set; }
    public Vacation(string destination, Countries country, int travelers, bool allInclusive) : base(destination, country, travelers)
    {
        AllInclusive = allInclusive;
    }
}
