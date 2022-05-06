﻿using System;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Chargily.EpayGateway.NET
{
    public class EpayPaymentRequest
    {
        [JsonPropertyName("client")] public string Name { get; set; }

        [JsonPropertyName("client_email")] public string Email { get; set; }

        [JsonPropertyName("invoice_number")] public string InvoiceNumber { get; set; }

        [JsonPropertyName("amount")] public double Amount { get; set; }

        [JsonPropertyName("discount")] public double DiscountPercentage { get; set; }

        [JsonPropertyName("back_url")] public string RedirectBackTo { get; set; }
        [JsonPropertyName("webhook_url")] public string CameFrom { get; set; }

        [JsonPropertyName("mode")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethod PaymentMethod { get; set; }

        [JsonPropertyName("comment")] public string ExtraInfo { get; set; }

        [JsonIgnore]
        public double AmountAfterDiscount
        {
            get => Amount - (Amount * DiscountPercentage / 100);
        }
    }

    public class PaymentRequest
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string InvoiceNumber { get; set; }

        public double Amount { get; set; }

        public double DiscountPercentage { get; set; }

        public string RedirectBackTo { get; set; }
        public string CameFrom { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethod PaymentMethod { get; set; }

        public string ExtraInfo { get; set; }

        [JsonIgnore]
        public double AmountAfterDiscount
        {
            get => Amount - (Amount * DiscountPercentage / 100);
        }
    }
}