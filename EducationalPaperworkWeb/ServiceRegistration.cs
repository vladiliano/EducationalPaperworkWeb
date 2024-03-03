using EducationalPaperworkWeb.Infrastructure.Infrastructure.DataBase;

namespace EducationalPaperworkWeb
{
    public static class ServiceRegistration
    {
        public static void RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ApplicationDbContext, ApplicationDbContext>();
        }
    }
}
