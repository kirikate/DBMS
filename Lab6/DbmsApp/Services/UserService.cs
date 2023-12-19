using System.Collections;
using DbmsApp.Context;
using DbmsApp.Models;

namespace DbmsApp.Services;

public class UserService: IUserService
{
	public User? CurrentUser { get; private set; }

	private Dictionary<long, int> _goods = new();
	private Dictionary<long, int> _coups = new();

	public Dictionary<long, int> GoodsInCart
	{
		get => _goods;
		set => _goods = value;
	}

	public Dictionary<long, int> CouponsInCart
	{
		get => _coups;
		set => _coups = value;
	}

	public bool IsAdmin => CurrentUser?.Role == "ADM";
	public bool IsStaff => CurrentUser?.Role != "USR";

	//private readonly PizzaPlaceContext _db;

	public bool Login(string email, int password, PizzaPlaceContext db)
	{
		//CHANGE
		User? usr = db.Users.FirstOrDefault(usr => usr.Email == email && usr.Password == password);
		if (usr is null) return false;

		CurrentUser = usr;
		//_goods.Clear();
		//_coups.Clear();

		return true;
	}

	public void Logout()
	{
		CurrentUser = null;
		_goods.Clear();
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
		_goods.Clear();
		_coups.Clear();
		return true;
	}
}