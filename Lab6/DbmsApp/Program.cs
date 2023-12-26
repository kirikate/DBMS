using DbmsApp.Context;
using DbmsApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc(options => { options.EnableEndpointRouting = false;});
builder.Services.AddDbContext<PizzaPlaceContext>();
builder.Services.AddSingleton<IUserService, UserService>();
var app = builder.Build();

app.UseMvc();
app.UseStaticFiles();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}");
app.MapControllerRoute("ids", "{controller}/{action}/{id:int}");
HttpContext context = new DefaultHttpContext();
app.Run();