using System;
using UnityEngine;

namespace ApiTests.CrewTests
{
    public class GetMyCrewChatTest: AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            try
            {
                var result = await Bridge.GetMyCrewChat();
                if (result.IsError)
                {
                    Debug.LogError($"[{GetType().Name}] Failed to get my crew chat # {result.ErrorMessage}");
                    return;
                }
                
                if (result.IsRequestCanceled) return;

                var chat = result.Model;
                
                if (chat == null) return;
                
                Debug.Log($"[{GetType().Name}] My crew chat name is {chat.Title}");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}