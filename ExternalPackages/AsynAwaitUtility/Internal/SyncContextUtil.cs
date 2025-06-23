using System.Threading;
using UnityEngine;

namespace Bridge.ExternalPackages.AsynAwaitUtility.Internal
{
    public static class SyncContextUtil
    {
        static SyncContextUtil()
        {
            UnitySynchronizationContext = SynchronizationContext.Current;
            UnityThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        public static int UnityThreadId
        {
            get; private set;
        }

        public static SynchronizationContext UnitySynchronizationContext
        {
            get; private set;
        }
    }
}

