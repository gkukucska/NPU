namespace ClientInterfaces
{
    public interface IRegistrationClient
    {
        Task RegisterAsync(string userName, string password, CancellationToken token);

        Task ValidateRegistrationDataAsync(string userName, string sessionToken, CancellationToken token);
    }

    public static class RegistrationClientExtensions
    {
        public static Task RegisterAsync(this IRegistrationClient client, string userName, string password)
            => client.RegisterAsync(userName, password, CancellationToken.None);
        public static void Register(this IRegistrationClient client, string userName, string password)
            => client.RegisterAsync(userName, password).Wait();
        public static Task ValidateRegistrationDataAsync(this IRegistrationClient client, string userName, string password)
            => client.ValidateRegistrationDataAsync(userName, password, CancellationToken.None);
        public static void ValidateRegistrationData(this IRegistrationClient client, string userName, string password)
            => client.ValidateRegistrationDataAsync(userName, password).Wait();
    }
}