using BuildingStore.Models;
using BuildingStore.Services.BusinessLogic;
using BuildingStore.Services.Patterns.Bridge;
using BuildingStore.Services.Patterns.Observer;
using BuildingStore.Services.Patterns.Proxy;
using BuildingStore.Services.Patterns.Proxy.ProtectionProxy;

using Microsoft.EntityFrameworkCore;

namespace BuildingStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(options =>options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<AuthorizationService>();
            builder.Services.AddScoped<IAdminLoginProxy, ProtectionAdminProxy>();
            builder.Services.AddScoped<RealAdminLoginProxy>();
            builder.Services.AddScoped<UserService>();
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddTransient<IDocumentSenderBridge, SmtpEmailSenderBridge>();
            builder.Services.AddTransient<OrderDocumentBridge, ReceiptDocumentBridge>();
            builder.Services.AddScoped<IOrderObserver, DatabaseObserver>();
            builder.Services.AddScoped<IOrderObserver, EmailObserver>();
            builder.Services.AddScoped<ProductService>();

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
                pattern: "{controller=Products}/{action=Products}/{id?}");  

            app.Run();
        }
    }
}
