using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using Bridge.Modules.Serialization;
using Newtonsoft.Json;

namespace Bridge.Services.UserProfile.PhoneLookup
{
    internal sealed class FriendsLookupService : IFriendsLookupService
    {
        private readonly IRequestHelper _requestHelper;
        private readonly ISerializer _serializer;
        private readonly string _serverUrl;

        public FriendsLookupService(string serverUrl, IRequestHelper requestHelper, ISerializer serializer)
        {
            _serverUrl = serverUrl;
            _requestHelper = requestHelper;
            _serializer = serializer;
        }

        public async Task<FriendsByPhoneLookupResult> LookupForFriends(string[] phoneNumbers)
        {
            var url = $"{_serverUrl.TrimEnd('/')}/group/lookup";

            var request = _requestHelper.CreateRequest(url, HTTPMethods.Post, true, true);
            request.AddJsonContent(JsonConvert.SerializeObject(phoneNumbers));

            var resp = await request.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
                return new FriendsByPhoneLookupResult(resp.DataAsText);

            var matchedUsers = _serializer.DeserializeProtobuf<PhoneLookupInfo[]>(resp.Data);
            return new FriendsByPhoneLookupResult(matchedUsers);
        }
    }
}