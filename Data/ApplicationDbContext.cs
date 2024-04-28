using Microsoft.EntityFrameworkCore;
using Диплом.Models;

namespace Диплом.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<GameRoom> GameRooms { get; set; }
        public DbSet<BoardGame> BoardGames { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<OpenRent> OpenRents { get; set; }
        public DbSet<Member> Members { get; set; }
    }
}
