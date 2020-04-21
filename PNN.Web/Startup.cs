using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PNN.web.Data;
using PNN.Web.Data;
using PNN.Web.Data.Entities;
using PNN.Web.Helpers;

namespace PNN.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //configurar propiedades del usuario
            services.AddIdentity<User, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
                cfg.Password.RequireDigit = false;
                cfg.Password.RequiredUniqueChars = 0;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<DataContext>();


            //inyectamos esta linea una vez agregamos la linea de conexión en appsttings para configurar la BD
            //PM> update-database
            //PM > add - migration InitialDb
            //PM > update - database
            services.AddDbContext<DataContext>(cfg =>
            {
                cfg.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            //agregamos una linea despues de insertar los datos en el SeedDb de Entities.
            services.AddTransient<SeedDb>();
            //Configuramos la inyección del UserHelper donde lo necesitemos
            services.AddScoped<IUserHelper, UserHelper>();
            //una vez creamos el ICombosHelper lo inyectamos aqui
            services.AddScoped<ICombosHelper, CombosHelper>();
            //agregamos el IConverterHelper lo inyectamos aqui
            services.AddScoped<IConverterHelper, ConverterHelper>();
            //agregamos el IImageHelper lo inyectamos aqui
            services.AddScoped<IImageHelper, ImageHelper>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //se agrega esta linea revisar tutorial 7
            app.UseAuthentication();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
