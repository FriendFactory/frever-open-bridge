using System;
using BestHTTP;
using Newtonsoft.Json;

namespace Bridge.Authorization.Models
{
    public sealed class JsonRequestContentComposer : IRequestContentComposer
    {
        private readonly ICredentials _credentials;
        
        public JsonRequestContentComposer(ICredentials credentials)
        {
            _credentials = credentials ?? throw new ArgumentException("Credentials cannot be null");
        }
        
        public void ComposeRequestContent(HTTPRequest httpRequest)
        {
            var requestJson = JsonConvert.SerializeObject(_credentials.GetLoginCredentials());
            httpRequest.AddJsonContent(requestJson);
        }
    }
}