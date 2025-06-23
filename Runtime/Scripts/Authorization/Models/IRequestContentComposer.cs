using BestHTTP;

namespace Bridge.Authorization.Models
{
    public interface IRequestContentComposer
    {
        void ComposeRequestContent(HTTPRequest httpRequest);
    }
}