using LibraNovel.Application.ViewModels.Paypal;
using LibraNovel.Application.Wrappers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Interfaces
{
    public interface IPaypalService
    {
        /*        Task<Response<AuthorizationResponseData?>> GetAuthorizationRequest();
                Task<Response<CreatePlanResponse?>> CreatePlan(CreatePlanRequest request);
                Task<Response<CreatePlanResponse?>> GetPlanDetails(string planId);
                Task<Response<CreateSubscriptionResponse?>> CreateSubscription(CreateSubscriptionRequest request);
                Task<Response<bool>> ActiveSubscription(string id, SubscriptionStatusChangeRequest request);
                Task<Response<bool>> SuspendSubscription(string id, SubscriptionStatusChangeRequest request);
                Task<Response<bool>> CancelSubscription(string id, SubscriptionStatusChangeRequest request);
                Task<Response<bool>> RefundSubscriptionAmount(string refundUrl, RefundRequest request);
                Task<Response<bool>> VerifyEvent(string json, IHeaderDictionary headerDictionary);*/
        Task<Response<CreateOrderResponse?>> CreateOrder(string value, string currency, string reference);
        Task<Response<CaptureOrderResponse?>> CaptureOrder(string orderId);
        Task<Response<string>> GetTransactions();

    }
}
