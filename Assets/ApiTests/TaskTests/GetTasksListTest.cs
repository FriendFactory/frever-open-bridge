namespace ApiTests.TaskTests
{
    internal sealed class GetTasksListTest: AuthorizedUserApiTestBase
    {
        protected override async void RunTestAsync()
        {
            var result = await Bridge.GetTasksAsync(null, 10, 10);
        }
    }
}