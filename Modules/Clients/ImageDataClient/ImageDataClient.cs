using ClientInterfaces;
using Google.Protobuf;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using NPU.Interfaces;
using NPU.Protocols;
using static NPU.Protocols.ImageDataService;
using static System.Net.Mime.MediaTypeNames;

namespace NPU.Clients.ImageDataClient
{
    public class ImageDataClient : IImageDataClient
    {
        private static readonly string _serverURI = "http://localhost:5105";
        private readonly ILogger _logger;
        private readonly IAuthenticatorProvider _authenticatorProvider;

        public ImageDataClient(IAuthenticatorProvider authenticatorProvider,ILogger<ImageDataClient> logger)
        {
            _authenticatorProvider = authenticatorProvider;
            _logger = logger;
        }

        public Task<(byte[] ImageData, string Description, string ImageID)> GetFirstImageAsync(string userName, string sessionToken, CancellationToken token)
        {
            _logger.LogDebug($"Get first image data request for user {userName}");
            var channel = GrpcChannel.ForAddress(_serverURI, new GrpcChannelOptions() { MaxRetryAttempts = 1 });
            var authenticationServiceClient = new ImageDataServiceClient(channel);
            var grpctask = authenticationServiceClient.GetFirstImageDataAsync(new ImageSessionData()
            {
                UserName = userName,
                SessionToken = sessionToken,
            }, cancellationToken: token).ResponseAsync;
            grpctask.ContinueWith(t =>
            {
                channel.Dispose();
            });
            grpctask.ContinueWith(t => 
            {
                if (!t.Result.IsSessionValid)
                {
                    _logger.LogInformation($"Get first image data request for user {userName} resulted in timed out session, forcing logout");
                    _authenticatorProvider.ForceLogout();
                }
            }, continuationOptions: TaskContinuationOptions.OnlyOnRanToCompletion);
            return grpctask.ContinueWith((t) =>
            (t.Result.SerializedImage.ToByteArray(), t.Result.Description,t.Result.ImageID));
        }

        public Task<(byte[] ImageData, string Description, string ImageID)> GetImageDataAsync(string userName, string sessionToken, string imageID, CancellationToken token)
        {
            _logger.LogDebug($"Get image data request for user {userName}");
            var channel = GrpcChannel.ForAddress(_serverURI, new GrpcChannelOptions() { MaxRetryAttempts = 1 });
            var authenticationServiceClient = new ImageDataServiceClient(channel);
            var grpctask = authenticationServiceClient.GetImageDataAsync(new ImageIdentiferData()
            {
                SessionData = new ImageSessionData() { UserName = userName, SessionToken = sessionToken },
                ImageID = imageID
            }, cancellationToken: token).ResponseAsync;
            grpctask.ContinueWith(t =>
            {
                channel.Dispose();
            });
            grpctask.ContinueWith(t =>
            {
                if (!t.Result.IsSessionValid)
                {
                    _logger.LogInformation($"Get image data request for user {userName} resulted in timed out session, forcing logout");
                    _authenticatorProvider.ForceLogout();
                }
            }, continuationOptions: TaskContinuationOptions.OnlyOnRanToCompletion);
            return grpctask.ContinueWith((t) => (t.Result.SerializedImage.ToByteArray(), t.Result.Description, t.Result.ImageID));
        }

        public Task<(byte[] ImageData, string Description, string ImageID)> GetNextImageDataAsync(string userName, string sessionToken, string imageID, CancellationToken token)
        {
            _logger.LogDebug($"Get next image data request for user {userName}");
            var channel = GrpcChannel.ForAddress(_serverURI, new GrpcChannelOptions() { MaxRetryAttempts = 1 });
            var authenticationServiceClient = new ImageDataServiceClient(channel);
            var grpctask = authenticationServiceClient.GetNextImageDataAsync(new ImageIdentiferData()
            {
                SessionData = new ImageSessionData() { UserName = userName, SessionToken = sessionToken },
                ImageID = imageID
            }, cancellationToken: token).ResponseAsync;
            grpctask.ContinueWith(t =>
            {
                channel.Dispose();
            });
            grpctask.ContinueWith(t =>
            {
                if (!t.Result.IsSessionValid)
                {
                    _logger.LogInformation($"Get next image data request for user {userName} resulted in timed out session, forcing logout");
                    _authenticatorProvider.ForceLogout();
                }
            }, continuationOptions: TaskContinuationOptions.OnlyOnRanToCompletion);
            return grpctask.ContinueWith((t) => (t.Result.SerializedImage.ToByteArray(), t.Result.Description, t.Result.ImageID));
        }

        public Task RemoveImageDataAsync(string userName, string sessionToken, string imageID, CancellationToken token)
        {
            _logger.LogDebug($"Remove image data request for user {userName}");
            var channel = GrpcChannel.ForAddress(_serverURI, new GrpcChannelOptions() { MaxRetryAttempts = 1 });
            var authenticationServiceClient = new ImageDataServiceClient(channel);
            var grpctask = authenticationServiceClient.RemoveImageDataAsync(new ImageIdentiferData()
            {
                SessionData = new ImageSessionData() { UserName = userName, SessionToken = sessionToken },
                ImageID = imageID
            }, cancellationToken: token).ResponseAsync;
            grpctask.ContinueWith(t =>
            {
                channel.Dispose();
            });
            grpctask.ContinueWith(t =>
            {
                if (!t.Result.IsSessionValid)
                {
                    _logger.LogInformation($"Remove image data request for user {userName} resulted in timed out session, forcing logout");
                    _authenticatorProvider.ForceLogout();
                }
            }, continuationOptions: TaskContinuationOptions.OnlyOnRanToCompletion);
            return grpctask;
        }

        public Task<string> SaveImageDataAsync(string userName, string sessionToken, byte[] imageData, string description, CancellationToken token)
        {
            _logger.LogDebug($"Save image data request for user {userName}");
            var channel = GrpcChannel.ForAddress(_serverURI, new GrpcChannelOptions() { MaxRetryAttempts = 1 });
            var authenticationServiceClient = new ImageDataServiceClient(channel);
            var grpctask = authenticationServiceClient.SaveImageDataAsync(new ImageUploadData()
            {
                SessionData = new ImageSessionData() { UserName = userName, SessionToken = sessionToken },
                SerializedImage = ByteString.CopyFrom(imageData),
                Description = description
            }, cancellationToken: token).ResponseAsync;
            grpctask.ContinueWith(t =>
            {
                channel.Dispose();
            });
            grpctask.ContinueWith(t =>
            {
                if (!t.Result.IsSessionValid)
                {
                    _logger.LogInformation($"Save image data request for user {userName} resulted in timed out session, forcing logout"); 
                    _authenticatorProvider.ForceLogout();
                }
            }, continuationOptions: TaskContinuationOptions.OnlyOnRanToCompletion);
            return grpctask.ContinueWith((t) => t.Result.ImageID);
        }
    }
}