using ClientInterfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using NPU.Protocols;
using System.Net;
using System.Security;
using static NPU.Protocols.AuthenticationService;

namespace NPU.Clients.AuthenticatorClient
{
    public class AuthenticatorClient : IAuthenticatorClient
    {
        private AuthenticationServiceClient _authenticationServiceClient;
        private GrpcChannel _channel;
        public AuthenticatorClient()
        {
            _channel = GrpcChannel.ForAddress("http://localhost:5005", new GrpcChannelOptions() { MaxRetryAttempts = 1 });
            _authenticationServiceClient = new AuthenticationServiceClient(_channel);
        }
        public Task CloseSessionAsync(string username, string sessiontoken, CancellationToken token)
            => _authenticationServiceClient.CloseSessionAsync(new SessionData()
            {
                UserName = username,
                SessionToken = sessiontoken
            }, cancellationToken: token).ResponseAsync;

        public Task<string> OpenSessionAsync(string username, string password, CancellationToken token)
            => _authenticationServiceClient.OpenSessionAsync(new LoginCredentialData()
            {
                UserName = username,
                Password = password
            }, cancellationToken: token).ResponseAsync.ContinueWith((t)=>t.Result.SessionToken);

        public Task<bool> ValidateSessionAsync(string userName,string sessionToken, CancellationToken token)
            => _authenticationServiceClient.ValidateSessionAsync(new SessionData()
            {
                UserName = userName,
                SessionToken = sessionToken
            },cancellationToken: token).ResponseAsync.ContinueWith((t)=>t.Result.IsValid);
    }
}