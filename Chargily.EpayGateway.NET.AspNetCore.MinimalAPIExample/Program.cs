using Chargily.EpayGateway.NET;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();


builder.Services.AddChargilyEpayGateway("[API_KEY]");
builder.Services
    .AddChargilyWebHookValidator("[APP_SECRET]")
    .AddChargilyValidatorMiddleware();


var app = builder.Build();

app
    .UseSwagger()
    .UseSwaggerUI();

app.MapPost("/invoice",
    async ([FromBody] EpayPaymentRequest request,
        [FromServices] IChargilyEpayClient<EpayPaymentResponse, EpayPaymentRequest> chargilyClient) =>
    {
        return await chargilyClient.CreatePayment(request);
    });

app.MapPost("/webhook-validator", ([FromServices] IWebHookValidator validator, HttpRequest request) =>
{
    var signature = request.Headers["Signature"].First();
    var validation = validator.Validate(signature, request.Body);

    return validation;
});

app.UseChargilyValidatorMiddleware();

app.Run();