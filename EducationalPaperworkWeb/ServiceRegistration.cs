using EducationalPaperworkWeb.Infrastructure.Infrastructure.DataBase;
using Microsoft.EntityFrameworkCore;

namespace EducationalPaperworkWeb
{
    public static class ServiceRegistration
    {
        public static void RegisterApplicationServices(this IServiceCollection services)
        {

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
