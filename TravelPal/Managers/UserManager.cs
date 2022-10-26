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
        AllUsers.Add(new User("Gandalf", "password", Enums.Countries.USA));

        AllUsers.Add(new Admin("admin", "password", Enums.Countries.Sweden));
    }
    public List<IUser> GetAllUsers()
    {
        return AllUsers;
    }

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

    //public void RemoveUser(IUser userToRemove)
    //{

    //}

    public bool UpdateUserName(IUser currentUser, string newUserName)
    {
        return true;
    }

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

    public void SignOutUser()
    {
        if (SignedInUser != null)
        {
            SignedInUser = null;
        }
    }
}
