using Bridge;
using UnityEngine;

namespace ApiTests.AuthorizationTests
{
    public class ParentEmailCheckTest : MonoBehaviour
    {
        [SerializeField] private FFEnvironment _environment;
        [SerializeField] private string _username;

        private async void Start()
        {
            var bridge = new ServerBridge();
            bridge.ChangeEnvironment(_environment);

            var result = await bridge.CheckIfParentEmailBound(_username);
            if (!result.IsSuccess)
            {
                Debug.LogError(result.ErrorMessage);
            }
            Debug.Log(result.Model);
        }
    }
}