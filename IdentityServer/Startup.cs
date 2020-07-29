using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using IdentityModel;
using IdentityServer.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer4;
using IdentityServer4.Stores;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc((options => options.EnableEndpointRouting = false));

            services.AddIdentityServer()
                .AddSigningCredential(GetIdServerCertificate())
                //.AddDeveloperSigningCredential()
                .AddInMemoryClients(ConfigureIdentityServer.GetClients())
                .AddInMemoryIdentityResources(ConfigureIdentityServer.GetIdentityResources())
                .AddProfileService<UserProfileService>();

            services.AddSingleton(sp =>
            {
                var config = TelemetryConfiguration.CreateDefault();

                config.DisableTelemetry = true;

                return config;
            });

            services.AddSingleton<IUserStore, UserStore>();
            services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();

            services.AddAuthentication()
                .AddGoogle("Google", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.ClientId = "434483408261-55tc8n0cs4ff1fe21ea8df2o443v2iuc.apps.googleusercontent.com";
                    options.ClientSecret = "3gcoTrEDPPJ0ukn_aYYT6PWo";
                });
        }

        public static X509Certificate2 GetIdServerCertificate()
        {
            using var rsaCertStore = new X509Store(StoreName.My, StoreLocation.LocalMachine, OpenFlags.ReadOnly);
            rsaCertStore.Open(OpenFlags.ReadOnly);
            return rsaCertStore.Certificates.Find(X509FindType.FindBySubjectName, "identitycertpkcs", true)[0];
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer(); // includes a call to UseAuthentication
            app.UseRouting();
            //app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
