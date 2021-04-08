using Microsoft.EntityFrameworkCore;
using QbSync.WebConnector.Core;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Sample.Db
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<QbTicket> QbTickets { get; set; } = default!;
        public DbSet<QbKvpState> QbKvpStates { get; set; } = default!;
        public DbSet<QbSetting> QbSettings { get; set; } = default!;

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

            modelBuilder.Entity<QbSetting>()
                .HasAlternateKey(c => c.Name);
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
    }

    public class QbKvpState
    {
        public string Ticket { get; set; } = default!;
        public string Key { get; set; } = default!;
        public string? Value { get; set; }
        public string CurrentStep { get; set; } = default!;
    }

    public class QbTicket : IAuthenticatedTicket
    {
        [Key]
        public string Ticket { get; set; } = default!;

        public string CurrentStep { get; set; } = default!;

        public bool Authenticated { get; set; }

        public int? UserId { get; set; }
        public virtual User? User { get; set; }
    }

    public class QbSetting
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Value { get; set; }
    }
}
