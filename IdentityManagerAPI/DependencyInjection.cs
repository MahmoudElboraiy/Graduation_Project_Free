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
using IdentityManager.Services.Interfaces;
using Domain.Interfaces;
using Domain.DTOs.Food;

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

            services.AddHttpContextAccessor();
            services.AddScoped<ILanguageDbContextAccessor, LanguageDbContextAccessor>();
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

            var connectionStringArabic = configuration.GetConnectionString("ExternalConnection");
            services.AddDbContext<Arabic_ApplicationDbContext>(options => options.UseSqlServer(connectionStringArabic));

            services.AddScoped<ILanguageDbContext>(provider =>
            {
                var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
                var lang = httpContextAccessor.HttpContext?.Items["Lang"]?.ToString() ?? "en";

                return lang == "ar"
                    ? provider.GetRequiredService<Arabic_ApplicationDbContext>()
                    : provider.GetRequiredService<ApplicationDbContext>();
            });

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new LocalizedConverter<Recipe>());
                    options.JsonSerializerOptions.Converters.Add(new LocalizedConverter<Nutrition>());
                    options.JsonSerializerOptions.Converters.Add(new LocalizedConverter<Ingredient>());
                    options.JsonSerializerOptions.Converters.Add(new LocalizedConverter<Recipe_Ingredient>());
                    options.JsonSerializerOptions.Converters.Add(new LocalizedConverter<RecipeWithNutritionDTO>());

                });

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
