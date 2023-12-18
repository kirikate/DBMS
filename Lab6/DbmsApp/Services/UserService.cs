using System.Collections;
using DbmsApp.Context;
using DbmsApp.Models;

namespace DbmsApp.Services;

public class UserService: IUserService
{
	public User? CurrentUser { get; private set; }

	private List<(Product prod, int count)> _prods = new();
	private List<(Coupon coup, int count)> _coups = new();

	public IEnumerable<(Product prod, int count)> ProdsInCart => _prods;

	public IEnumerable<(Coupon coup, int count)> CouponsInCart => _coups;
	public bool IsAdmin => CurrentUser?.Role == "ADM";

	//private readonly PizzaPlaceContext _db;

	public bool Login(string email, int password, PizzaPlaceContext db)
	{
		
		User? usr = db.Users.FirstOrDefault(usr => usr.Email == email && usr.Password == password);
		if (usr is null) return false;

		CurrentUser = usr;
		_prods.Clear();
		_coups.Clear();

		return true;
	}

	public void Logout()
	{
		CurrentUser = null;
		_prods.Clear();
		_coups.Clear();
	}

	public bool Register(User user, PizzaPlaceContext db)
	{
		User? usr = db.Users.FirstOrDefault(usr => usr.Email == user.Email);
		if (usr != null) return false;

		//CHANGE
		db.Users.Add(user);
		db.SaveChanges();
		
		CurrentUser = user;
		_prods.Clear();
		_coups.Clear();
		return true;
	}
}