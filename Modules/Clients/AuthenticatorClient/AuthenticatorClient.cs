using ClientInterfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using NPU.Protocols;

namespace NPU.Clients.AuthenticatorClient
{
    public class AuthenticatorClient : NPU.Protocols.AuthenticationService.AuthenticationServiceBase, IAuthenticatorClient
    {
        public Task CloseSessionAsync(string username, string sessiontoken)
            => CloseSession(new SessionData()
            {
                UserName = username,
                SessionToken = sessiontoken
            }, null);

        public Task OpenSessionAsync(string username, string password)
            => OpenSession(new LoginCredentialData()
            {
                UserName = username,
                Password = password
            },null);

        public override Task<SessionData> OpenSession(LoginCredentialData request, ServerCallContext context)
        {
            return base.OpenSession(request, context);
        }

        public override Task<Empty> CloseSession(SessionData request, ServerCallContext context)
        {
            return base.CloseSession(request, context);
        }
    }
}