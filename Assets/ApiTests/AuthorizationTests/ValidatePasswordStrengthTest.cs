using Bridge;
using UnityEngine;

namespace ApiTests.AuthorizationTests
{
    public sealed class ValidatePasswordStrengthTest : MonoBehaviour
    {
        [SerializeField] private FFEnvironment _environment;
        [SerializeField] private string _password;
        [SerializeField] private string _nickname = "Nickname";

        private IBridge _bridge;

        private async void Start()
        {
            _bridge = new ServerBridge();
            _bridge.ChangeEnvironment(_environment);

            var result =  await _bridge.ValidatePasswordStrength(_password, _nickname);
            if (result.IsError)
            {
                Debug.LogError(result.ErrorMessage);
                return;
            }

            if (result.Model.Ok)
            {
                Debug.Log("Password Valid");
                return;
            }

            if (!result.Model.IsTooSimple)
            {
                Debug.Log("Password is too simple");
            }

            if (!result.Model.IsLongEnough)
            {
                Debug.Log("Password is too short");
            }


            if (!result.Model.IsStrong)
            {
                Debug.Log("Password it too weak");
            }
        }
    }
}
