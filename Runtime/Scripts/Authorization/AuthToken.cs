namespace Bridge.Authorization
{
    [System.Serializable]
    public class AuthToken
    {
        public string Value;
        public string RefreshToken;

        public AuthToken()
        {

        }

        public AuthToken(string value, string refreshToken)
        {
            Value = value;
            RefreshToken = refreshToken;
        }
    }
}