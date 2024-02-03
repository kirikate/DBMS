using DbmsApp.Context;
using DbmsApp.Models;
using DbmsApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using NuGet.Configuration;

namespace DbmsApp.Controllers;

public class CartController : Controller
{
	private readonly PizzaPlaceContext _db;
	private readonly IUserService _us;
	public CartController(PizzaPlaceContext db, IUserService us)
	{
		_db = db;
		_us = us;
	}
	// GET
	public IActionResult Index()
	{
		List<(Good, int)> lsg = new();
		List<(Coupon, int, List<(Good, int)>)> lsc = new();
		Console.WriteLine("From cart contr");
		if (!_us.GoodsInCart.Any(lol=> lol.Value > 0) && !_us.CouponsInCart.Any(lol=> lol.Value > 0))
		{
			return RedirectToAction("Index", "Catalog");
		}
		foreach ((var key, var value) in _us.GoodsInCart)
		{
			Console.WriteLine($"GoodId {key} - count = {value}");
		}
		
		foreach ((var goodId, var count) in _us.GoodsInCart)
		{
			if (count <= 0) continue; 
			Good gd = _db.Goods.Find(goodId);
			gd.Product = _db.Products.Find(gd.ProductId);
			lsg.Add((gd, count));
		}
		
		foreach ((var couponId, var count) in _us.CouponsInCart)
		{
			if (count <= 0) continue; 
			Coupon cp = _db.Coupons.Find(couponId);
			
			List <(Good good, int count)> lst = new();
			var gtc = _db.GoodsToCoupons.FromSqlRaw($"SELECT * FROM" +
													$" GoodsToCoupons WHERE couponId={cp.Id}").ToList();

			foreach (var it in gtc)
			{
				var good = _db.Goods.FromSqlRaw($"SELECT * FROM Goods WHERE id = {it.ProductId}").First();
				good.Product = _db.Products.FromSqlRaw($"SELECT * FROM Products WHERE id={good.ProductId}").First();
				lst.Add((good, it.Count));
			}
			
			lsc.Add((cp, count, lst)!);
		}
		// foreach ((var good, var count) in lsg)
		// {
		// 	Console.WriteLine($"lsg{good.Product.Name} - lsg{count}");
		// }
		
		var res = (lsg, lsc);
		return View((lsg, lsc));
	}

	public IActionResult AddOneProductTo(int id)
	{
		_us.GoodsInCart[id] += 1;
		return RedirectToAction("Index");
	}

	public IActionResult RemoveOneProduct(int id)
	{
		_us.GoodsInCart[id] -= 1;
		return RedirectToAction("Index");
	}
	
	public IActionResult ClearOneProduct(int id)
	{
		_us.GoodsInCart[id] = 0;
		return RedirectToAction("Index");
	}

	public IActionResult ClearOneCoupon(int id)
	{
		_us.CouponsInCart[id] = 0;
		return RedirectToAction("Index");
	}
	
	public IActionResult AddOneCouponTo(int id)
	{
		_us.CouponsInCart[id] += 1;
		return RedirectToAction("Index");
	}

	public IActionResult RemoveOneCoupon(int id)
	{
		_us.CouponsInCart[id] -= 1;
		return RedirectToAction("Index");
	}


	[HttpPost]
	public IActionResult Index(string address, string? entrance, string? number)
	{
		//Console.WriteLine($"{address} - {entrance} - {number}");
		if (_us.CurrentUser is null) return RedirectToAction("Login", "Auth");
		
		var addr = new Address()
		{
			Adress = address,
			Entrance = entrance,
			Number = number
		};
		
		//CHANGE
		_db.Database.ExecuteSqlRaw(
			$"INSERT INTO Addresses(adress, entrance, number, userId) " + 
			$"VALUES({addr.Adress}, {addr.Entrance}, {addr.Number}, {_us.CurrentUser.Id})");
		_db.SaveChanges();
		//var smth = _db.Addresses.First(addr => addr.UserId == _us.CurrentUser.Id && addr.Adress == address);
		var smth = _db.Addresses.FromSqlRaw($"SELECT * FROM Addresses WHERE userId={_us.CurrentUser.Id}" +
											$" AND adress={address}").First();
		Console.WriteLine($"id of new address {smth.Id}");
		var ordr = new Order()
		{
			AddressId = smth.Id,
			DateOfOrder = DateTime.Now,
			UserId = _us.CurrentUser.Id,
			Price = 0
		};
		_db.Database.ExecuteSqlRaw($"INSERT INTO Orders(userId, dateOfOrder, dateOfDelivery, price, addressId) " +
									$"VALUES ({ordr.UserId}, {ordr.DateOfOrder.ToValues()}, NULL, 0, {ordr.AddressId})");
		//var eo = _db.Orders.Add(ordr);
		var eo = _db.Orders.FromSqlRaw($"SELECT * FROM Orders WHERE userId = {ordr.UserId} " +
										$"AND dateOfOrder={ordr.DateOfOrder.ToValues()}").First();

		_db.SaveChanges();
		Console.WriteLine("Is there anybody here...");
		Console.WriteLine($"id of new order is {eo.Id}");
		foreach ((var goodId, var count) in _us.GoodsInCart)
		{

			_db.Database.ExecuteSqlRaw($"INSERT INTO GoodsToOrders(orderId, productId, [count]) " +
									$"VALUES ({eo.Id}, {goodId}, {count})");
			_db.SaveChanges();
		}

		foreach ((var coupId, var count) in _us.CouponsInCart)
		{
			_db.Database.ExecuteSqlRaw($"INSERT INTO CouponsToOrders(orderId, couponId, [count]) " +
										$"VALUES ({eo.Id}, {coupId}, {count})");
			_db.SaveChanges();
		}

		_db.SaveChanges();
		_us.GoodsInCart.Clear();
		_us.GoodsInCart.Clear();
		return Redirect("/");
	}
}