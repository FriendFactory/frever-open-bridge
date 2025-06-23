using UnityEngine;

namespace ApiTests
{
    public class CrewLanguageTest : AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var result = await Bridge.GetCrewsList("", 10, 0, null, default);
            if (result.IsSuccess)
            {
                foreach (var model in result.Models)
                {
                    Debug.Log(model.LanguageId);
                }
            }
            
            if (result.IsError) Debug.Log(result.ErrorMessage);

            var langResult = await Bridge.GetAvailableLanguages(default);
            if (langResult.IsSuccess)
            {
                foreach (var model in langResult.Models)
                {
                    Debug.Log(model.Name);
                }
            }
            
            if (langResult.IsError) Debug.Log(langResult.ErrorMessage);
        }
    }
}