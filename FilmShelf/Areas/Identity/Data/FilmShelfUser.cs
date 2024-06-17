using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FilmShelf.Areas.Identity.Data;

// Add profile data for application users by adding properties to the FilmShelfUser class
public class FilmShelfUser : IdentityUser
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public enum Gender
    {
        Male,
        Female,
        Other
    }
    public Gender SelectedGender { get; set; }

    public enum Region
    {
        Asia,
        Europe,
        Africa,
        America,
        Oceania
    }
    public Region SelectedRegion { get; set; }
}

