using DbmsApp.Context;
using DbmsApp.Models;

namespace DbmsApp.Services;

public interface IUserService
{
	public User? CurrentUser { get; }
	public IEnumerable<(Product prod, int count)> ProdsInCart { get; }
	public IEnumerable<(Coupon coup, int count)> CouponsInCart { get; }
	public bool IsAdmin { get; }	
	
	public bool Login(string email, int password, PizzaPlaceContext db);
	public void Logout();
	public bool Register(User usr, PizzaPlaceContext db);
}