using ApplicationCore.Entities;
using ApplicationCore.RepositoryInterfaces;
using ApplicationCore.ServiceInterfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieShop.MVC.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieShop.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllersWithViews(options=>options.Filters.Add(typeof(MovieShopHeaderFilterAttribute)));
            services.AddDbContext<MovieShopDbContext>(options => options.UseSqlServer(
               Configuration.GetConnectionString("MovieShopDbConnection")
           ));
            services.AddControllersWithViews();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<ICastService, CastService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddScoped<IMovieRepository, MovieRepository>();
            //services.AddScoped<IAsyncRepository<Movie>, EfRepository<Movie>>();
            services.AddScoped<IAsyncRepository<Genre>, EfRepository<Genre>>();
            //services.AddScoped<IAsyncRepository<User>, EfRepository<User>>();
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<ICastRepository, CastRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();

            services.AddScoped<MovieShopHeaderFilterAttribute>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option=>
            {
                option.Cookie.Name = "MovieShopAuthCookie";
                option.ExpireTimeSpan = TimeSpan.FromHours(2);
                option.LoginPath = "/Account/login";
            });

            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
