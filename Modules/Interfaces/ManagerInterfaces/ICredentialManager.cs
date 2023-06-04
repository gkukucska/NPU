namespace NPU.Interfaces
{
    public interface ICredentialManager
    {
        bool IsUserTaken(string username);
        Task<bool> RegisterUserAsync(string username, string password, CancellationToken token);
        bool IsCredentialValid(string username, string password);   

    }
}
