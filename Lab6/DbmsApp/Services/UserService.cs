using System.Collections;
using DbmsApp.Context;
using DbmsApp.Models;
using Microsoft.EntityFrameworkCore;

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
		User? usr = db.Users.FromSqlRaw($"SELECT * FROM Users WHERE email=N'{email}' AND password={password}").First();
		db.Logs.Add(new Log()
		{
			Logg = $"{DateTime.Now} - SELECT FROM USERS FOR LOGIN {email}"
		});
		db.SaveChanges();
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
		User? usr = db.Users.FromSqlRaw($"SELECT * FROM Users WHERE email=N'{user.Email}' AND password={user.Password}").First();
		if (usr != null) return false;

		//CHANGE
		db.Users.Add(user);
		db.Logs.Add(new Log()
		{
			Logg = $"{DateTime.Now} - INSERT INTO Users object {user.Email} {user.FirstName} {user.LastName}"
		});
		db.SaveChanges();
		
		CurrentUser = user;
		_goods.Clear();
		_coups.Clear();
		return true;
	}
}