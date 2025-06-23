using UnityEngine;

namespace Bridge.Results
{
    public sealed class ChatMessageFilesResult : Result
    {
        public Texture2D[] Images { get; private set; }

        private ChatMessageFilesResult(bool isCancelled): base(isCancelled)
        {
        }

        private ChatMessageFilesResult(string error) : base(error)
        {
        }
        
        internal static ChatMessageFilesResult Success(Texture2D[] images)
        {
            return new ChatMessageFilesResult(false)
            {
                Images = images
            };
        }

        internal static ChatMessageFilesResult Cancelled()
        {
            return new ChatMessageFilesResult(true);
        }

        internal static ChatMessageFilesResult Error(string error, Texture2D[] images = null)
        {
            return new ChatMessageFilesResult(error)
            {
                Images = images
            };
        }
    }
}