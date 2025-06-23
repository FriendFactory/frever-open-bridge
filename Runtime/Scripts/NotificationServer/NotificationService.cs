using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
using Bridge.Authorization;
using Bridge.Modules.Serialization;
using Bridge.Results;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bridge.NotificationServer
{
    internal sealed class NotificationService : INotificationService
    {
        private readonly IRequestHelper _requestHelper;
        private readonly ISerializer _serializer;
        private readonly string _serverUrl;

        private readonly IDictionary<string, Type> _serializationMapping = new Dictionary<string, Type>
        {
            ["NewCommentOnVideo"] = typeof(NewCommentOnVideoNotification),
            ["NewCommentOnVideoYouHaveCommented"] = typeof(NewCommentOnVideoYouHaveCommentedNotification),
            ["NewFollower"] = typeof(NewFollowerNotification),
            ["NewFriendVideo"] = typeof(NewFriendVideoNotification),
            ["NewLikeOnVideo"] = typeof(NewLikeOnVideoNotification),
            ["NewMentionInCommentOnVideo"] = typeof(NewMentionInCommentOnVideo),
            ["YourVideoRemixed"] = typeof(YourVideoRemixedNotification),
            ["YouTaggedOnVideo"] = typeof(YouTaggedOnVideoNotification),
            ["YourVideoConverted"] = typeof(YourVideoConversionCompletedNotification),
            ["VideoDeleted"] = typeof(VideoDeletedNotification),
            ["NewMentionOnVideo"] = typeof(NewMentionOnVideoNotification),
            ["NewStatusReached"] = typeof(NewStatusReachedNotification),
            ["NewLevelReached"] = typeof(NewLevelReachedNotification),
            ["SeasonQuestAccomplished"] = typeof(SeasonQuestAccomplishedNotification),
            ["InvitationAccepted"] = typeof(InvitationAcceptedNotification),
            ["NonCharacterTagOnVideo"] = typeof(YouTaggedOnVideoNotification),
            ["BattleResultCompleted"] = typeof(StyleBattleResultCompletedNotification),
            ["CrewInvitationReceived"] = typeof(CrewInvitationReceivedNotification),
            ["CrewJoinRequestAccepted"] = typeof(CrewJoinRequestAcceptedNotification),
            ["CrewJoinRequestReceived"] = typeof(CrewJoinRequestReceivedNotification),
            ["FriendJoinedCrew"] = typeof(FriendJoinedCrewNotification),
            ["VideoRatingCompleted"] = typeof(RatedVideoNotification),
            ["VideoStyleTransformed"] = typeof(VideoStyleTransformedNotification)
        };

        private static readonly JsonSerializerSettings JsonSerializerSettings = new()
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        };

        public NotificationService(string hostUrl, IRequestHelper authBridge, ISerializer serializer)
        {
            _requestHelper = authBridge ?? throw new ArgumentNullException(nameof(authBridge));
            _serverUrl = hostUrl;
            _serializer = serializer;
        }

        public async Task<ArrayResult<NotificationBase>> MyLatestNotifications(int? top = null, CancellationToken token = default)
        {
            try
            {
                return await MyLatestNotificationsInternal(top, token);
            }
            catch (OperationCanceledException)
            {
                return ArrayResult<NotificationBase>.Cancelled();
            }
        }

        public async Task<Result> MarkAsRead(long[] notificationIds)
        {
            if (notificationIds == null)
                throw new ArgumentNullException(nameof(notificationIds));

            var url = new Uri(
                new Uri(_serverUrl, UriKind.Absolute),
                new Uri("notifications/mark-as-read", UriKind.Relative)
            );

            var req = _requestHelper.CreateRequest(url, HTTPMethods.Put, true, false);
            var json = _serializer.SerializeToJson(notificationIds);
            req.AddJsonContent(json);

            var resp = await req.GetHTTPResponseAsync();
            if (!resp.IsSuccess)
            {
                var error = resp.DataAsText;
                return new ErrorResult(error);
            }

            return new SuccessResult();
        }

        private async Task<ArrayResult<NotificationBase>> MyLatestNotificationsInternal(int? top = null, CancellationToken cancellationToken = default)
        {
            var url = new Uri(new Uri(_serverUrl, UriKind.Absolute), new Uri("notifications", UriKind.Relative))
                .ToString();
            if (top != null) url = $"{url}?$top={top}";

            var request = _requestHelper.CreateRequest(url, HTTPMethods.Get, true, false);
            var resp = await request.GetHTTPResponseAsync(cancellationToken);
            if (!resp.IsSuccess)
            {
                var error = resp.DataAsText;
                return new ArrayResult<NotificationBase>(error);
            }

            var jArray = JsonConvert.DeserializeObject<JArray>(resp.DataAsText, JsonSerializerSettings);
            var notificationsList = new List<NotificationBase>();

            foreach (var token in jArray)
            {
                var notification = DeserializeNotificationByType(token);
                if(notification == null) continue;
                notificationsList.Add(notification);
            }
            
            return new ArrayResult<NotificationBase>(notificationsList.ToArray());
        }

        private NotificationBase DeserializeNotificationByType(JToken token)
        {
            if (!_serializationMapping.TryGetValue(token["notificationType"].ToString(), out var provider))
            {
                return null;
            }
            
            return (NotificationBase) JsonConvert.DeserializeObject(
                token.ToString(), provider, JsonSerializerSettings);
        }
    }
}