using System.IO;
using Bridge.ClientServer.ImageGeneration;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.ImageGeneration
{
    internal sealed class ReplicateImageGenerationTest: AuthorizedUserApiTestBase
    {
        [SerializeField] private string _prompt = "Cat and dog are walking in Stockholm";
        
        protected override async void RunTestAsync()
        {
            var req = new ReplicateRequest();
            req.Version = "4b8e37b91a60fdc9dba0e17a73267b5428afc232a9d2e33e67895fe521514536";
            req.Input = new ReplicateInput();
            req.Input.Prompt = _prompt;
            var resp = await Bridge.GenerateImage(req);
            Debug.Log(JsonConvert.SerializeObject(resp));
            Debug.Log($"### File exists: {File.Exists(resp.Model.LocalFilePath)}");
        }
    }
}