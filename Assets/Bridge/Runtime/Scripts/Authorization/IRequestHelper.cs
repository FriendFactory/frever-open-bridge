using System;
using System.Net.Http;
using BestHTTP;

namespace Bridge.Authorization
{
    internal interface IRequestHelper
    {
        AuthToken Token { get; }
        void SetToken(AuthToken token);
        
        HTTPRequest CreateRequest(Uri uri, HTTPMethods method, bool addToken, bool protobuf);
        HTTPRequest CreateRequest(string url, HTTPMethods method, bool addToken, bool protobuf);
        HTTPRequest CreateRequest(string url, HTTPMethods method, string token);
        HttpClient CreateClient(bool addToken, bool addRetryPolicy = true);
    }
}