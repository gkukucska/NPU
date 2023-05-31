﻿namespace ClientInterfaces
{
    public interface IAuthenticatorClient
    {
        Task OpenSessionAsync(string userName, string password, CancellationToken token);
        Task CloseSessionAsync(string userName, string sessionToken, CancellationToken token);
        Task<bool> ValidateSessionAsync(string userName, string sessionToken, CancellationToken token);
    }

    public static class AuthenticatorClientExtensions
    {
        public static Task OpenSessionAsync(this IAuthenticatorClient client, string userName, string password)
            => client.OpenSessionAsync(userName, password, CancellationToken.None);
        public static void OpenSession(this IAuthenticatorClient client, string userName, string password)
            => client.OpenSessionAsync(userName, password).Wait();
        public static Task CloseSessionAsync(this IAuthenticatorClient client, string userName, string password)
            => client.CloseSessionAsync(userName, password, CancellationToken.None);
        public static void CloseSession(this IAuthenticatorClient client, string userName, string password)
            => client.CloseSessionAsync(userName, password).Wait();
        public static Task<bool> ValidateSessionAsync(this IAuthenticatorClient client, string userName, string password)
            => client.ValidateSessionAsync(userName, password, CancellationToken.None);
        public static bool ValidateSession(this IAuthenticatorClient client, string userName, string password)
            => client.ValidateSessionAsync(userName, password).Result;
    }
}