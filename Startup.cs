using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using ReferMeAPI.Repositories;
using ReferMeAPI.Services.JWTAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferMeAPI
{
    public class Startup
    {
        private static async Task<UsersCosmosDbService> InitializeUsersCosmosClientInstanceAsync(IConfigurationSection configurationSection)
        {
            var databaseName = configurationSection["DatabaseName"];
            var containerName = configurationSection["ContainerName"];
            var account = configurationSection["Account"];
            var key = configurationSection["Key"];

            var client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);
            var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");
            var cosmosDbService = new UsersCosmosDbService(client, databaseName, containerName);
            return cosmosDbService;
        }

        private static async Task<PostsCosmosDbService> InitializePostsCosmosClientInstanceAsync(IConfigurationSection configurationSection)
        {
            var databaseName = configurationSection["DatabaseName"];
            var containerName = configurationSection["ContainerName"];
            var account = configurationSection["Account"];
            var key = configurationSection["Key"];

            var client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);
            var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");
            var postscosmosDbService = new PostsCosmosDbService(client, databaseName, containerName);
            return postscosmosDbService;
        }

        private static async Task<CollegesCosmosDbService> InitializeCollegesCosmosClientInstanceAsync(IConfigurationSection configurationSection)
        {
            var databaseName = configurationSection["DatabaseName"];
            var containerName = configurationSection["ContainerName"];
            var account = configurationSection["Account"];
            var key = configurationSection["Key"];

            var client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);
            var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");
            var collegescosmosDbService = new CollegesCosmosDbService(client, databaseName, containerName);
            return collegescosmosDbService;
        }

        private static async Task<CompaniesCosmosDbService> InitializeCompaniesCosmosClientInstanceAsync(IConfigurationSection configurationSection)
        {
            var databaseName = configurationSection["DatabaseName"];
            var containerName = configurationSection["ContainerName"];
            var account = configurationSection["Account"];
            var key = configurationSection["Key"];

            var client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);
            var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");
            var companiescosmosDbService = new CompaniesCosmosDbService(client, databaseName, containerName);
            return companiescosmosDbService;
        }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod()
                .AllowAnyHeader());
            });

            services.AddSingleton<IUsersCosmosDbService>(InitializeUsersCosmosClientInstanceAsync(Configuration.GetSection("UsersCosmosDb")).GetAwaiter().GetResult());
            services.AddSingleton<IPostsCosmosDbService>(InitializePostsCosmosClientInstanceAsync(Configuration.GetSection("PostsCosmosDb")).GetAwaiter().GetResult());
            services.AddSingleton<ICollegesCosmosDbService>(InitializeCollegesCosmosClientInstanceAsync(Configuration.GetSection("CollegesCosmosDb")).GetAwaiter().GetResult());
            services.AddSingleton<ICompaniesCosmosDbService>(InitializeCompaniesCosmosClientInstanceAsync(Configuration.GetSection("CompaniesCosmosDb")).GetAwaiter().GetResult());

            services.AddDbContext<ReferMeDBContext>();
            services.AddControllers();
            services.AddAuthentication(
                authOptions =>
                {
                    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(jwtOptions =>
                {
                    var key = Configuration.GetValue<string>("JwtConfig:Key");
                    var keyBytes = Encoding.ASCII.GetBytes(key);
                    jwtOptions.SaveToken = true;
                    jwtOptions.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                        ValidateLifetime = true,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                    };
                });

            services.AddSingleton(typeof(IJwtTokenManager), typeof(JwtTokenManager));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ReferMeAPI", Version = "v1" });
            });
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft
                .Json.ReferenceLoopHandling.Ignore)
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver
                = new DefaultContractResolver());
            services.AddScoped<IUserRepository, UsersRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ICompaniesRepository, CompaniesRepository>();
            services.AddScoped<ICollegeRepository, CollegeRepository>();
            services.AddScoped<IJwtTokenManager, JwtTokenManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReferMeAPI v1"));

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
