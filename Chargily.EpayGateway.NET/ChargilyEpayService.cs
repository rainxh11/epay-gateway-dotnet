using System;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using FluentValidation;
using Chargily.EpayGateway.NET.Validations;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace Chargily.EpayGateway.NET
{
    public static class ChargilyEpayService
    {
        public static IServiceCollection AddChargilyEpayGateway(this IServiceCollection services, string apiKey)
        {
            return services
                .AddLogging()
                .AddHttpClient()
                .AddRefitClient<IChargilyEpayAPI>()
                .ConfigureHttpClient(client => client.BaseAddress = new Uri("https://epay.chargily.com.dz"))
                .Services
                .AddSingleton<IValidator<EpayPaymentRequest>, PaymentRequestValidator>()
                .AddSingleton<IChargilyEpayClient<EpayPaymentResponse, EpayPaymentRequest>, ChargilyEpayClient>(
                    provider =>
                    {
                        var logger = provider.GetService<ILogger<ChargilyEpayClient>>();
                        var validator = provider.GetService<IValidator<EpayPaymentRequest>>();
                        var apiClient = provider.GetService<IChargilyEpayAPI>();

                        return new ChargilyEpayClient(apiKey, logger, validator, apiClient);
                    });
        }

        public static IServiceCollection AddChargilyEpayGateway(this IServiceCollection services, string apiKey,
            Action<HttpClient> configureHttpClient)
        {
            return services
                .AddLogging()
                .AddHttpClient()
                .AddRefitClient<IChargilyEpayAPI>()
                .ConfigureHttpClient(configureHttpClient)
                .Services
                .AddSingleton<IValidator<EpayPaymentRequest>, PaymentRequestValidator>()
                .AddSingleton<IChargilyEpayClient<EpayPaymentResponse, EpayPaymentRequest>, ChargilyEpayClient>(
                    provider =>
                    {
                        var logger = provider.GetService<ILogger<ChargilyEpayClient>>();
                        var validator = provider.GetService<IValidator<EpayPaymentRequest>>();
                        var apiClient = provider.GetService<IChargilyEpayAPI>();

                        return new ChargilyEpayClient(apiKey, logger, validator, apiClient);
                    });
        }

        public static IServiceCollection AddChargilyEpayGateway<TResponse, TRequest>(this IServiceCollection services,
            IChargilyEpayClient<TResponse, TRequest> client)
        {
            return services
                .AddLogging()
                .AddHttpClient()
                .AddRefitClient<IChargilyEpayAPI>()
                .ConfigureHttpClient(client => client.BaseAddress = new Uri("https://epay.chargily.com.dz"))
                .Services
                .AddSingleton<IValidator<EpayPaymentRequest>, PaymentRequestValidator>()
                .AddSingleton(client);
        }

        public static IServiceCollection AddChargilyEpayGateway<TResponse, TRequest>(this IServiceCollection services,
            IChargilyEpayClient<TResponse, TRequest> client, Action<HttpClient> configureHttpClient)
        {
            return services
                .AddLogging()
                .AddHttpClient()
                .AddRefitClient<IChargilyEpayAPI>()
                .ConfigureHttpClient(configureHttpClient)
                .Services
                .AddSingleton<IValidator<EpayPaymentRequest>, PaymentRequestValidator>()
                .AddSingleton(client);
        }
    }
}