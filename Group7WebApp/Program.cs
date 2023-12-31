using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Group7WebApp.Data;
using Group7WebApp.Areas.Identity.Data;
using Group7WebApp.Helpers;
using Group7WebApp.Helpers.Interface;
using Group7WebApp.Helpers.Implementation;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AuthDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AuthDbContextConnection' not found.");

builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(connectionString));
//builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(connectionString));

// Change RequireConfirmedAccount to false
builder.Services.AddDefaultIdentity<WebAppUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IAuthorizationMiddlewareService, AuthorizationMiddlewareService>();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AuthDbContext>();
    var userM = services.GetRequiredService<UserManager<WebAppUser>>();
    var roleM = services.GetRequiredService<RoleManager<IdentityRole>>();

    await MyIdentityDbInitializer.SeedData(userM, roleM, context);
}


app.Run();
