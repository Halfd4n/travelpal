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
    public bool IsAllInclusive { get; set; }
    public Vacation(string destination, Countries country, int travelers, bool isAllInclusive) : base(destination, country, travelers)
    {
        IsAllInclusive = isAllInclusive;
    }
    public override string GetInfo()
    {
        return base.GetInfo();
    }
}
