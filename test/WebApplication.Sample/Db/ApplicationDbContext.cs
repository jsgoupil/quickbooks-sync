using Microsoft.EntityFrameworkCore;
using QbSync.WebConnector.Core;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Sample.Db
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<QbTicket> QbTickets { get; set; }
        public DbSet<QbKvpState> QbKvpStates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=webconnector.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(new User { Id = 1, UserName = "jsgoupil", Password = "password" });

            modelBuilder.Entity<QbKvpState>()
                .HasKey(c => new { c.Ticket, c.Key, c.CurrentStep });
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class QbKvpState
    {
        public string Ticket { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string CurrentStep { get; set; }
    }

    public class QbTicket : IAuthenticatedTicket
    {
        [Key]
        public string Ticket { get; set; }

        public string CurrentStep { get; set; }

        public bool Authenticated { get; set; }

        public int? UserId { get; set; }
        public virtual User User { get; set; }
    }
}
