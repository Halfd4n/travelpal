using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using TravelPal.Interfaces;
using TravelPal.Models;

namespace TravelPal.Managers;
public class UserManager
{
    public List<IUser> AllUsers { get; set; } = new();
    public IUser SignedInUser { get; set; }

    public UserManager()
    {
        OtherItem defautlItem1 = new("Toothbrush", 1);
        OtherItem defautlItem2 = new("Tickets", 2);
        OtherItem defautlItem3 = new("T-Shirt", 3);

        List<IPackingListItem> defaultPackingList1 = new();

        defaultPackingList1.Add(defautlItem1);
        defaultPackingList1.Add(defautlItem2);
        defaultPackingList1.Add(defautlItem3);

        DateTime defaultStart1 = new(2023, 2, 10);
        DateTime defaultEnd1 = new(2023, 2, 12);

        TimeSpan travelTimeSpan1 = defaultEnd1 - defaultStart1;

        int travelLength = Convert.ToInt32(travelTimeSpan1.Days);

        Trip defaultTrip = new("ESL", Enums.Countries.Poland, 2, defaultPackingList1, defaultStart1, defaultEnd1, travelLength, Enums.TripTypes.Leisure);

        OtherItem defaultItem4 = new("Flipflops", 1);
        OtherItem defaultItem5 = new("Shades", 2);

        TravelDocument defaultTravelDocument = new("Passport", false);

        List<IPackingListItem> defaultPackingList2 = new();

        defaultPackingList2.Add(defaultItem4);
        defaultPackingList2.Add(defaultItem5);
        defaultPackingList2.Add(defaultTravelDocument);

        DateTime defaultStart2 = new(2022, 12, 10);
        DateTime defaultEnd2 = new(2022, 12, 24);

        travelLength = (int)(defaultEnd2 - defaultStart2).TotalDays;

        bool allInclusive = true;

        Vacation defaultVacation = new("Barcelona", Enums.Countries.Spain, 6, defaultPackingList2, defaultStart2, defaultEnd2, travelLength, allInclusive);

        User Gandalf = new User("Gandalf", "password", Enums.Countries.Sweden);

        Gandalf.Travels.Add(defaultTrip);
        Gandalf.Travels.Add(defaultVacation);

        AllUsers.Add(Gandalf);
        AllUsers.Add(new Admin("admin", "password", Enums.Countries.Sweden));
    }

    // Method that returns AllUsers list containing all IUser objects:
    public List<IUser> GetAllUsers()
    {
        return AllUsers;
    }

    // Method that adds a new user to the AllUsers list:
    public bool AddUser(IUser newUser)
    {
        bool isValidUser = ValidateUserName(newUser.Username);

        if (!isValidUser)
        {
            return false;
        }
        
        AllUsers.Add(newUser);
        return true;
    }

    // !!! --- MAYBE CHANGE --- !!!
    public bool UpdateUser(string newUserName)
    {
        bool isValidNewUserName = ValidateUserName(newUserName);

        if (!isValidNewUserName)
        {
            return false;
        }

        return true;
    }

    // Method that checks if a username is already in use:
    private bool ValidateUserName(string username)
    {
        foreach (IUser user in AllUsers)
        {
            if (user.Username.Equals(username))
            {
                return false;
            }
        }
        return true;
    }

    // Method to set the property SignedInUser to the current user loging in:
    public bool SignInUser (string username, string password)
    {
        foreach(IUser user in AllUsers)
        {
            if(user.Username.Equals(username) && user.Password.Equals(password))
            {
                SignedInUser = user;

                return true;
            }
        }
        
        return false;
    }

    // Method to sign out that sets SignedInUser back to null.
    public void SignOutUser()
    {
        if (SignedInUser != null)
        {
            SignedInUser = null;
        }
    }
}
