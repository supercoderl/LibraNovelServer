using LibraNovel.Application.Interfaces;
using LibraNovel.Application.ViewModels.Paypal;
using LibraNovel.Application.Wrappers;
using LibraNovel.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LibraNovel.Application.Services
{
    public class PaypalService : IPaypalService
    {
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private const string TokenCacheKey = "PaypalToken";
        private string? BaseURL;
        private string? ClientID;
        private string? Secret;

        public PaypalService(IConfiguration configuration, IMemoryCache cache, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
            BaseURL = _configuration["PaypalConfiguration:Mode"] == "Live"
            ? "https://api-m.paypal.com"
            : "https://api-m.sandbox.paypal.com";
            ClientID = _configuration["PaypalConfiguration:ClientID"];
            Secret = _configuration["PaypalConfiguration:ClientSecret"];
        }

        private async Task<AuthorizationResponseData?> Authenticate()
        {
            var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ClientID}:{Secret}"));

            var content = new List<KeyValuePair<string, string>>
            {
                new("grant_type", "client_credentials")
            };

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{BaseURL}/v1/oauth2/token"),
                Method = HttpMethod.Post,
                Headers =
                {
                    { "Authorization", $"Basic {auth}" }
                },
                Content = new FormUrlEncodedContent(content)
            };

            var httpClient = new HttpClient();
            var httpResponse = await httpClient.SendAsync(request);
            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<AuthorizationResponseData>(jsonResponse);

            return response;
        }

        public async Task<Response<CreateOrderResponse?>> CreateOrder(string value, string currency, string reference)
        {
            var auth = await Authenticate();

            var request = new CreateOrderRequest
            {
                intent = "CAPTURE",
                purchase_units = new List<PurchaseUnit>
                {
                    new()
                    {
                        reference_id = reference,
                        amount = new ViewModels.Paypal.Amount
                        {
                            currency_code = currency,
                            value = value
                        }
                    }
                }
            };

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {auth?.access_token}");

            var httpResponse = await httpClient.PostAsJsonAsync($"{BaseURL}/v2/checkout/orders", request);

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<CreateOrderResponse>(jsonResponse);

            return new Response<CreateOrderResponse?>
            {
                Succeeded = true,
                Data = response
            };
        }

        public async Task<Response<CaptureOrderResponse?>> CaptureOrder(string orderId)
        {
            var auth = await Authenticate();

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {auth?.access_token}");

            var httpContent = new StringContent("", Encoding.Default, "application/json");

            var httpResponse = await httpClient.PostAsync($"{BaseURL}/v2/checkout/orders/{orderId}/capture", httpContent);

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<CaptureOrderResponse>(jsonResponse);

            return new Response<CaptureOrderResponse?>
            {
                Succeeded = true,
                Data = response
            };
        }

        public async Task<Response<string>> GetTransactions()
        {
            var auth = await Authenticate();

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {auth?.access_token}");

            var httpContent = new StringContent("", Encoding.Default, "application/json");

            var httpResponse = await httpClient.GetAsync($"{BaseURL}/v1/reporting/transactions?fields=transaction_info,payer_info,shipping_info,auction_info,cart_info,incentive_info,store_info&start_date=2024-06-20T23:59:59.999Z&end_date=2024-07-20T00:00:00.000Z");

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<CaptureOrderResponse>(jsonResponse);

            return new Response<string>
            {
                Succeeded = true,
                Data = ""
            };
        }

        /* public async Task<Response<AuthorizationResponseData?>> GetAuthorizationRequest()
         {
             EnsureHttpClientCreated();
             if (_cache.TryGetValue(TokenCacheKey, out string? accessToken))
             {
                 return new Response<AuthorizationResponseData?>
                 {
                     Succeeded = true,
                     Data = new AuthorizationResponseData { access_token = accessToken ?? string.Empty }
                 };
             }

             var byteArray = Encoding.ASCII.GetBytes($"{_configuration["PaypalConfiguration:ClientID"]}:{_configuration["PaypalConfiguration:ClientSecret"]}");
             _client.DefaultRequestHeaders.Authorization =
                 new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

             var keyValueParis = new List<KeyValuePair<string, string>>
                 { new KeyValuePair<string, string>("grant_type", "client_credentials") };

             var response = await _client.PostAsync($"{_configuration["PaypalConfiguration:BaseURL"]}/v1/oauth2/token", new FormUrlEncodedContent(keyValueParis));

             var responseAsString = await response.Content.ReadAsStringAsync();

             var authorizationResponse = JsonConvert.DeserializeObject<AuthorizationResponseData>(responseAsString);

             if (authorizationResponse != null)
             {
                 var cacheOptions = new MemoryCacheEntryOptions()
                     .SetAbsoluteExpiration(TimeSpan.FromMinutes(authorizationResponse.expires_in)); // Adjust based on token lifetime
                 _cache.Set(TokenCacheKey, authorizationResponse.access_token, cacheOptions);
             }

             return new Response<AuthorizationResponseData?>
             {
                 Succeeded = true,
                 Data = authorizationResponse
             };
         }

         public async Task<Response<CreatePlanResponse?>> CreatePlan(CreatePlanRequest request)
         {
             EnsureHttpClientCreated();
             SetToken();
             var requestContent = JsonConvert.SerializeObject(request);

             var httpRequestMessage = new HttpRequestMessage
             {
                 Content = new StringContent(requestContent, Encoding.UTF8, "application/json")
             };

             httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
             httpRequestMessage.Headers.Add("Prefer", "return=representation");

             var response = await _client.PostAsync($"{_configuration["PaypalConfiguration:BaseURL"]}/v1/billing/plans", httpRequestMessage.Content);

             var responseAsString = await response.Content.ReadAsStringAsync();

             var result = JsonConvert.DeserializeObject<CreatePlanResponse>(responseAsString);

             return new Response<CreatePlanResponse?>
             {
                 Succeeded = true,
                 Data = result
             };
         }

         public async Task<Response<CreatePlanResponse?>> GetPlanDetails(string planId)
         {
             SetToken();
             var response = await _client.GetAsync($"{_configuration["PaypalConfiguration:BaseURL"]}/v1/billing/plans/{planId}");

             var responseAsString = await response.Content.ReadAsStringAsync();

             var result = JsonConvert.DeserializeObject<CreatePlanResponse>(responseAsString);

             return new Response<CreatePlanResponse?>
             {
                 Succeeded = true,
                 Data = result
             };
         }

         public async Task<Response<CreateSubscriptionResponse?>> CreateSubscription(CreateSubscriptionRequest request)
         {

             var requestContent = JsonConvert.SerializeObject(request);

             var httpRequestMessage = new HttpRequestMessage
             {
                 Content = new StringContent(requestContent, Encoding.UTF8, "application/json")
             };

             httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

             var response = await _client.PostAsync($"{_configuration["PaypalConfiguration:BaseURL"]}/v1/billing/subscriptions", httpRequestMessage.Content);

             var responseAsString = await response.Content.ReadAsStringAsync();

             var result = JsonConvert.DeserializeObject<CreateSubscriptionResponse>(responseAsString);

             return new Response<CreateSubscriptionResponse?>
             {
                 Succeeded = true,
                 Data = result
             };
         }

         public async Task<Response<bool>> ActiveSubscription(string id, SubscriptionStatusChangeRequest request)
         {

             var requestContent = JsonConvert.SerializeObject(request);

             var httpRequestMessage = new HttpRequestMessage
             {
                 Content = new StringContent(requestContent, Encoding.UTF8, "application/json")
             };

             httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

             var response = await _client.PostAsync($"{_configuration["PaypalConfiguration:BaseURL"]}/v1/billing/subscriptions/{id}/activate", httpRequestMessage.Content);

             return new Response<bool>
             {
                 Succeeded = true,
                 Data = response.StatusCode == HttpStatusCode.NoContent
             };
         }

         public async Task<Response<bool>> SuspendSubscription(string id, SubscriptionStatusChangeRequest request)
         {

             var requestContent = JsonConvert.SerializeObject(request);

             var httpRequestMessage = new HttpRequestMessage
             {
                 Content = new StringContent(requestContent, Encoding.UTF8, "application/json")
             };

             httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

             var response = await _client.PostAsync($"{_configuration["PaypalConfiguration:BaseURL"]}/v1/billing/subscriptions/{id}/suspend", httpRequestMessage.Content);

             return new Response<bool>
             {
                 Succeeded = true,
                 Data = response.StatusCode == HttpStatusCode.NoContent
             };
         }

         public async Task<Response<bool>> CancelSubscription(string id, SubscriptionStatusChangeRequest request)
         {

             var requestContent = JsonConvert.SerializeObject(request);

             var httpRequestMessage = new HttpRequestMessage
             {
                 Content = new StringContent(requestContent, Encoding.UTF8, "application/json")
             };

             httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

             var response = await _client.PostAsync($"{_configuration["PaypalConfiguration:BaseURL"]}/v1/billing/subscriptions/{id}/cancel", httpRequestMessage.Content);

             return new Response<bool>
             {
                 Succeeded = true,
                 Data = response.StatusCode == HttpStatusCode.NoContent
             };
         }

         public async Task<Response<bool>> RefundSubscriptionAmount(string refundUrl, RefundRequest request)
         {

             var requestContent = JsonConvert.SerializeObject(request);

             var httpRequestMessage = new HttpRequestMessage
             {
                 Content = new StringContent(requestContent, Encoding.UTF8, "application/json")
             };

             httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

             var response = await _client.PostAsync(refundUrl, httpRequestMessage.Content);

             return new Response<bool>
             {
                 Succeeded = true,
                 Data = response.StatusCode == HttpStatusCode.NoContent
             };
         }

         public async Task<Response<bool>> VerifyEvent(string json, IHeaderDictionary headerDictionary)
         {
             // !!IMPORTANT!!
             // Without this direct JSON serialization, PayPal WILL ALWAYS return verification_status = "FAILURE".
             // This is probably because the order of the fields are different and PayPal does not sort them. 
             var paypalVerifyRequestJsonString = $@"{{
                 ""transmission_id"": ""{headerDictionary["PAYPAL-TRANSMISSION-ID"][0]}"",
                 ""transmission_time"": ""{headerDictionary["PAYPAL-TRANSMISSION-TIME"][0]}"",
                 ""cert_url"": ""{headerDictionary["PAYPAL-CERT-URL"][0]}"",
                 ""auth_algo"": ""{headerDictionary["PAYPAL-AUTH-ALGO"][0]}"",
                 ""transmission_sig"": ""{headerDictionary["PAYPAL-TRANSMISSION-SIG"][0]}"",
                 ""webhook_id"": ""6WC685942N447610S"",
                 ""webhook_event"": {json}
                 }}";

             var content = new StringContent(paypalVerifyRequestJsonString, Encoding.UTF8, "application/json");

             var resultResponse = await _client.PostAsync($"{_configuration["PaypalConfiguration:BaseURL"]}/v1/notifications/verify-webhook-signature", content);

             var responseBody = await resultResponse.Content.ReadAsStringAsync();

             var verifyWebhookResponse = JsonConvert.DeserializeObject<WebhookVerificationResponse>(responseBody);

             if (verifyWebhookResponse?.verification_status != "SUCCESS")
             {
                 return new Response<bool>(false, null);
             }

             return new Response<bool>(true, null);
         }

         private void EnsureHttpClientCreated()
         {
             if (_client == null)
             {
                 CreateHttpClient();
             }
         }*/

        private string? GetToken()
        {
            if (_cache.TryGetValue(TokenCacheKey, out string? accessToken))
            {
                return accessToken;
            }
            return string.Empty;
        }
    }
}
