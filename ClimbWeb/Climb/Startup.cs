using System.Reflection;
using Climb.Data;
using Climb.Extensions;
using Climb.Services;
using Climb.Services.ModelServices;
using Climb.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NJsonSchema;
using NSwag.AspNetCore;

namespace Climb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("defaultConnection");
            if(string.IsNullOrWhiteSpace(connectionString))
            {
                services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("Test"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            }

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "climb.com",
                        ValidateAudience = true,
                        ValidAudience = "climb",
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = Configuration.GetSecurityKey(),
                        ValidateLifetime = true,
                    };
                    options.SaveToken = true;
                });

            services.AddMvc();

            services.AddTransient<IApplicationUserService, ApplicationUserService>();
            services.AddTransient<IGameService, GameService>();
            services.AddTransient<ILeagueService, LeagueService>();
            services.AddTransient<ISeasonService, SeasonService>();
            services.AddTransient<ISetService, SetService>();

            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddTransient<ITokenHelper, TokenHelper>();
            services.AddTransient<IUrlUtility, UrlUtility>();
            services.AddTransient<IScheduleFactory, RoundRobinScheduler>();
            services.AddTransient<ICdnService, FileStorageCdn>();
            services.AddTransient<IPointService, PointService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                    ReactHotModuleReplacement = true
                });
                app.UseDatabaseErrorPage();
            }

            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseSwaggerUi(typeof(Startup).GetTypeInfo().Assembly,
                settings => settings.GeneratorSettings.DefaultPropertyNameHandling = PropertyNameHandling.CamelCase);
            app.UseMvc();
        }
    }
}