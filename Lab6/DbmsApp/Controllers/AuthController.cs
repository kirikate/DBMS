using System.ComponentModel.DataAnnotations;
using DbmsApp.Context;
using DbmsApp.Services;
using DbmsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using AutoMapper;

namespace DbmsApp.Controllers;

public class AuthController : Controller
{
	// GET
	private readonly IUserService _us;
	private readonly PizzaPlaceContext _db;
	
	private static int Hashie(string value)
	{
		int num = 352654597;
		int num2 = num;

		for (int i = 0; i < value.Length; i += 4)
		{
			int ptr0 = value[i] << 16;
			if (i + 1 < value.Length)
				ptr0 |= value[i + 1];

			num = (num << 5) + num + (num >> 27) ^ ptr0;

			if (i + 2 < value.Length)
			{
				int ptr1 = value[i + 2] << 16;
				if (i + 3 < value.Length)
					ptr1 |= value[i + 3];
				num2 = (num2 << 5) + num2 + (num2 >> 27) ^ ptr1;
			}
		}

		return num + num2 * 1566083941;
	}
	
	public AuthController(IUserService us, PizzaPlaceContext db)
	{
		_us = us;
		_db = db;
	}
	
	public class LoginModel
	{
		[Required] 
		public string Email { get; set; }
		
		[Required]
		public string Password { get; set; }
	}

	public class RegisterModel
	{
		[Required]
		[Display(Name = "Имя")]
		public string FirstName { get; set; } = null!;

		[Required]
		[Display(Name = "Фамилия")]
		public string LastName { get; set; } = null!;
		
		[Required]
		[RegularExpression(@"[A-Za-z0-9]{8,}")]
		[Display(Name="Пароль")]
		public string Password { get; set; }
		
		[Required]
		public string Email { get; set; } = null!;
		
		public string? Phone { get; set; }
	}
	
	[HttpGet]
	public IActionResult Login()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Login(LoginModel lm)
	{
		if (!ModelState.IsValid) return Redirect("/auth/login");
		
		var res = _us.Login(lm.Email, Hashie(lm.Password), _db);
		if (!res) return View();

		return Redirect("/");
	}

	public IActionResult Logout()
	{
		_us.Logout();
		return Redirect("/");
	}

	[HttpGet]
	public IActionResult Register()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Register(RegisterModel rm)
	{
		MapperConfiguration config = new MapperConfiguration(opt=>
		{
			opt.CreateMap<RegisterModel, User>().ForMember(dest => dest.Password, opt =>
			{
				opt.MapFrom(src => Hashie(src.Password));
			});
		});
		Mapper mapper = new Mapper(config);

		var usr = mapper.Map<User>(rm);
		usr.Role = "USR";
		var res = _us.Register(usr, _db);
		if (!res) return View();

		return Redirect("/");
	}
}