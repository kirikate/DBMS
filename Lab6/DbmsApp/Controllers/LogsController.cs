using DbmsApp.Context;
using Microsoft.AspNetCore.Mvc;

namespace DbmsApp.Controllers;

public class LogsController : Controller
{
	private readonly PizzaPlaceContext _db;
	public LogsController(PizzaPlaceContext db)
	{
		_db = db;
	}
	// GET
	public IActionResult Index()
	{
		return View(_db.Logs.ToList());
	}
}