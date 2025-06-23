namespace Bridge.ClientServer
{
    public interface IOkResponse
    {
        bool Ok { get; }
        string ErrorMessage { get; }
    }
}
