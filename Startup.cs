using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using vega.Persistence;
using vega.Core;
using AutoMapper;
using vega.Core.Models;
using vega.Controllers;

namespace Vega
{
  public class Startup
  {
    public Startup(IHostingEnvironment env)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

        if (env.IsDevelopment())
            builder = builder.AddUserSecrets<Startup>();

        builder = builder.AddEnvironmentVariables();
        Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<PhotoSettings>(Configuration.GetSection("PhotoSettings"));

        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IPhotoRepository, PhotoRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IPhotoService, PhotoService>();
        services.AddTransient<IPhotoStorage, FileSystemPhotoStorage>();

        services.AddAutoMapper();

        services.AddDbContext<VegaDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));

        services.AddAuthorization(options => {
          options.AddPolicy(Policies.RequireAdminRole, policy => policy.RequireClaim("https://vega.com/roles", "Admin"));
        });

        // Add framework services.
        services.AddMvc();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
        loggerFactory.AddConsole(Configuration.GetSection("Logging"));
        loggerFactory.AddDebug();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {
                HotModuleReplacement = true
            });
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
        }

        app.UseStaticFiles();

        var options = new JwtBearerOptions
        {
          Audience = "https://api.vega.com",
          Authority = "https://vegaproject.auth0.com/"
        };
        app.UseJwtBearerAuthentication(options);

        app.UseMvc(routes =>
        {
            routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");

            routes.MapSpaFallbackRoute(
                name: "spa-fallback",
                defaults: new { controller = "Home", action = "Index" });
        });
    }
  }
}
