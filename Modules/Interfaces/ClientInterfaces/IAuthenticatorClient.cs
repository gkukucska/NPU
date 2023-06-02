namespace ClientInterfaces
{
    public interface IAuthenticatorClient
    {
        Task<string> OpenSessionAsync(string userName, string password, CancellationToken token);
        Task CloseSessionAsync(string userName, string sessionToken, CancellationToken token);
        Task<bool> ValidateSessionAsync(string userName, string sessionToken, CancellationToken token);
    }

    public static class AuthenticatorClientExtensions
    {
        public static Task<string> OpenSessionAsync(this IAuthenticatorClient client, string userName, string password)
            => client.OpenSessionAsync(userName, password, CancellationToken.None);
        public static string OpenSession(this IAuthenticatorClient client, string userName, string password)
            => client.OpenSessionAsync(userName, password).Result;
        public static Task CloseSessionAsync(this IAuthenticatorClient client, string userName, string sessionToken)
            => client.CloseSessionAsync(userName, sessionToken, CancellationToken.None);
        public static void CloseSession(this IAuthenticatorClient client, string userName, string sessionToken)
            => client.CloseSessionAsync(userName, sessionToken).Wait();
        public static Task<bool> ValidateSessionAsync(this IAuthenticatorClient client, string userName, string sessionToken)
            => client.ValidateSessionAsync(userName, sessionToken, CancellationToken.None);
        public static bool ValidateSession(this IAuthenticatorClient client, string userName, string sessionToken)
            => client.ValidateSessionAsync(userName, sessionToken).Result;
    }
}