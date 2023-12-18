using Microsoft.AspNetCore.Mvc;

namespace DbmsApp.Controllers;

public class HomeController : Controller
{
	// GET
	public IActionResult Index()
	{
		return View();
	}
}