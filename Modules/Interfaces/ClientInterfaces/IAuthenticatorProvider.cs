namespace NPU.Interfaces
{ 
    public interface IAuthenticatorProvider
    {
        string UserName { get; }
        string SessionToken { get; }

    }
}