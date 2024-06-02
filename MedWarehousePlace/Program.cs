using BLL.Interfaces;
using BLL.Services;
using DAL.Interfaces;
using DAL.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using BLL.Infrastructure.MappingProfiles;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MedWarehousePlace
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<DAL.EF.ApplicationDbContext>(options => options.UseSqlServer(
                builder.Configuration.GetConnectionString("connectionString")));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddTransient<IWarehouseService, WarehouseService>();
            builder.Services.AddTransient<IInventoryService, InventoryService>();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddAutoMapper(typeof(WarehouseProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(ItemProfile).Assembly);
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(x => x.LoginPath = "/account/login");

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

            app.Run();
        }
    }
}
