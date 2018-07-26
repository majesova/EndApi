using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EndApi.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Swashbuckle.AspNetCore.Swagger;
using EndApi.Models;
using Newtonsoft.Json.Serialization;
using EndApi.Filters;

namespace EndApi
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
            // Get JWT Token Settings from JwtSettings.json file
            JwtSettings settings;
            settings = GetJwtSettings();
            // Create singleton of JwtSettings
            services.AddSingleton<JwtSettings>(settings);

            services.AddDbContext<EndContext>(opt=>opt.UseSqlite("Data Source=end.db"));
            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<EndContext>()
                .AddDefaultTokenProviders();
            services.AddScoped<UserRepository>();


            // ===== Add Jwt Authentication ========
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(opt=>{
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme =JwtBearerDefaults.AuthenticationScheme;
                
            }).AddJwtBearer(cfg=>{
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidIssuer = settings.Issuer,
                        ValidAudience = settings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key)),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(settings.DaysToExpiration)
                    };
            });



            services.AddMvc(opt=>{
                opt.Filters.Add(new ValidateModelAttribute());
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "En API Scheme", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = "header",
                        Type = "apiKey"
                    });
                    c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>{
                        {"Bearer", Enumerable.Empty<string>()}
                    });
            });
             services.AddAuthorization(cfg=>{
                    //The claim type and value are case sensitive
                    //cfg.AddPolicy("canAccessProducts", p=>p.RequireClaim("CanAccessProducts", "true"));
            });

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, EndContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
             // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                 c.DocExpansion(DocExpansion.None);
            });

            //app.UseHttpsRedirection();
            app.UseCors(options => options.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseMvc();
              // ===== Create tables ======
            dbContext.Database.EnsureCreated();
        }

        public JwtSettings GetJwtSettings(){
                JwtSettings settings = new JwtSettings();
                settings.Audience = Configuration["JwtSettings:audience"];
                settings.Issuer = Configuration["JwtSettings:issuer"];
                settings.Key = Configuration["JwtSettings:key"];
                settings.DaysToExpiration =Convert.ToInt32(Configuration["JwtSettings:expireDays"]);
                return settings;
        }
    }
}
