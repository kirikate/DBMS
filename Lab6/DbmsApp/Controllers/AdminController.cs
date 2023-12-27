using DbmsApp.Context;
using DbmsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbmsApp.Controllers;

public class AdminController : Controller
{
	private readonly PizzaPlaceContext _db;
	public AdminController(PizzaPlaceContext db)
	{
		_db = db;
	}

	public IActionResult Users()
	{
		var users = _db.Users.FromSqlRaw("SELECT * FROM Users").ToList();
		
		return View(users);
	}
	
	public IActionResult MakeUsr(int id)
	{
		_db.Database.ExecuteSqlRaw($"UPDATE Users SET role = N'USR' WHERE id = {id}");
		_db.Logs.Add(new Log()
		{
			Logg = $"{DateTime.Now} - Role of user {id} changed to USR"
		});
		_db.SaveChanges();
		return RedirectToAction("Users");
	}
	
	public IActionResult MakeStf(int id)
	{
		_db.Database.ExecuteSqlRaw($"UPDATE Users SET role = N'STF' WHERE id = {id}");
		_db.Logs.Add(new Log()
		{
			Logg = $"{DateTime.Now} - Role of user {id} changed to STF"
		});
		_db.SaveChanges();
		return RedirectToAction("Users");
	}
	
	public IActionResult MakeAdm(int id)
	{
		_db.Database.ExecuteSqlRaw($"UPDATE Users SET role = N'ADM' WHERE id = {id}");
		_db.Logs.Add(new Log()
		{
			Logg = $"{DateTime.Now} - Role of user {id} changed to STF"
		});
		_db.SaveChanges();
		return RedirectToAction("Users");
	}
	
	public IActionResult Coupons()
	{
		var res = _db.Coupons.FromSqlRaw("SELECT * FROM Coupons").ToList();
		List<(Coupon, List<(Good, int)>)> model = new();
		
		foreach (var coupon in res)
		{
			List <(Good good, int count)> lst = new();
			var gtc = _db.GoodsToCoupons.FromSqlRaw($"SELECT * FROM" +
													$" GoodsToCoupons WHERE couponId={coupon.Id}").ToList();

			foreach (var it in gtc)
			{
				var good = _db.Goods.FromSqlRaw($"SELECT * FROM Goods WHERE id = {it.ProductId}").First();
				good.Product = _db.Products.FromSqlRaw($"SELECT * FROM Products WHERE id={good.ProductId}").First();
				lst.Add((good, it.Count));
			}
			model.Add((coupon, lst));
		}
		return View(model);
	}
	
	[HttpPost]
	public IActionResult Coupons(Coupon coup)
	{
		_db.Database.ExecuteSqlRaw($"INSERT INTO Coupons(number, price, dateOfStart, dateOfExpiration) VALUES"+
									$" ({coup.Number}, {coup.Price}, {coup.DateOfStart.ToValues()},"
									+$"{coup.DateOfExpiration.ToValues()})");

		_db.SaveChanges();
		return RedirectToAction("Coupons");
	}

	public IActionResult CouponPage(int id)
	{
		//coupon
		var coupon = _db.Coupons.FromSqlRaw($"SELECT * FROM Coupons WHERE id={id}").First();
		
		//products in coupon
		List <(Good good, int count)> lst = new();
		var gtc = _db.GoodsToCoupons.FromSqlRaw($"SELECT * FROM" +
												$" GoodsToCoupons WHERE couponId={coupon.Id}").ToList();

		foreach (var it in gtc)
		{
			var good = _db.Goods.FromSqlRaw($"SELECT * FROM Goods WHERE id = {it.ProductId}").First();
			good.Product = _db.Products.FromSqlRaw($"SELECT * FROM Products WHERE id={good.ProductId}").First();
			lst.Add((good, it.Count));
		}
		//model.Add((coupon, lst));
		//allproducts

		var goods = _db.Goods.FromSqlRaw($"SELECT * FROM Goods").ToList();
		for(int i = 0; i < goods.Count; ++i)
		{
			goods[i].Product = _db.Products.
				FromSqlRaw($"SELECT * FROM Products WHERE id={goods[i].ProductId}").First();
			
		}

		_db.SaveChanges();
		return View((coupon, lst, goods));
	}
	
	[HttpPost]
	public IActionResult CouponPage(long id, long good, int count)
	{
		Console.WriteLine($"INSERT INTO GoodsToCoupons(couponId, productId, count) " +
						$"VALUES ({id}, {good}, {count})");
		_db.Database.ExecuteSqlRaw($"INSERT INTO GoodsToCoupons(couponId, productId, count) " +
									$"VALUES ({id}, {good}, {count})");
		_db.SaveChanges();
		return RedirectToAction("CouponPage", new {
		id=id});
	}

	public IActionResult DeleteCoupon(int id)
	{
		if(_db.CouponsToOrders.FromSqlRaw($"SELECT * FROM CouponsToOrders WHERE couponId={id}").Any())
		{
			return RedirectToAction("CouponPage");
		}
		
		_db.Database.ExecuteSqlRaw($"DELETE FROM GoodsToCoupons WHERE couponId={id}");
		_db.Database.ExecuteSqlRaw($"DELETE FROM Coupons WHERE id={id}");
		_db.SaveChanges();
		return RedirectToAction("Coupons");
	}
}