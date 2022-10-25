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
    private List<IUser> allUsers = new();
    private IUser signedInUser;

    public void SeedDefaultUsers()
    {
        User defaultUser = new("Gandalf", "password", Enums.Countries.USA);

        Admin admin = new("admin", "password", Enums.Countries.Sweden);

        allUsers.Add(defaultUser);
        allUsers.Add(admin);
    }
    public List<IUser> GetAllUsers()
    {
        return allUsers;
    }

    public bool AddUser(IUser newUser)
    {
        foreach(IUser user in allUsers)
        {
            if(user.Username == newUser.Username)
            {
                return false;
            }
        }
        return true;
    }

    public void RemoveUser(IUser userToRemove)
    {

    }

    public bool UpdateUserName(IUser currentUser, string newUserName)
    {
        return true;
    }

    public bool ValidateUserName(string username)
    {
        return true;
    }

    public bool SignInUser (string username, string password)
    {
        return true;
    }
}
