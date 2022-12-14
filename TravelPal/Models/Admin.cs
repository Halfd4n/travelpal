using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPal.Enums;
using TravelPal.Interfaces;

namespace TravelPal.Models;
internal class Admin : IUser
{
    public string Username { get; set; }
    public string Password { get; set; }
    public Countries Location { get; set; }
    public bool IsAdmin { get; } = true;

    public Admin(string username, string password, Countries location)
    {
        Username = username;
        Password = password;
        Location = location;
    }
}
