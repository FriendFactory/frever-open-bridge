using System;
using ApiTests;
using UnityEngine;

public class GetSoundsTest : AuthorizedUserApiTestBase
{
    private readonly long[] _songIds = new long[] { 2L, 4L, 8L };
    private readonly long[] _userSoundIds = new long[] { 16L, 32L, 64L };
    private readonly long[] _externalSongIds = new long[] { 128L, 256L, 512L };

    protected override async void RunTestAsync()
    {
        try
        {
            var result = await Bridge.GetSounds(_songIds, _userSoundIds, _externalSongIds, default);
            if (result.IsError)
            {
                Debug.LogError($"[{GetType().Name}] Failed to get sounds # {result.ErrorMessage}");
                return;
            }

            var songs = result.Model.Songs;
            var userSounds = result.Model.UserSounds;
            var externalSongs = result.Model.ExternalSongs;
            
            // TODO: compare ids from response with passed ones
            Debug.Log($"[{GetType().Name}] songs: {songs?.Length ?? 0}, userSounds: {userSounds?.Length ?? 0}, external songs: {externalSongs?.Length ?? 0}");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
