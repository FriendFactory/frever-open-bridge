using Bridge;
using Bridge.Authorization.Models;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.CredentialValidations
{
    public class NickNameValidation : MonoBehaviour
    {
        public string NickName;
        public FFEnvironment Environment = FFEnvironment.Develop;
    
        async void Start()
        {
            var bridge = new ServerBridge();
            bridge.ChangeEnvironment(Environment);
            var validationModel = new ValidationModel()
            {
                UserName = NickName
            };
            var resp = await bridge.ValidateRegistrationCredentials(validationModel);
            Debug.Log(JsonConvert.SerializeObject(resp));
        }
    }
}
