using DbmsApp.Context;
using DbmsApp.Models;
using DbmsApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace DbmsApp.Controllers;

public class CatalogController : Controller
{
	private PizzaPlaceContext _db;
	// GET
	private readonly IUserService _us;

	public CatalogController(PizzaPlaceContext db, IUserService us)
	{
		_db = db;
		_us = us;
	}
	public IActionResult Index()
	{
		//имя
		//размер
		//цена
		
		
		//CHANGE
		var ls = _db.Goods.ToList();
		foreach(var it in ls)
		{
			it.Product = _db.Products.FromSqlRaw($"SELECT * FROM Products WHERE id = {it.ProductId}").First();
		}
		
		_db.Logs.Add(new Log()
		{
			Logg = $"{DateTime.Now} - SELECT FROM GOODS FOR CATALOG"
		});
		_db.SaveChanges();
		return View(ls);
	}
	
	[HttpGet("{id:long}")]
	public IActionResult AddToCart(long id)
	{
		//var good = _db.Goods.Find(id);

		try
		{
			_us.GoodsInCart[id] += 1;
		}
		catch (KeyNotFoundException)
		{
			_us.GoodsInCart[id] = 1;
		}

		foreach ((var key, var value) in _us.GoodsInCart)
		{
			Console.WriteLine($"GoodId {key} - count = {value}");
		}
		
		return Redirect("/Catalog");
	}
}