using Microsoft.AspNetCore.Mvc;
using DbmsApp.Context;

namespace DbmsApp.Controllers;

public class ListsController : Controller
{
	private readonly PizzaPlaceContext _db;
	// GET
	public ListsController(PizzaPlaceContext cont)
	{
		_db = cont;
	}
	public IActionResult UsersList()
	{
		return View(_db.Users.ToList());
	}
}