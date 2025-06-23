using Bridge.Results;
using Bridge.Services.CreatePage;
using UnityEngine;

namespace ApiTests.CreatePageTests
{
    public sealed class GetCreatePageContentTests : AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var contentResp = await Bridge.GetCreatePageContent();
            if (contentResp.IsError)
            {
                Debug.LogError($"Error: {contentResp.ErrorMessage}");
                return;
            }

            Result result = null;
            foreach (var row in contentResp.Model.Rows)
            {
                switch (row.ContentType)
                {
                    case CreatePageContentTypes.Video:
                        result = await Bridge.GetCreatePageRowVideo(row.Id, null, 10);
                        break;
                    case CreatePageContentTypes.Template:
                        result = await Bridge.GetCreatePageRowTemplates(row.Id, null, 10);
                        break;
                    case CreatePageContentTypes.Hashtag:
                        result = await Bridge.GetCreatePageRowHashtags(row.Id, null, 10);
                        break;
                    case CreatePageContentTypes.Song:
                        result = await Bridge.GetCreatePageRowExternalSongs(row.Id, null, 10);
                        break;
                }

                if (result != null)
                {
                    
                }
            }
        }
    }
}
