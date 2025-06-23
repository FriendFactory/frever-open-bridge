using System;
using System.Linq;
using BestHTTP;
using BestHTTP.Forms;

namespace Bridge.Authorization.Models
{
    public sealed class MultipartRequestContentComposer : IRequestContentComposer
    {
        private readonly ICredentials _credentials;
        
        public MultipartRequestContentComposer(ICredentials credentials)
        {
            _credentials = credentials ?? throw new ArgumentException("Credentials cannot be null");
        }
        
        public void ComposeRequestContent(HTTPRequest httpRequest)
        {
            var formData = _credentials.GetLoginCredentials().ToList();
            var multiform = new HTTPMultiPartForm();
            foreach (var pair in formData) multiform.AddField(pair.Key, pair.Value);
            
            httpRequest.SetForm(multiform);  
        }
    }
}