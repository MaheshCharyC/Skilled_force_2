using Microsoft.EntityFrameworkCore;
using Skilled_Force_VS_22.Manager;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvc().AddSessionStateTempDataProvider();
builder.Services.AddSession();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<SkilledForceDB>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SkilledForceDB")));
builder.Services.AddMvc().AddControllersAsServices();


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

app.UseAuthorization();

app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "home",
        pattern: "{controller=Home}/{action=Index}");
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{action=LoginForm}");
    endpoints.MapControllerRoute(
       name: "default",
       pattern: "{controller=Account}/{action=RegistrationForm}");
    endpoints.MapControllerRoute(
       name: "default",
       pattern: "{controller=Account}/{action=CompanyRegistrationForm}");
    endpoints.MapControllerRoute(
       name: "default",
       pattern: "{controller=Job}/{action=PostJob}");
});
/*
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
*/

using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<SkilledForceDB>();
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
    SeedData.Initialize(serviceScope.ServiceProvider);

}

app.Run();
