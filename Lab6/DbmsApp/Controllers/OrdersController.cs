using DbmsApp.Context;
using DbmsApp.Models;
using DbmsApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbmsApp.Controllers;

public class OrdersController : Controller
{
	private PizzaPlaceContext _db;
	private IUserService _us;

	public OrdersController(PizzaPlaceContext db, IUserService us)
	{
		_db = db;
		_us = us;
	}
	// GET
	[HttpGet("[controller]/[action]/{id:long}")]
	public IActionResult Close(int id)
	{
		var now = DateTime.Now;
		_db.Database.ExecuteSqlRaw($"UPDATE Orders SET dateOfDelivery = " +
								$"{now.ToValues()} FROM Orders WHERE id = {id}");
		

		// Console.WriteLine($"UPDATE Orders SET dateOfDelivery = DATETIMEFROMPARTS(" +
		// 				$"{now.ToValues()}) FROM Orders WHERE id = {id}");
		
		_db.Logs.Add(new Log()
		{
			Logg = $"{DateTime.Now} - ORDER N {id} IS CLOSED"
		});
		_db.SaveChanges();
		return RedirectToAction("Index");
	}
	public IActionResult Index()
	{
		//if((_us.IsAdmin))
		//CHANGE
		var orders = _db.Orders.Where(ordr => ordr.DateOfDelivery == null).ToList();
		//var orders = _db.Orders.ToList();
		for(int i = 0; i < orders.Count; ++i)
		{
			orders[i].Address = _db.Addresses.FromSqlRaw($"SELECT * FROM Addresses WHERE id = {orders[i].AddressId}").First();
			orders[i].User = _db.Users.Find(orders[i].UserId);
		}
		//+User address
		
		return View(orders);
	}
}