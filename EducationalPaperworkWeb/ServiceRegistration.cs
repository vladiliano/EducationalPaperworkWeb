using EducationalPaperworkWeb.Infrastructure.Infrastructure.DataBase;
using EducationalPaperworkWeb.Service.Service.Implementations;
using EducationalPaperworkWeb.Service.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using EducationalPaperworkWeb.Repository.Repository.Intarfaces.UnitOfWork;
using EducationalPaperworkWeb.Infrastructure.Infrastructure.Repository;

namespace EducationalPaperworkWeb
{
    public static class ServiceRegistration
    {
        public static void RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserAccountService, UserAccountService>();
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
