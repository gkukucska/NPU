using ClientInterfaces;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using NPU.Interfaces;
using NPU.Protocols;
using System.Net;
using System.Security;
using static NPU.Protocols.AuthenticationService;

namespace NPU.Clients.AuthenticatorClient
{
    public class AuthenticatorClient : IAuthenticatorClient
    {
        private static readonly string _serverURI = "http://localhost:5005";
        private readonly ILogger _logger;

        public AuthenticatorClient(ILogger<AuthenticatorClient> logger)
        {
            _logger = logger;
        }

        public Task CloseSessionAsync(string userName, string sessiontoken, CancellationToken token)
        {
            _logger.LogDebug($"Close session request for user {userName}");
            var channel = GrpcChannel.ForAddress(_serverURI, new GrpcChannelOptions() { MaxRetryAttempts = 1 });
            var authenticationServiceClient = new AuthenticationServiceClient(channel);
            var grpctask = authenticationServiceClient.CloseSessionAsync(new SessionData()
            {
                UserName = userName,
                SessionToken = sessiontoken
            }, cancellationToken: token).ResponseAsync;
            grpctask.ContinueWith(t => { channel.Dispose(); });
            return grpctask;
        }

        public Task<string> OpenSessionAsync(string userName, string password, CancellationToken token)
        {
            _logger.LogDebug($"Open session request for user {userName}");
            var channel = GrpcChannel.ForAddress(_serverURI, new GrpcChannelOptions() { MaxRetryAttempts = 1 });
            var authenticationServiceClient = new AuthenticationServiceClient(channel);
            var grpctask = authenticationServiceClient.OpenSessionAsync(new LoginCredentialData()
            {
                UserName = userName,
                Password = password
            }, cancellationToken: token).ResponseAsync;
            grpctask.ContinueWith(t => { channel.Dispose(); });
            return grpctask.ContinueWith((t) => t.Result.SessionToken);
        }

        public Task<bool> ValidateSessionAsync(string userName, string sessionToken, CancellationToken token)
        {
            _logger.LogDebug($"Session validation request for user {userName}");
            var channel = GrpcChannel.ForAddress(_serverURI, new GrpcChannelOptions() { MaxRetryAttempts = 1 });
            var authenticationServiceClient = new AuthenticationServiceClient(channel);
            var grpctask = authenticationServiceClient.ValidateSessionAsync(new SessionData()
            {
                UserName = userName,
                SessionToken = sessionToken
            }, cancellationToken: token).ResponseAsync;
            grpctask.ContinueWith(t => { channel.Dispose(); });
            return grpctask.ContinueWith((t) =>
            {
                _logger.LogInformation($"Result of session validation request for user {userName} is {t.Result.IsValid}");
                return t.Result.IsValid;
            });
        }
    }
}