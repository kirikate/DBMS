using DbmsApp.Context;
using DbmsApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbmsApp.Controllers;

public class UserController : Controller
{
	private readonly PizzaPlaceContext _db;
	private readonly IUserService _us;
	public UserController(PizzaPlaceContext db, IUserService us)
	{
		_db = db;
		_us = us;
	}
	// GET

	public class UserDataModel
	{
		public string FirstName { get; set; } = null!;

		public string LastName { get; set; } = null!;
		
		public string Email { get; set; } = null!;
		
		public string? Phone { get; set; }
	}
	public IActionResult Index()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Index(UserDataModel model)
	{
		Console.WriteLine($"UPDATE Users SET first_name=N'{model.FirstName}', last_name=N'{model.LastName}', " +
						$"email=N'{model.Email}', phone='{model.Phone}' WHERE id={_us.CurrentUser.Id}");
		
		_db.Database.ExecuteSqlRaw($"UPDATE Users SET first_name=N'{model.FirstName}', last_name=N'{model.LastName}', " +
									$"email=N'{model.Email}',phone='{model.Phone}' WHERE id={_us.CurrentUser.Id}");
		_db.SaveChanges();
		_us.CurrentUser.FirstName = model.FirstName;
		_us.CurrentUser.LastName = model.LastName;
		_us.CurrentUser.Email = model.Email;
		_us.CurrentUser.Phone = model.Phone;
		return View();
	}
}