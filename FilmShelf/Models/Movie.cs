using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmShelf.Models
{
    public class Movie
    {
        public int MovieID { get; set; }

        [Required(ErrorMessage ="Please enter a Title")]
        [StringLength(50,ErrorMessage ="Title is too long.")]
        public required string Title { get; set; }

        [Required(ErrorMessage ="Please provide a description")]
        [StringLength(500,ErrorMessage ="Description is too long.")]
        public required string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage ="Please enter a price")]
        [Range(1,20,ErrorMessage ="Please ensure price is within $1 to $20.")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public int GenreID { get; set; }

        [Required(ErrorMessage = "Please upload an image")]
        [Display(Name ="Image File")]
        [NotMapped]
        public required IFormFile ImageFile { get; set; }

        [Display(Name = "Image")]
        public required string ImageUrl { get; set; }

        // Navigation property
        public required Genre Genre { get; set; }
    }
}
