using UnityEngine;

namespace ApiTests.ProfileTests
{
    internal sealed class GetProfilePublicUrlTest: AuthorizedUserApiTestBase
    {
        [SerializeField] private string _nickName;

        protected override void RunTestAsync()
        {
            Bridge.ChangeEnvironment(Environment);
            var url = Bridge.GetUserProfilePublicUrl(_nickName);
            Application.OpenURL(url);
        }
    }
}