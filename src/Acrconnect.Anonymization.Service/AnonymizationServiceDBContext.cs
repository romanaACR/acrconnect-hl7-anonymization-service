using AcrConnect.Anonymization.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace AcrConnect.Anonymization.Service
{
    public class AnonymizationServiceDBContext : DbContext
    {
        private static readonly int MaximumLengthOfName = 100;
        private static readonly int MaximumLengthOfDescription = 1000;
        private static readonly int MaximumLength = 5000;

        public DbSet<AnonymizationLog> Logs { get; set; }
        public DbSet<AcrProfileDetail> AcrProfileDetails { get; set; }


        public AnonymizationServiceDBContext(DbContextOptions options) :
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //AnonymizationLog
            modelBuilder.Entity<AnonymizationLog>()
                        .Property(x => x.Id)
                        .ValueGeneratedOnAdd();
            modelBuilder.Entity<AnonymizationLog>()
                        .Property(x => x.Message)
                        .IsRequired(false);

            //AcrProfileDetail
            modelBuilder.Entity<AcrProfileDetail>()
                 .HasKey(x => x.Id);

            modelBuilder.Entity<AcrProfileDetail>()
                        .Property(x => x.Id)
                        .ValueGeneratedOnAdd(); 

        }
    }
}
