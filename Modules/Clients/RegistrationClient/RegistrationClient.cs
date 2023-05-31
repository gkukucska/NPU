using ClientInterfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using NPU.Protocols;
using static NPU.Protocols.RegistrationService;

namespace NPU.Clients.RegistrationClient
{
    public class RegistrationClient : IRegistrationClient
    {
        private RegistrationServiceClient _registrationServiceClient;
        private GrpcChannel _channel;
        public RegistrationClient()
        {
            _channel = GrpcChannel.ForAddress("http://localhost:5005", new GrpcChannelOptions() { MaxRetryAttempts = 1 });
            _registrationServiceClient = new RegistrationServiceClient(_channel);
        }

        public Task RegisterAsync(string userName, string password, CancellationToken token)
            => _registrationServiceClient.RegisterAsync(new RegistrationData()
            {
                UserName = userName,
                Password = password
            }, cancellationToken: token).ResponseAsync;

        public Task ValidateRegistrationDataAsync(string userName, string password, CancellationToken token)
            => _registrationServiceClient.ValidateRegistrationDataAsync(new RegistrationData()
            {
                UserName = userName,
                Password = password
            }, cancellationToken: token).ResponseAsync;
    }
}