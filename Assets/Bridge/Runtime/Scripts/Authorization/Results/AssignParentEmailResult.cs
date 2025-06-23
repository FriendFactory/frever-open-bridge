using Bridge.Results;

namespace Bridge.Authorization.Results
{
    public sealed class AssignParentEmailResult: Result
    {
        public string NewEmailCode { get; private set; }

        private AssignParentEmailResult()
        {
        }

        private AssignParentEmailResult(string error) : base(error)
        {
        }
        
        internal static AssignParentEmailResult Success(string newEmailCode)
        {
            return new AssignParentEmailResult()
            {
                NewEmailCode = newEmailCode
            };
        }

        internal static AssignParentEmailResult Error(string error)
        {
            return new AssignParentEmailResult(error);
        }
    }
}