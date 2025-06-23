namespace ApiTests.BodyAnimationTests
{
    internal sealed class DownloadBodyAnimationList: AuthorizedUserApiTestBase
    {
        public int Take = 20;
        public int TakePrevious = 0;
        public long RaceId = 1;
        
        protected override async void RunTestAsync()
        {
            var resp = await Bridge.GetBodyAnimationListAsync(null,Take, TakePrevious, RaceId);
        }
    }
}