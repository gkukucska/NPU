namespace ClientInterfaces
{
    public interface IRegistrationClient
    {
        Task<bool> RegisterAsync(string userName, string password, CancellationToken token);

        Task<bool> ValidateRegistrationDataAsync(string userName, string sessionToken, CancellationToken token);
    }

    public static class RegistrationClientExtensions
    {
        public static Task<bool> RegisterAsync(this IRegistrationClient client, string userName, string password)
            => client.RegisterAsync(userName, password, CancellationToken.None);
        public static bool Register(this IRegistrationClient client, string userName, string password)
            => client.RegisterAsync(userName, password).Result;
        public static Task<bool> ValidateRegistrationDataAsync(this IRegistrationClient client, string userName, string password)
            => client.ValidateRegistrationDataAsync(userName, password, CancellationToken.None);
        public static bool ValidateRegistrationData(this IRegistrationClient client, string userName, string password)
            => client.ValidateRegistrationDataAsync(userName, password).Result;
    }
}