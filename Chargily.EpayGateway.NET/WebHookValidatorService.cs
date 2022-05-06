using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chargily.EpayGateway.NET
{
    public static partial class ChargilyEpayService
    {
        /// <summary>
        /// Add Chargily WebHook Validator Service
        /// </summary>
        /// <param name="services"></param>
        /// <param name="appSecret">Chargily APP SECRET, get it from Chargily Dashboard https://epay.chargily.com.dz/secure/admin/epay-api</param>
        /// <returns></returns>
        public static IServiceCollection AddChargilyWebHookValidator(this IServiceCollection services, string appSecret)
        {
            return services
                .AddSingleton<IWebHookValidator, WebHookValidator>(provider =>
                {
                    var logger = provider.GetService<ILogger<WebHookValidator>>();

                    return new WebHookValidator(appSecret, logger);
                });
        }

        /// <summary>
        /// Add Chargily WebHook Validator Service, APP SECRET is loaded automatically from 'appsettings.json' 'CHARGILY_APP_SECRET' field only if using ASP.NET Core or .NET MAUI otherwise it throws an exception
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddChargilyWebHookValidator(this IServiceCollection services)
        {
            return services
                .AddSingleton<IWebHookValidator, WebHookValidator>(provider =>
                {
                    var logger = provider.GetService<ILogger<WebHookValidator>>();
                    var configuration = provider.GetService<IConfiguration>();
                    return new WebHookValidator(configuration, logger);
                });
        }

        public static IServiceCollection AddChargilyValidatorMiddleware(this IServiceCollection services)
        {
            return services
                .AddScoped<WebHookValidatorMiddleware>();
        }

        public static IApplicationBuilder UseChargilyValidatorMiddleware(this IApplicationBuilder appBuilder)
        {
            return appBuilder
                .UseMiddleware<WebHookValidatorMiddleware>();
        }
    }
}