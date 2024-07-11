using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmShelf.Models
{
    public class Genre
    {
        public int GenreID { get; set; }

        [Required(ErrorMessage ="Please enter a genre name")]
        [StringLength(20,ErrorMessage ="Genre name is too long")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$",
            ErrorMessage ="Make sure name starts with a capital letter." + " Genre name must not include any symbols or numbers.")]
        public string Name { get; set; }

        // Navigation property
        public ICollection<Movie> Movies { get; set; }
    }
}
