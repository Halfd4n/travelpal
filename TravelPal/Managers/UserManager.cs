using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPal.Interfaces;

namespace TravelPal.Managers;

public class UserManager
{
    private List<IUser> allUsers = new();

    private IUser signedInUser;

    public bool AddUser(IUser newUser)
    {
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
