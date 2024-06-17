using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FilmShelf.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;


namespace FilmShelf.Models
{
    public class Rental
    {
        public int RentalID { get; set; }

        public int MovieID { get; set; }

        
        public string UserID { get; set; }
        public FilmShelfUser User { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Rental Date")]
        public DateTime RentDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Return Date")]
        public DateTime ReturnDate { get; set; }

        // Navigation property
        public Movie Movie { get; set; }
    }
}
