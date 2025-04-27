using DataAcess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using DataAcess.Repos;
using DataAcess.Repos.IRepos;
using Microsoft.Extensions.FileProviders;
using IdentityManagerAPI.Middlewares;
using IdentityManager.Services.ControllerService.IControllerService;
using IdentityManager.Services.ControllerService;
using IdentityManager.Services;
using IdentityManager.Services.JwtAuthentication;
using Microsoft.Extensions.Configuration;
using IdentityManagerAPI;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
//var jwtSettings = new JwtSettings();

//builder.Configuration.Bind(nameof(jwtSettings), jwtSettings);

//builder.Services.AddSingleton(jwtSettings);

//builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddApplication().AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

//builder.Services
//           .AddIdentityCore<ApplicationUser>(o =>
//           {
//               o.SignIn.RequireConfirmedPhoneNumber = false;
//               o.SignIn.RequireConfirmedEmail = false;
//               o.Password.RequireDigit = true;
//               o.Password.RequireLowercase = true;
//               o.Password.RequireUppercase = true;
//               o.Password.RequireNonAlphanumeric = false;
//               o.Password.RequiredLength = 8;
//           })
//           .AddRoles<IdentityRole>()
//           .AddEntityFrameworkStores<ApplicationDbContext>()
//           .AddDefaultTokenProviders();
// Add database context
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddDbContext<ConvertExcelDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("ExternalConnection")));

// Configure Identity
//builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders();


// Add AutoMapper
//builder.Services.AddAutoMapper(typeof(MappingConfig));
//builder.Services.AddAutoMapper(typeof(RecipeProfile));

//builder.Services.AddScoped<UserManager<ApplicationUser>>();
//builder.Services.AddScoped<SignInManager<ApplicationUser>>();
//builder.Services.AddScoped<RoleManager<IdentityRole>>();

//// Add Services
//builder.Services.AddScoped<IAuthService, AuthService>();
//builder.Services.AddScoped<IUserService, UserService>();


//// Add Repositories
//builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<IImageRepository, ImageRepository>();

// Add OpenAPI with Bearer Authentication Support
//builder.Services.AddOpenApi("v1", options =>
//{
//    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
//});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
            "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
            "Example: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
     {
         {
             new OpenApiSecurityScheme
             {
                 Reference = new OpenApiReference
                             {
                                 Type = ReferenceType.SecurityScheme,
                                 Id = "Bearer"
                             },
                 Scheme = "Bearer",
                 Name = "Bearer",
                 In = ParameterLocation.Header
             },
             new List<string>()
         }
     });

});

// Configure JWT Authentication insted of cookies
//var key = Encoding.ASCII.GetBytes(builder.Configuration["ApiSettings:Secret"]);
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(key),
//        ValidateIssuer = false,
//        ValidateAudience = false,
//        ClockSkew = TimeSpan.FromDays(7)
//    };
//});


// Register the global exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapScalarApiReference();
}

// Use the global exception handler
app.UseExceptionHandler();


app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
});

app.MapControllers();

app.Run();
