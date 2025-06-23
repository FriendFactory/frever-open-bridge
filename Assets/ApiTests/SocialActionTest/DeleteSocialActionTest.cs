using System.Threading;
using UnityEngine;

namespace ApiTests.SocialActionTest
{
    public class DeleteSocialActionTest : AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var getResult = await Bridge.GetPersonalisedSocialActions();
            
            if (getResult.IsError)
            {
                Debug.LogError($"Get Social Actions failed\n{getResult.ErrorMessage}");
                return;
            }

            if (getResult.Models.Length == 0)
            {
                Debug.LogError($"No social actions available");
                return;
            }

            var model = getResult.Models[0];
            var deleteResult = await Bridge.DeleteSocialAction(model.RecommendationId, model.ActionId);

            if (deleteResult.IsSuccess)
            {
                Debug.Log($"Successfully deleted action");
                return;
            }

            Debug.LogError(deleteResult.ErrorMessage);
        }
    }
}