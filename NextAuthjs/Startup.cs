using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using NextAuthjs.Common;

namespace NextAuthjs
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
            IdentityModelEventSource.ShowPII = true;
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // You need to set this to the same value as the one in ```[...nextauth].ts``` as shown above
                    var signingKey = this.Configuration["JWTSigningKeyBase64UrlEncoded"];

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        RequireSignedTokens = true,

                        // Note here we use Base64Url.Decode
                        IssuerSigningKey = new SymmetricSecurityKey(Base64Url.Decode(signingKey)),

                        ValidateIssuer = false,

                        ValidateAudience = false,

                        // Token will only be valid if not expired yet, with 5 minutes clock skew.
                        ValidateLifetime = true,
                        RequireExpirationTime = true,
                        ClockSkew = new TimeSpan(0, 5, 0),
                    };

                    options.Events = new JwtBearerEvents
                    {
                        // Get token from NextAuth cookie, token query parameter, or Authorization Bearer header
                        // See below
                        OnMessageReceived = AuthUtils.ExtractToken,
                    };
                });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NextAuthjs", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NextAuthjs v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
