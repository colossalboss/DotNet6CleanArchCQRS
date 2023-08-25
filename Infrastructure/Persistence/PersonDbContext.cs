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
			optionsBuilder.UseMySql("Server=localhost;Database=testdb3;User=root;Password=1111", new MySqlServerVersion(new Version(8, 0, 25)));
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}

		public virtual DbSet<Person> Persons { get; set; }
	}
}

