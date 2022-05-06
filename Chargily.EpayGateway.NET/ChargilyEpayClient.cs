using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Chargily.EpayGateway.NET
{
    public class ChargilyEpayClient : IChargilyEpayClient<EpayPaymentResponse, EpayPaymentRequest>
    {
        private readonly string _apiKey;
        private readonly IChargilyEpayAPI _apiClient;
#nullable enable
        private readonly ILogger<ChargilyEpayClient>? _logger;
        private readonly IValidator<EpayPaymentRequest>? _validator;
#nullable disable
        public ChargilyEpayClient(string apiKey,
            ILogger<ChargilyEpayClient> logger,
            IValidator<EpayPaymentRequest> validator,
            IChargilyEpayAPI apiClient)
        {
            _apiKey = apiKey;
            _logger = logger;
            _validator = validator;
            _apiClient = apiClient;
        }

        public async Task<EpayPaymentResponse> CreatePayment(EpayPaymentRequest request)
        {
            try
            {
                _logger?.LogInformation($"[ChargilyEpay.NET] New Payment Request:" +
                                        $"{Environment.NewLine}{JsonSerializer.Serialize(request)}");

                var response = new EpayPaymentResponse();
                var validation = await _validator.ValidateAsync(request);
                response.ValidatePaymentRequest(validation);
                if (!validation.IsValid)
                {
                    _logger?.LogError($"[ChargilyEpay.NET] Payment Request Validation Error");
                    return response;
                }

                var apiResponse = await _apiClient.CreateInvoice(request, _apiKey);
                await response.CreatePaymentResponse(apiResponse);
                _logger?.LogInformation($"[ChargilyEpay.NET] Payment Response for Invoice '{request.InvoiceNumber}'" +
                                        $":{Environment.NewLine}{JsonSerializer.Serialize(response)}");

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"[ChargilyEpay.NET] Exception Thrown: {ex.Message}");
                throw new Exception($"Create Payment Request Failed!. {ex.Message}", ex);
            }
        }
    }
}