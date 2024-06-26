using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FilmShelf.Areas.Identity.Data;
using FilmShelf.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using FilmShelf;

namespace FilmShelf.Controllers
{
    public class MoviesController : Controller
    {
        private readonly FilmShelfContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public MoviesController(FilmShelfContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Movies
        public async Task<IActionResult> Index(
    string sortOrder,
    string currentFilter,
    string searchString,
    int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var movies = _context.Movie.Include(s => s.Genre);
                           
            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString)).Include(s => s.Genre);
            }
            switch (sortOrder)
            {
                case "name_desc":
                    movies = movies.OrderByDescending(s => s.Title).Include(s => s.Genre);
                    break;
                case "Date":
                    movies = movies.OrderBy(s => s.ReleaseDate).Include(s => s.Genre);
                    break;
                case "date_desc":
                    movies = movies.OrderByDescending(s => s.ReleaseDate).Include(s => s.Genre);
                    break;
                default:
                    movies = movies.OrderBy(s => s.Title).Include(s => s.Genre);
                    break;
            }
            int pageSize = 3;
            return View(await PaginatedList<Movie>.CreateAsync(movies.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.MovieID == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            ViewData["GenreID"] = new SelectList(_context.Set<Genre>(), "GenreID", "Name");
            return View();
        }


        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieID,Title,Description,ReleaseDate,Price,GenreID,Genre.Name,ImageFile")] Movie movie)
        {
            if (!ModelState.IsValid)
            {

                //Save image to wwwroot/image
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(movie.ImageFile.FileName);
                string extension = Path.GetExtension(movie.ImageFile.FileName);
                movie.ImageUrl = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                using (var fileStream = new FileStream(path,FileMode.Create))
                {
                    await movie.ImageFile.CopyToAsync(fileStream);
                }
                //Insert Record 
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreID"] = new SelectList(_context.Set<Genre>(), "GenreID", "Name", movie.GenreID);
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            ViewData["GenreID"] = new SelectList(_context.Set<Genre>(), "GenreID", "Name", movie.GenreID);
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieID,Title,Description,ReleaseDate,Price,GenreID,ImageUrl")] Movie movie)
        {
            if (id != movie.MovieID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.MovieID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreID"] = new SelectList(_context.Set<Genre>(), "GenreID", "Name", movie.GenreID);
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.MovieID == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.MovieID == id);
        }
    }
}
