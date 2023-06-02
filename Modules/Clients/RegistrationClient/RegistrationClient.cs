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
        public Task<bool> RegisterAsync(string userName, string password, CancellationToken token)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5005", new GrpcChannelOptions() { MaxRetryAttempts = 1 });
            var registrationServiceClient = new RegistrationServiceClient(channel);
            var grpctask = registrationServiceClient.RegisterAsync(new RegistrationData()
            {
                UserName = userName,
                Password = password
            }, cancellationToken: token).ResponseAsync;
            grpctask.ContinueWith(t => { channel.Dispose(); });
            return grpctask.ContinueWith((t) => t.Result.IsSucceeded);
        }
        public Task<bool> ValidateRegistrationDataAsync(string userName, string password, CancellationToken token)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5005", new GrpcChannelOptions() { MaxRetryAttempts = 1 });
            var registrationServiceClient = new RegistrationServiceClient(channel);
            var grpctask = registrationServiceClient.ValidateRegistrationDataAsync(new RegistrationData()
            {
                UserName = userName,
                Password = password
            }, cancellationToken: token).ResponseAsync;
            grpctask.ContinueWith(t => { channel.Dispose(); });
            return grpctask.ContinueWith((t) => t.Result.IsValid);
        }
    }
}