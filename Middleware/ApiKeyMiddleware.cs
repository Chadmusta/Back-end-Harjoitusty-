namespace Harjoitustyo.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string APIKEYNAME = "ApiKey";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(APIKEYNAME,out var extracterApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Api key missing");
                return;
            }
            var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
            var apikey = appSettings.GetValue<string>(APIKEYNAME);
            if(!apikey.Equals(extracterApiKey))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("invalid API key");
                return;
            }
            await _next(context);
        }
    }
}
