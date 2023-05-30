namespace ClientInterfaces
{
    public interface IAuthenticatorClient
    {
        Task OpenSessionAsync(string username, string password);
        Task CloseSessionAsync(string username, string sessiontoken);
    }
}