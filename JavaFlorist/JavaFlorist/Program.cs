using System.Text.Json.Serialization;
using JavaFlorist.Helpers;
using JavaFlorist.Repository;
using JavaFlorist.Services;
using JavaFlorist.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers().AddJsonOptions(x =>
                                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//connect db
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
//
builder.Services.AddControllersWithViews().AddSessionStateTempDataProvider();
builder.Services.AddMvc().AddSessionStateTempDataProvider();

// use session

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
  options.LoginPath = "/User/Login"; // Đường dẫn đến trang đăng nhập
}
);
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// register paypalClient Singletion()
builder.Services.AddSingleton(x => new PaypalClient(
    builder.Configuration["PaypalOption:AppId"],
    builder.Configuration["PaypalOption:AppSecret"],
    builder.Configuration["PaypalOption:Mode"]
));

builder.Services.AddSingleton<IVnPayService, VnPayService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

