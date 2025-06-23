using UnityEngine;

namespace ApiTests.SocialActionTest
{
    internal sealed class MarkSocialActionAsCompleteTest : AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var result = await Bridge.GetPersonalisedSocialActions();
            
            if (result.IsError)
            {
                Debug.LogError($"Get Social Actions failed\n{result.ErrorMessage}");
                return;
            }

            if (result.Models.Length == 0)
            {
                Debug.LogError($"No social actions available");
                return;
            }

            var model = result.Models[0];
            var completionResult = await Bridge.MarkActionAsComplete(model.RecommendationId, model.ActionId);

            if (completionResult.IsSuccess)
            {
                Debug.Log($"Successfully marked action as complete");
                return;
            }

            Debug.LogError(completionResult.ErrorMessage);
        }
    }
}