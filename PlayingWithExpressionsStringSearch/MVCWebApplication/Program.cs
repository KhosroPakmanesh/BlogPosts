using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCWebApplication.DbContexts;

namespace MVCWebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<OrderDbContext>(options =>
            {
                options.UseInMemoryDatabase("orderDb");
            });
            
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add
                (new AutoValidateAntiforgeryTokenAttribute());
            });

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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            using var serviceScope = app.Services.CreateScope();
            var orderDbContext= serviceScope.ServiceProvider
                .GetRequiredService<OrderDbContext>();
            orderDbContext.Database.EnsureCreated();

            app.Run();
        }
    }
}
