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

		_db.Database.ExecuteSqlRaw($"EXEC CloseOrder @orderId={id}");

		_db.SaveChanges();
		return RedirectToAction("Index");
	}
	public IActionResult Index()
	{
		//if((_us.IsAdmin))
		//CHANGE
		// var orders = _db.Orders.FromSqlRaw(ordr => ordr.DateOfDelivery == null).ToList();
		var orders = _db.Orders.FromSqlRaw($"SELECT * FROM Orders WHERE dateOfDelivery IS NULL").ToList();

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