using Northwind.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureSqlConnectionFactory(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureRailwayPattern();
builder.Services.ConfigureAuthorizationPolicy();
builder.Services.ConfigureMediatR();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAutoMigrations();

app.UseRouting();

app.UseAuthorization();
app.UseStatusCodePagesWithReExecute("/Account/AccessDenied");
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();