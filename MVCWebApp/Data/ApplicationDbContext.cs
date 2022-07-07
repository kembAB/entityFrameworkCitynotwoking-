using Microsoft.EntityFrameworkCore;
using MVCWebApp.Models.City;
using MVCWebApp.Models.Country;
using MVCWebApp.Models.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCWebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        //model to be translated  to the database
        public DbSet<Person> People { get; set; }
        //public personSeedDbContext(DbContextOptions<personSeedDbContext> options) : base(options)
        //{

        //}

        //public DbSet<Person> People { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<City>()
                .Property<string>("CountryForeignKey");
            modelBuilder.Entity<Person>()
                .Property<int>("CityForeignKey");


            modelBuilder.Entity<City>()
                .HasOne(c => c.Country)
                .WithMany(co => co.Cities)
            .HasForeignKey("CountryForeignKey");

            modelBuilder.Entity<Person>()
                .HasOne(p => p.City)
                .WithMany(c => c.People)
            .HasForeignKey("CityForeignKey");


            modelBuilder.Entity<Country>().HasData(
                new Country { CountryName = "Sweden" },
                new Country { CountryName = "USA" },
                new Country { CountryName = "UK" });


            modelBuilder.Entity<City>().HasData(
            new { ID = 1, CityName = "Lund", CountryForeignKey = "Sweden" },
            new { ID = 2, CityName = "Gothenburg", CountryForeignKey = "USA" },
            new { ID = 3, CityName = "Stockholm", CountryForeignKey = "UK" });


            modelBuilder.Entity<Person>().HasData(
                new { ID = 1, Name = "John Stwart", PhoneNumber = "0786574567", CityForeignKey = 1 },
                new { ID = 2, Name = "Josefine Gustafsson", PhoneNumber = "0786544567", CityForeignKey = 2 },
                new { ID = 3, Name = "Andrew  Monnet", PhoneNumber = "0786894567", CityForeignKey = 3 });

            //protected override void OnModelCreating(ModelBuilder modelBuilder)
            //{
            //    //default seeding 
            //    modelBuilder.Entity<Person>().HasData(new Person { ID = 1, Name = "Abel Magicho", City = "gothenburg", PhoneNumber = "0743675431" });
            //    modelBuilder.Entity<Person>().HasData(new Person { ID = 2, Name = "Josefin  Larsson", City = "Stockholm", PhoneNumber = "0743345434" });
            //    modelBuilder.Entity<Person>().HasData(new Person { ID = 3, Name = "yonas  walters", City = "dc", PhoneNumber = "0143345444" });
            //}
        }
    }
}