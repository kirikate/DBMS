using DbmsApp.Context;
using DbmsApp.Models;
using DbmsApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;

namespace DbmsApp.Controllers;

public class CatalogController : Controller
{
	private PizzaPlaceContext _db;
	// GET
	private readonly IUserService _us;

	public class GoodDto
	{
		public long Id { get; set; }
		public string Name { get; set; } = null!;
		public string? Ingredients { get; set; }
		public decimal Price { get; set; }
		public string? Size { get; set; }
		public long? ProductId { get; set; }
	}
	
	public CatalogController(PizzaPlaceContext db, IUserService us)
	{
		_db = db;
		_us = us;
	}
	public IActionResult Index()
	{
		// var res = from g in _db.Goods
		// 	join p in _db.Products on g.ProductId equals p.Id
		// 	select new GoodDto(){Id=g.Id, Price = g.Price, Ingredients = p.Ingredients, Size = g.Size, Name = p.Name};

		var res = _db.ReadableGoods.FromSqlRaw($"SELECT * FROM ReadableGoods");

		//CHANGE
		var ls = _db.Goods.ToList();
		foreach(var it in ls)
		{
			it.Product = _db.Products.FromSqlRaw($"SELECT * FROM Products WHERE id = {it.ProductId}").First();
		}
		
		// _db.Logs.Add(new Log()
		// {
		// 	Logg = $"{DateTime.Now} - SELECT FROM GOODS FOR CATALOG"
		// });
		_db.Database.ExecuteSqlRaw(
			$"INSERT INTO Logs([log]) VALUES (FORMAT(GETDATE(), 'yyyy-MM-dd HH:mm:ss') + N' - SELECT FROM GOODS FOR CATALOG')");
		_db.SaveChanges();
		return View(res);
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