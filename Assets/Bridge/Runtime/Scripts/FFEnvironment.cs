namespace Bridge
{
    /// <summary>
    /// We must keep environment values intact since we are storing their values locally to know which env used in last session
    /// </summary>
    public enum FFEnvironment 
    {
        Production = 1,
        Stage = 2,
        Test = 3,
        Develop = 4,
        ProductionUSA = 5
    }
}
