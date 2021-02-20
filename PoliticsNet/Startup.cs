using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.EntityFrameworkCore;
using Politics.Data;
using PoliticsNet.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using PoliticsNet.Helpers;

using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using AutoMapper;

namespace Politics
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
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (env == "Development")
            {
                // Local MySql
                services.AddDbContext<DataContext>(
                    x => {
                        x.UseLazyLoadingProxies();
                        x.UseMySql(Configuration.GetConnectionString("DefaultConnection"),
                        mySqlOptions => {
                            mySqlOptions.ServerVersion(new Version(8, 0, 21), ServerType.MySql);
                            mySqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                        });
                    }
                );
            } else {
                // Heroku PostgreSQL
                services.AddDbContext<DataContext>(options =>
                {
                    var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                    connUrl = connUrl.Replace("postgres://", string.Empty);
                    var pgUserPass = connUrl.Split("@")[0];
                    var pgHostPortDb = connUrl.Split("@")[1];
                    var pgHostPort = pgHostPortDb.Split("/")[0];
                    var pgDb = pgHostPortDb.Split("/")[1];
                    var pgUser = pgUserPass.Split(":")[0];
                    var pgPass = pgUserPass.Split(":")[1];
                    var pgHost = pgHostPort.Split(":")[0];
                    var pgPort = pgHostPort.Split(":")[1];

                    string connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};SSL Mode=Require;TrustServerCertificate=True";

                    options.UseNpgsql(connStr);
                });
            }

            services.AddCors();
            services.Configure<CloudinarySettings>(Configuration.GetSection("AppSettings:Cloudinary"));
            services.AddAutoMapper(typeof(PostRespository).Assembly);
            services.AddScoped<IAuthRepository, AuthRespository>();
            services.AddScoped<IUserRespository, UserRespository>();
            services.AddScoped<IProviderRespository, ProviderRespository>();
            services.AddScoped<IPostRespository, PostRespository>();
            services.AddScoped<IPhoneRespository, PhoneRespository>();
            services.AddScoped<IElectionRespository, ElectionRespository>();
            services.AddScoped<IRatingsRespository, RatingsRespository>();
            services.AddScoped<IMainRespository, MainRespository>();
            services.AddScoped<IActivityRespository, ActivityRespository>();
            services.AddControllers().AddNewtonsoftJson(
                opt => {
                    //selfreferencing loop ignore??
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                }
            );
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer( options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                                    Encoding.ASCII.GetBytes(
                                        Configuration.GetSection("AppSettings:Token").Value)
                                    ),
                        ValidateIssuer = false, // TODO : SECURITY
                        ValidateAudience = false,
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler(
                    builder => builder.Run(
                        async context => {
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            var error = context.Features.Get<IExceptionHandlerFeature>();
                            if(error != null){
                                context.Response.AddAplicationError(error.Error.Message);
                                await context.Response.WriteAsync(error.Error.Message);
                            }
                        })
                );
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //TODO: Security? withorigin
            app.UseCors( x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToController("Index", "Fallback");
            });
        }
    }
}
