using EducationalPaperworkWeb.Infrastructure.Infrastructure.DataBase;
using EducationalPaperworkWeb.Service.Service.Implementations;
using EducationalPaperworkWeb.Service.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using EducationalPaperworkWeb.Repository.Repository.Interfaces.UnitOfWork;
using EducationalPaperworkWeb.Infrastructure.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using EducationalPaperworkWeb.Infrastructure.Infrastructure.DataStorage.Interface;
using EducationalPaperworkWeb.Infrastructure.Infrastructure.DataStorage.BlobStorage;
using EducationalPaperworkWeb.Service.Service.Implementations.ChatHub;

namespace EducationalPaperworkWeb
{
    public static class ServiceRegistration
    {
        public static void RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
            services.AddHttpContextAccessor();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => { options.Cookie.Name = "ApplicationCookie"; });
            services.AddAuthorization();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserAccountService, UserAccountService>();

            services.AddSingleton<IDataStorage, BlobStorage>();

            services.AddSignalR(hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true;
                hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(10);
            });

            services.AddSingleton<ChatHub>();
        }

        public static void RegisterDataBase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }
    }
}
