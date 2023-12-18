using Microsoft.EntityFrameworkCore;

using DbmsApp.Models;
namespace DbmsApp.Context;

public class AppDbContext: DbContext
{
	public DbSet<User> Users { get; set; } = null!;
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		try
		{
			optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=PizzaPlace;Trusted_Connection=True;");
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			throw ex;
		}
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>().ToTable("Users");
	}
}