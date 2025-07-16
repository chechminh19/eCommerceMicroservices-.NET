using UserApi.Application.Interfaces;

namespace UserApi.Presentation.Middlewares
{
    public class ConfirmationTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        public ConfirmationTokenMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Path.StartsWithSegments("/confirm-email"))
            {
                await _next(context);
                return;
            }
            var token = context.Request.Query["token"].FirstOrDefault();
            var baseUrl = _configuration["AppSettings:UrlMidleware"];
            var successUrl = $"{baseUrl}/verify-success";
            var errorUrl = $"{baseUrl}/verify-error";

            if (string.IsNullOrEmpty(token))
            {
                await _next(context);
                return;
            }
            try
            {   
                var userRepo = context.RequestServices.GetRequiredService<IUserRepo>();
                var user = await userRepo.GetUserByConfirmationToken(token);

                if (user == null)
                {
                    context.Response.Redirect(errorUrl);
                    return;
                }

                if (user.IsEmailVerified)
                {
                    // if email is verified
                    context.Response.Redirect($"{baseUrl}/already-verified");
                    return;
                }

                // confirm email
                user.IsEmailVerified = true;
                user.EmailConfirmationToken = null;
                await userRepo.UpdateAsync(user);

                // Redirect to success
                context.Response.Redirect(successUrl);
            }
            catch (Exception)
            {
                context.Response.Redirect(errorUrl);
            }
        }
    }
}
