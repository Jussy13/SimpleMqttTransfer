using Microsoft.EntityFrameworkCore;

namespace Database.Models
{
    public class CalculationContext : DbContext
    {
        public CalculationContext(DbContextOptions optionsBuilder) : base(optionsBuilder)
        { }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Message>()
                .HasIndex(m => new {Type = m.Topic});
        }
    }
}
