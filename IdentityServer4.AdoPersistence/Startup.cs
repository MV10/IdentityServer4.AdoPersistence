using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ClientWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(options => options.EnableEndpointRouting = false);


            services.AddSingleton(sp =>
            {
                var config = TelemetryConfiguration.CreateDefault();

                config.DisableTelemetry = true;

                return config;
            });

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "cookie";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("cookie")
                .AddOpenIdConnect( "oidc", options =>
                {
                    options.SignInScheme = "cookie";
                    options.Authority = "http://localhost:5000";
                    //options.UsePkce = true;
                    options.RequireHttpsMetadata = false;
                    options.ClientId = "mv10blog.client";
                    options.ClientSecret = "the_secret";
                    options.ResponseType = OidcConstants.ResponseTypes.Code;
                    //options.ResponseType = OidcConstants.ResponseTypes.Code;
                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.UsePkce = true;
                    //options.ResponseMode = "form_post";
                    //options.CallbackPath = "/signin-oidc";

                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            //app.UseRouting();
            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseMvcWithDefaultRoute();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapDefaultControllerRoute()
            //        .RequireAuthorization();
            //});
        }
    }
}
