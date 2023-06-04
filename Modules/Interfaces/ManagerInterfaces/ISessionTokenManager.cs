namespace NPU.Interfaces
{
    public interface ISessionTokenManager
    {
        void CloseSession(string username, string token);
        string GetSessionToken(string username, string password);
        bool ValidateSession(string username, string token);
    }

}
