using ClientInterfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using NPU.Protocols;
using static NPU.Protocols.RegistrationService;

namespace NPU.Clients.RegistrationClient
{
    public class RegistrationClient : IRegistrationClient
    {
        private static readonly string _serverURI = "http://localhost:5005";
        private readonly ILogger _logger;

        public RegistrationClient(ILogger<RegistrationClient> logger)
        {
            _logger = logger;
        }
        public Task<bool> RegisterAsync(string userName, string password, CancellationToken token)
        {
            _logger.LogDebug($"Registration request for user {userName}");
            var channel = GrpcChannel.ForAddress(_serverURI, new GrpcChannelOptions() { MaxRetryAttempts = 1 });
            var registrationServiceClient = new RegistrationServiceClient(channel);
            var grpctask = registrationServiceClient.RegisterAsync(new RegistrationData()
            {
                UserName = userName,
                Password = password
            }, cancellationToken: token).ResponseAsync;
            grpctask.ContinueWith(t => { channel.Dispose(); });
            return grpctask.ContinueWith((t) =>
            {
                _logger.LogInformation($"Result of registration request for user {userName} is {t.Result.IsSucceeded}");
                return t.Result.IsSucceeded;
            });
        }
        public Task<bool> ValidateRegistrationDataAsync(string userName, string password, CancellationToken token)
        {
            _logger.LogDebug($"Registration validation request for user {userName}");
            var channel = GrpcChannel.ForAddress(_serverURI, new GrpcChannelOptions() { MaxRetryAttempts = 1 });
            var registrationServiceClient = new RegistrationServiceClient(channel);
            var grpctask = registrationServiceClient.ValidateRegistrationDataAsync(new RegistrationData()
            {
                UserName = userName,
                Password = password
            }, cancellationToken: token).ResponseAsync;
            grpctask.ContinueWith(t => { channel.Dispose(); });
            return grpctask.ContinueWith((t) =>
            {
                _logger.LogInformation($"Result of registration validation request for user {userName} is {t.Result.IsValid}");
                return t.Result.IsValid;
            });
        }
    }
}