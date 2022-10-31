using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPal.Enums;
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

    public Travel UpdateTravel(Travel travelToUpdate, string destination, Countries country, List<IPackingListItem> packingList,string tripOrVacation, TripTypes tripType, int travelInteger, DateTime startDate, DateTime endDate, int travelDays, bool isAllInclusive)
    {
        bool isTrip = TripOrVacation(tripOrVacation);

        if (isTrip)
        {
            Trip newTrip = new(destination, country, travelInteger, packingList, startDate, endDate, travelDays, tripType);

            AllTravels.Add(newTrip);
            AllTravels.Remove(travelToUpdate);

            return newTrip;
        }
        else if (!isTrip)
        {
            Vacation newVacation = new(destination, country, travelInteger, packingList, startDate, endDate, travelDays, isAllInclusive);

            AllTravels.Add(newVacation);
            AllTravels.Remove(travelToUpdate);

            return newVacation;
        }

        return travelToUpdate;
    }

    public bool TripOrVacation(string tripOrVacation)
    {
        if (tripOrVacation.Equals("Trip"))
        {
            return true;
        }

        return false;
    }
}
