using System.Globalization;

public class LanguageMiddleware
{
    private readonly RequestDelegate _next;

    public LanguageMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string lang = "en";

        // الأولوية للـ Query String
        if (context.Request.Query.ContainsKey("lang"))
        {
            lang = context.Request.Query["lang"];
        }
        // بعدين Header
        else if (context.Request.Headers.ContainsKey("Accept-Language"))
        {
            var acceptLang = context.Request.Headers["Accept-Language"].ToString().Split(',').FirstOrDefault()?.ToLower();
            if (acceptLang != null && acceptLang.StartsWith("ar"))
                lang = "ar";
            else
                lang = "en"; // fallback
        }
        // بعدين Cookie لو عاوز
        else if (context.Request.Cookies.ContainsKey("lang"))
        {
            lang = context.Request.Cookies["lang"];
        }

        lang = (lang ?? "en").ToLower();

        // خزن اللغة علشان نستخدمها في أي مكان
        context.Items["Lang"] = lang;

        // اختيار Culture اختياري
        var culture = new CultureInfo(lang == "ar" ? "ar-EG" : "en-US");
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        await _next(context);
    }
}
