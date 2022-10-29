﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPal.Interfaces;
using TravelPal.Models;

namespace TravelPal.Managers;

public class TravelManager
{
    public List<Travel> AllTravels { get; set; } = new();

    public List<Travel> GetAllTravels()
    {
        return AllTravels;
    }
    public void AddTravel(Travel newTravel)
    {
        AllTravels.Add(newTravel);
    }

    public void RemoveTravel(Travel travelToRemove)
    {
        AllTravels.Remove(travelToRemove);


    }
}
