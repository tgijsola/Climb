﻿using System.Collections.Generic;
using System.Reflection;
using Climb.Data;
using Climb.Services;
using Climb.Services.ModelServices;
using Climb.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            ConfigureDB(services);

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddCookieTempDataProvider();

            services.AddTransient<IApplicationUserService, ApplicationUserService>();
            services.AddTransient<IGameService, GameService>();
            services.AddTransient<ILeagueService, LeagueService>();
            services.AddTransient<ISeasonService, SeasonService>();
            services.AddTransient<ISetService, SetService>();

            services.AddSingleton<IEmailSender, SendGridService>();
            services.AddTransient<ITokenHelper, TokenHelper>();
            services.AddTransient<IUrlUtility, UrlUtility>();
            services.AddTransient<IScheduleFactory, RoundRobinScheduler>();
            services.AddTransient<ICdnService, FileStorageCdn>();
            services.AddTransient<IPointService, EloPointService>();
            services.AddTransient<ISeasonPointCalculator, ParticipationSeasonPointCalculator>();
        }

        private void ConfigureDB(IServiceCollection services)
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
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

                //app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                //{
                //    HotModuleReplacement = false,
                //    ReactHotModuleReplacement = false,
                //    EnvironmentVariables = new Dictionary<string, string> {{"mode", "development"}},
                //});
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Site/Error", "?statusCode={0}");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSwaggerUi(typeof(Startup).GetTypeInfo().Assembly,
                settings => settings.GeneratorSettings.DefaultPropertyNameHandling = PropertyNameHandling.CamelCase);

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();
        }
    }
}