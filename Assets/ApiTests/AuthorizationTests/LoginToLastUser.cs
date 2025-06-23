using System.Threading.Tasks;
using Bridge;
using UnityEngine;

namespace ApiTests.AuthorizationTests
{
    public class LoginToLastUser : MonoBehaviour
    {
        async Task Start()
        {
            var bridge = new ServerBridge();
            bridge.ChangeEnvironment(FFEnvironment.Test);
            Debug.Log("Last environment was: " + bridge.LastLoggedEnvironment);
            var res = await bridge.LoginToLastSavedUserAsync();
            if (res.IsSuccess)
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.LogError(res.ErrorMessage);
            }
        }
    }
}
