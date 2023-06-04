using System.Xml.Serialization;

namespace NPU.Interfaces
{ 
    public interface IAuthenticatorProvider
    {
        string UserName { get; }
        string SessionToken { get; }

        void ForceLogout();

        event EventHandler<EventArgs> OnLogout;

    }
}