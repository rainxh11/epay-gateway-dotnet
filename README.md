<img src="https://raw.githubusercontent.com/rainxh11/Chargily.EpayGateway.NET/master/assets/chargily.svg" width="300">

[![Latest version](https://img.shields.io/nuget/v/Chargily.EpayGateway.NET.svg)](https://www.nuget.org/packages/Chargily.EpayGateway.NET/)

# Chargily ePay Gateway C\#.NET Plugin, supports ASP.NET Core, Xamarin, .NET MAUI

![Chargily ePay Gateway](https://raw.githubusercontent.com/Chargily/epay-gateway-php/main/assets/banner-1544x500.png "Chargily ePay Gateway")

Integrate ePayment gateway with Chargily easily.
- Currently support payment by **CIB / EDAHABIA** cards and soon by **Visa / Mastercard**
- This is a **C#.NET Nuget Package**, If you are using another programing language [Browse here](https://github.com/Chargily/) or look to [API documentation](https://github.com/Chargily/epay-gateway-php/blob/master/README.md)

# Installation
**First**, install the `Chargily.EpayGateway.NET` [NuGet package](https://www.nuget.org/packages/Chargily.EpayGateway.NET) into your app
```powershell
PM> Install-Package Chargily.EpayGateway.NET
```

# Requirements
1. Get your API Key/Secret from [ePay by Chargily](https://epay.chargily.com.dz) dashboard for free

# How to use
this package provide `ChargilyEpayClient` client, to create payment request use: 
```csharp
using Chargily.EpayGateway.NET;

var client = ChagilyEpay.CreateClient("[API_KEY]");

var payment = new EpayPaymentRequest()
{
    InvoiceNumber = "[SOME_INVOICE_NUMER]"
    Name = "Ahmed",
    Email = "rainxh11@gmail.com",
    Amount = 1500,
    DiscountPercentage = 5.0,
    PaymentMethod = PaymentMethod.EDAHABIA,
    RedirectBackTo = "https://example.com/",
    CameFrom = "https://shop.com/purchase",
    ExtraInfo = "Product Purchase"
};

var response = await client.CreatePayment(EpayPaymentRequest);
```

# Usage with ASP.NET Core Minimal API
```csharp
using Chargily.EpayGateway.NET;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddChargilyEpayGateway("[API_KEY]");

var app = builder.Build();

app.MapPost("/invoice",
    async ([FromBody] EpayPaymentRequest request,
        [FromServices] IChargilyEpayClient<EpayPaymentResponse, EpayPaymentRequest> chargilyClient) =>
    {
        return await chargilyClient.CreatePayment(request.Adapt<EpayPaymentRequest>());
    });

app.Run();
```
### Request:
```json
{
    "invoice_number" : "321616",
    "client" : "Ahmed",
    "client_email" : "rainxh11@gmail.com",
    "amount" : 1500,
    "discount" : 5.0,
    "mode" : "EDAHABIA",
    "back_url" : "https://example.com/",
    "webhook_url" : "https://shop.com/purchase",
    "comment" : "Product Purchase"
}
```
### Response:
```json
{
    "httpStatusCode": 201,
    "responseMessage": {
        "Message": "Success"
    },
    "isSuccessful": true,
    "isRequestValid": true,
    "body": {
        "checkout_url": "https://epay.chargily.com.dz/checkout/d00c1e652200798bbc35f688b2910fa9bc6c4c30d38b51e3f4142e407fa7c141"
    },
    "createdOn": "2022-05-06T03:55:49.6527862+01:00"
}
```


# Usage with .NET MAUI
```csharp
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Extensions.DependencyInjection;
namespace MyApp
{
  public static class MauiProgram
  {
    public static MauiApp CreateMauiApp()
    {
      var builder = MauiApp.CreateBuilder();
      builder.UseMauiApp<App>();
      builder.Services.AddChargilyEpayGateway("[API_KEY]");
      return builder.Build();
    }
  }
}
```
then you can add in `ViewModels`:
```csharp
public class MainViewModel : ViewModelBase  
{  
    private ChargilyEpayClient _chargilyClient;  

    public MainViewModel(ChargilyEpayClient chargilyClient)  
    {  
        _chargilyClient = chargilyClient;  
    }  
    ...  
}
```

### This package is using [`Microsoft.Extensions.DependencyInjection`](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection) dependancy injection, so it can be used with application or framework using it.

[api-keys]: https://epay.chargily.com.dz/secure/admin/apikeys