using DataAcess;
using IdentityManager.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

public class LanguageDbContextAccessor : ILanguageDbContextAccessor
{

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IServiceProvider _serviceProvider;

    public LanguageDbContextAccessor(IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
    {
        _httpContextAccessor = httpContextAccessor;
        _serviceProvider = serviceProvider;
    }

    public ApplicationDbContext GetDbContext()
    {
        //var lang = _httpContextAccessor.HttpContext?.Items["Lang"]?.ToString() ?? "en";
        //if (lang == "ar")
        //    return _serviceProvider.GetRequiredService<Arabic_ApplicationDbContext>();
        //else
            return _serviceProvider.GetRequiredService<ApplicationDbContext>();
    }
}