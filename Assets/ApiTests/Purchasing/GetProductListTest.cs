using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests.Purchasing
{
    internal sealed class GetProductListTest : AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var products = await Bridge.GetProductOffers();
            if (products.IsError)
            {
                Debug.LogError($"Failed: {products.ErrorMessage}");
            }
            else
            {
                Debug.Log($"{JsonConvert.SerializeObject(products.Model)}");
            }
        }
    }
}
