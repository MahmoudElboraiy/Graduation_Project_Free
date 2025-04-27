using DataAcess;
using DataAcess.Repos.IRepos;
using DataAcess.Repos;
using IdentityManager.Services.ControllerService.IControllerService;
using IdentityManager.Services.ControllerService;
using IdentityManager.Services.JwtAuthentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Domain.DTOs.Mapper;
using Domain.Domain;

namespace IdentityManagerAPI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
                this IServiceCollection services,
                IConfiguration configuration
            )
        {
            var jwtSettings = new JwtSettings();

            configuration.Bind(nameof(jwtSettings), jwtSettings);

            services.AddSingleton(jwtSettings);

            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<IIngredientRepository, IngredientRepository>();
            //services.AddScoped<IIngredientLogRepository, IngredientLogRepository>();
            //services.AddScoped<IIngredientStockRepository, IngredientStockRepository>();
            //services.AddScoped<IItemRepository, ItemRepository>();
            //services.AddScoped<IMealRepository, MealRepository>();
            //services.AddScoped<IPlanRepository, PlanRepository>();
            //services.AddScoped<IPromoCodeRepository, PromoCodeRepository>();
            //services.AddScoped<IRecipeIngredientRepository, RecipeIngredientRepository>();
            //services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            //services.AddScoped<IUserPrefernceRepository, UserPrefernceRepository>();
            services.AddAutoMapper(typeof(MappingConfig));
            services.AddAutoMapper(typeof(RecipeProfile));

            services.AddScoped<UserManager<ApplicationUser>>();
            services.AddScoped<SignInManager<ApplicationUser>>();
            services.AddScoped<RoleManager<IdentityRole>>();

            // Add Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();


            // Add Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();

            services
           .AddIdentityCore<ApplicationUser>(o =>
           {
               o.SignIn.RequireConfirmedPhoneNumber = false;
               o.SignIn.RequireConfirmedEmail = false;
               o.Password.RequireDigit = true;
               o.Password.RequireLowercase = true;
               o.Password.RequireUppercase = true;
               o.Password.RequireNonAlphanumeric = false;
               o.Password.RequiredLength = 8;
           })
           .AddRoles<IdentityRole>()
           .AddEntityFrameworkStores<ApplicationDbContext>()
           .AddDefaultTokenProviders();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));


            services
                .AddAuthentication(cfg =>
                {
                    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                    cfg.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddJwtBearer(options =>
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.Key)
                        ),
                        ClockSkew = TimeSpan.FromDays(7)
                    }
                );

            services.AddAuthorization();

            return services;
        }
    }
}
