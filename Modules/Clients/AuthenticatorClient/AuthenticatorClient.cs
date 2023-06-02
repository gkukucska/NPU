using ClientInterfaces;
using Google.Protobuf;
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
        public Task CloseSessionAsync(string username, string sessiontoken, CancellationToken token)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5005", new GrpcChannelOptions() { MaxRetryAttempts = 1 });
            var authenticationServiceClient = new AuthenticationServiceClient(channel);
            var grpctask = authenticationServiceClient.CloseSessionAsync(new SessionData()
            {
                UserName = username,
                SessionToken = sessiontoken
            }, cancellationToken: token).ResponseAsync;
            grpctask.ContinueWith(t => { channel.Dispose(); });
            return grpctask;
        }

        public Task<string> OpenSessionAsync(string username, string password, CancellationToken token)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5005", new GrpcChannelOptions() { MaxRetryAttempts = 1 });
            var authenticationServiceClient = new AuthenticationServiceClient(channel);
            var grpctask = authenticationServiceClient.OpenSessionAsync(new LoginCredentialData()
            {
                UserName = username,
                Password = password
            }, cancellationToken: token).ResponseAsync;
            grpctask.ContinueWith(t => { channel.Dispose(); });
            return grpctask.ContinueWith((t) => t.Result.SessionToken);
        }

        public Task<bool> ValidateSessionAsync(string userName, string sessionToken, CancellationToken token)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5005", new GrpcChannelOptions() { MaxRetryAttempts = 1 });
            var authenticationServiceClient = new AuthenticationServiceClient(channel);
            var grpctask = authenticationServiceClient.ValidateSessionAsync(new SessionData()
            {
                UserName = userName,
                SessionToken = sessionToken
            }, cancellationToken: token).ResponseAsync;
            grpctask.ContinueWith(t => { channel.Dispose(); });
            return grpctask.ContinueWith((t) => t.Result.IsValid);
        }
    }
}