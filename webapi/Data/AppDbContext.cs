using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users
        {
            get; set;
        }

        public DbSet<Film> Films
        {
            get; set;
        }
        public DbSet<Session> Sessions
        {
            get; set;
        }
        public DbSet<Place> Places
        {
            get; set;
        }
        public DbSet<Employee> Employees
        {
            get; set;
        }
        public DbSet<Ticket> Tickets
        {
            get; set;
        }
        public DbSet<CinemaHall> CinemaHalls
        {
            get; set;
        }
        public DbSet<Assets> Assets
        {
            get; set;
        }
        public DbSet<Genre> Genres
        {
            get; set;
        }
        public DbSet<FilmWorker> FilmWorkers
        {
            get; set;
        }
        public DbSet<FilmFilmWorker> FilmFilmWorkers
        {
            get; set;
        }
        public DbSet<FilmGenre> FilmGenres
        {
            get; set;
        }
    }
}
