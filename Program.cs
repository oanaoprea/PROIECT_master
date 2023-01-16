using Microsoft.EntityFrameworkCore;
using PROIECT.Data;
using PROIECT.Hubs;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PlantsContext>(options =>

options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>();

builder.Services.AddSignalR();

builder.Services.AddDbContext<IdentityContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.AllowedForNewUsers = true;
});

builder.Services.ConfigureApplicationCookie(opts => {
    opts.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddAuthorization(opts => { opts.AddPolicy("OnlySales", policy => { policy.RequireClaim("Departament", "Sales"); }); });

builder.Services.AddAuthorization(opts => { opts.AddPolicy("SalesManager", policy => { policy.RequireRole("Manager"); policy.RequireClaim("Departament", "Sales"); }); });
builder.Services.ConfigureApplicationCookie(opts => {
    opts.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    DbInitializer.Initialize(services);
}

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
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ChatHub>("/Chat");
app.MapRazorPages();

app.Run();
