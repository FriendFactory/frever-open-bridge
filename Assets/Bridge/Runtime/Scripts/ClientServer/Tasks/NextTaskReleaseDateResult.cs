using System;
using Bridge.Results;

namespace Bridge.ClientServer.Tasks
{
    public sealed class NextTaskReleaseDateResult : Result
    {
        public bool HasUpcomingTask { get; private set; }
        public DateTime? DateTime { get; private set; }
        
        public static NextTaskReleaseDateResult Success(DateTime dateTime)
        {
            return new NextTaskReleaseDateResult
            {
                HasUpcomingTask = true,
                DateTime = dateTime
            };
        }
        
        public static NextTaskReleaseDateResult DontHaveNextTaskReleaseDateResult()
        {
            return new NextTaskReleaseDateResult
            {
                HasUpcomingTask = false
            };
        }

        public static NextTaskReleaseDateResult Cancelled()
        {
            return new NextTaskReleaseDateResult(true);
        }

        public static NextTaskReleaseDateResult Error(string error)
        {
            return new NextTaskReleaseDateResult(error);
        }

        private NextTaskReleaseDateResult()
        {
        }

        private NextTaskReleaseDateResult(bool isCanceled): base(isCanceled)
        {
        }
        
        private NextTaskReleaseDateResult(string error): base(error)
        {
        }
    }
}