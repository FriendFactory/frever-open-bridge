using System.Collections.Generic;
using System.Linq;
using Bridge.AssetManagerServer;
using Bridge.Models.AsseManager;
using Bridge.Models.ClientServer.Assets;
using Bridge.Services.UserProfile;
using UnityEngine;

namespace ApiTests.ProfileTests
{
    internal sealed class UpdateProfileTest: AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var userInfoResp = await Bridge.GetCurrentUserInfo();
            var gendersResp = await Bridge.GetAsync(new Query<Gender>());
            var userInfo = userInfoResp.Profile;
            var currentGender = userInfo.GenderId;
            var updateGenderValue = gendersResp.Models.First(x => x.Id != currentGender).Id;
            var updateModel = new UpdateProfileRequest(userInfo.MainCharacterId, userInfo.Nickname, updateGenderValue, userInfo.TaxationCountryId, CharacterAccess.Private, "sdasd", new Dictionary<string, string>());
            var updateResp = await Bridge.UpdateUserProfile(updateModel);
            if(updateResp.IsSuccess) 
                Debug.Log("Updated successfully");
            else
                Debug.LogError($"Failed. Reason: {updateResp.ErrorMessage}");
        }
    }
}