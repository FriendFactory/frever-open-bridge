using System.IO;
using ApiTests;
using UnityEngine;

public sealed class TranscodingTest : AuthorizedUserApiTestBase
{
   protected override async void RunTestAsync()
   {
      var path = Path.Combine(Application.streamingAssetsPath, "TestFiles" ,TestFileNames.VIDEO_MP4);
      var videoBytes = File.ReadAllBytes(path);
      var res = await Bridge.ExtractAudioAsync(videoBytes, 15);

      if (res.IsError)
      {
         Debug.LogError(res.ErrorMessage);
         return;
      }

      var audioSource = gameObject.AddComponent<AudioSource>();
      audioSource.clip = res.AudioClip;
      audioSource.Play();
   }
}
