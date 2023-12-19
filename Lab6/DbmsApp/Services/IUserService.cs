using DbmsApp.Context;
using DbmsApp.Models;

namespace DbmsApp.Services;

public interface IUserService
{
	public User? CurrentUser { get; }
	public Dictionary<long, int> GoodsInCart { get; set; }
	public Dictionary<long, int> CouponsInCart { get; set; }
	public bool IsAdmin { get; }
	public bool IsStaff { get; }
	
	public bool Login(string email, int password, PizzaPlaceContext db);
	public void Logout();
	public bool Register(User usr, PizzaPlaceContext db);
}