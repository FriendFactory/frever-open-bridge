namespace Bridge.Results
{
    public abstract class FileUpdateResult:Result
    {
        protected string UpdatedVersionId { get; set; }

        protected FileUpdateResult(bool isCanceled): base(isCanceled)
        {
        }

        protected FileUpdateResult(string errorMessage) : base(errorMessage)
        {
        }
    }
}