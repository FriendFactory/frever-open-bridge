using System;
using Bridge.VideoServer;
using UnityEngine;

namespace ApiTests.VideoTests
{
    internal sealed class GenerateLipSyncTest: AuthorizedUserApiTestBase
    {
        private enum SoundType
        {
            Song,
            UserSound,
            ExternalSong,
        }
        
        [SerializeField] private long _videoId;
        [Header("Source Audio")]
        [SerializeField] private SoundType _soundType = SoundType.Song;
        [SerializeField] private long _songId;
        [SerializeField] private long _userSoundId;
        [SerializeField] private long _externalSongId;
        
        protected override async void RunTestAsync()
        {
            try
            {
                var model = GetModel();
            
                var result = await Bridge.GenerateLipSync(_videoId, model);
                if (result.IsError)
                {
                    Debug.LogError($"[{GetType().Name}] Failed to generate lip sync: {result.ErrorMessage}");
                    return;
                }
                
                Debug.Log($"[{GetType().Name}] Lip sync generation scheduled successfully");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private LipSyncRequest GetModel()
        {
            var builder = new LipSyncRequestBuilder();

            return _soundType switch
            {
                SoundType.Song => builder.WithSongId(_songId).Build(),
                SoundType.UserSound => builder.WithUserSoundId(_userSoundId).Build(),
                SoundType.ExternalSong => builder.WithExternalSongId(_externalSongId).Build(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}