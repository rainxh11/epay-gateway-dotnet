using Chargily.EpayGateway.NET;
using Mapster;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

builder.Services.AddChargilyEpayGateway("[API_KEY]");

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

app.Run();