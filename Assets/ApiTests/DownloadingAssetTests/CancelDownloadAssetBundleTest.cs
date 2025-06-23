using System.Linq;
using System.Threading;
using Bridge.AssetManagerServer;
using Bridge.ExternalPackages.AsynAwaitUtility;
using Bridge.Models.AsseManager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ApiTests.DownloadingAssetTests
{
    internal sealed class CancelDownloadAssetBundleTest: AuthorizedUserApiTestBase
    {
        [SerializeField] private KeyCode CancelKey = KeyCode.Space;
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();

        protected override async void RunTestAsync()
        {
            var setLocationQuery = new Query<SetLocation>();
            setLocationQuery.SetMaxTop(1);
            setLocationQuery.SetOrderBy(nameof(SetLocation.Id), OrderByType.Descend);
            setLocationQuery.ExpandField(nameof(SetLocation.SetLocationBundle));

            var response = await Bridge.GetAsync(setLocationQuery, _tokenSource.Token);
            var setLocation = response.Models.First();
            await Bridge.ClearCacheAsync();
            Debug.Log("Start loading asset bundle");
            var assetResult = await Bridge.GetAssetAsync(setLocation.SetLocationBundle, false, _tokenSource.Token);
            Debug.Log("Loaded result is canceled: " + assetResult.IsRequestCanceled);
            if (assetResult.IsSuccess)
            {
                var scenePath = (assetResult.Object as AssetBundle).GetAllScenePaths().First();
                await SceneManager.LoadSceneAsync(scenePath);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(CancelKey))
            {
                Debug.Log("Cancel");
                _tokenSource.Cancel();
            }
        }
    }
}