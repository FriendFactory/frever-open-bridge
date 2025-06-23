namespace Bridge
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBridgeAutomationHelper
    {
#if UNITY_EDITOR
        void RunInEditor();
#endif
    }
}