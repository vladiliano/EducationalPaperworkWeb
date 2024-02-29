using EducationalPaperworkWeb.Infrastructure.Infrastructure.Conventions;
using EducationalPaperworkWeb.Infrastructure.Infrastructure.ViewEngines;

namespace EducationalPaperworkWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            ConfigureServices(builder.Services);

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(o => o.Conventions.Add(new FeatureConvention()))
                  .AddRazorOptions(options =>
                  {
                      // {0} - Action Name
                      // {1} - Controller Name
                      // {2} - Feature Name
                      // Replace normal view location entirely
                      options.ViewLocationFormats.Clear();
                      options.ViewLocationFormats.Add("/Features/{2}/{1}/{0}.cshtml");
                      options.ViewLocationFormats.Add("/Features/{2}/{0}.cshtml");
                      options.ViewLocationFormats.Add("/Features/Shared/{0}.cshtml");
                      options.ViewLocationExpanders.Add(new FeatureFoldersRazorViewEngine());
                  });
        }
    }
}
