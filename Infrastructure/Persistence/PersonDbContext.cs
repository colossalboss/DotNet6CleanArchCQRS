using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
	public class PersonDbContext : DbContext
	{

		//public PersonDbContext(DbContextOptions<PersonDbContext> options) : base(options)
		//{
		//}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			//optionsBuilder.UseSqlite("Data Source=app.db");
			//optionsBuilder.UseMySql("Server=localhost;Database=testdb3;User=root;Password=1111", new MySqlServerVersion(new Version(8, 0, 25)));
			optionsBuilder.UseNpgsql("host=localhost;port=5432;database=mydb;username=postgres;password=password;sslmode=prefer;");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Person>().HasData(
                    new Person ("Test User") { Email = "test@email.com", Id = 1 },
                    new Person("Test2 User2") { Email = "test2@email.com", Id = 2 }
                );

            modelBuilder.Entity<Address>().HasData(
                    new Address { City = "Lagos", PersonId = 1, Id = 1 },
                    new Address { City = "Abuja", PersonId = 2, Id = 2 }
                );
            base.OnModelCreating(modelBuilder);
		}

		public virtual DbSet<Person> Persons { get; set; }
		public virtual DbSet<Address> Addresses { get; set; }
	}
}

