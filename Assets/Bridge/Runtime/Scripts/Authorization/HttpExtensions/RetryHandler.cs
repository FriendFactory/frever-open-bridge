using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Bridge.Authorization.HttpExtensions
{
    internal sealed class RetryHandler : DelegatingHandler
    {
        private const int MaxRetries = 3;

        public RetryHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        { }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            for (var i = 0; i < MaxRetries; i++)
            {
                try
                {
                    if(i > 0)
                    {
                        Debug.Log($"Retry request attempt. Request attempt {i+1}. ");
                    }
                    response = await base.SendAsync(request, cancellationToken);
                }
                catch (Exception e)
                {
                    if(e is OperationCanceledException)
                        throw;
                    Debug.Log($"Exception during request: {e.Message}");
                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent($"{e.GetType().Name} : {e.Message}")
                    };
                    continue;
                }
                
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
                else
                {
                    Debug.Log($"Error response during request: {response.ReasonPhrase} {await response.Content.ReadAsStringAsync()}");
                }
            }

            return response;
        }
    }
}