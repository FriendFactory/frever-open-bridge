using Bridge.Results;

namespace Bridge.Authorization
{
    public sealed class ValidationInvitationCodeResult: Result
    {
        public bool IsValid { get; private set; }
        public string ReasonPhrase { get; private set; }

        private ValidationInvitationCodeResult()
        {
        }
        
        private ValidationInvitationCodeResult(string error): base(error)
        {
        }
        
        internal static ValidationInvitationCodeResult Valid()
        {
            return new ValidationInvitationCodeResult()
            {
                IsValid = true
            };
        }
        
        internal static ValidationInvitationCodeResult NonValid(string reasonPhrase)
        {
            return new ValidationInvitationCodeResult()
            {
                IsValid = false,
                ReasonPhrase = reasonPhrase
            };
        }
        
        internal static ValidationInvitationCodeResult Error(string errorMessage)
        {
            return new ValidationInvitationCodeResult(errorMessage);
        }
    }
}