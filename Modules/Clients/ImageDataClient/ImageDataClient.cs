using ClientInterfaces;
using Google.Protobuf;
using Grpc.Net.Client;
using NPU.Interfaces;
using NPU.Protocols;
using static NPU.Protocols.ImageDataService;
using static System.Net.Mime.MediaTypeNames;

namespace NPU.Clients.ImageDataClient
{
    public class ImageDataClient : IImageDataClient
    {
        private IAuthenticatorProvider _authenticatorProvider;

        public ImageDataClient(IAuthenticatorProvider authenticatorProvider)
        {
            _authenticatorProvider = authenticatorProvider;
        }

        public Task<(byte[] ImageData, string Description, string ImageID)> GetFirstImageAsync(string userName, string sessionToken, CancellationToken token)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5105", new GrpcChannelOptions() { MaxRetryAttempts = 1 });
            var authenticationServiceClient = new ImageDataServiceClient(channel);
            var grpctask = authenticationServiceClient.GetFirstImageDataAsync(new ImageSessionData()
            {
                UserName = userName,
                SessionToken = sessionToken,
            }, cancellationToken: token).ResponseAsync;
            grpctask.ContinueWith(t =>
            {
                channel.Dispose();
                if (!t.Result.IsSessionValid)
                {
                    _authenticatorProvider.ForceLogout();
                }
            });
            return grpctask.ContinueWith((t) => (t.Result.SerializedImage.ToByteArray(), t.Result.Description,t.Result.ImageID));
        }

        public Task<(byte[] ImageData, string Description, string ImageID)> GetImageDataAsync(string userName, string sessionToken, string imageID, CancellationToken token)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5105", new GrpcChannelOptions() { MaxRetryAttempts = 1 });
            var authenticationServiceClient = new ImageDataServiceClient(channel);
            var grpctask = authenticationServiceClient.GetImageDataAsync(new ImageIdentiferData()
            {
                SessionData = new ImageSessionData() { UserName = userName, SessionToken = sessionToken },
                ImageID = imageID
            }, cancellationToken: token).ResponseAsync;
            grpctask.ContinueWith(t =>
            {
                channel.Dispose();
                if (!t.Result.IsSessionValid)
                {
                    _authenticatorProvider.ForceLogout();
                }
            });
            return grpctask.ContinueWith((t) => (t.Result.SerializedImage.ToByteArray(), t.Result.Description, t.Result.ImageID));
        }

        public Task<(byte[] ImageData, string Description, string ImageID)> GetNextImageDataAsync(string userName, string sessionToken, string imageID, CancellationToken token)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5105", new GrpcChannelOptions() { MaxRetryAttempts = 1 });
            var authenticationServiceClient = new ImageDataServiceClient(channel);
            var grpctask = authenticationServiceClient.GetNextImageDataAsync(new ImageIdentiferData()
            {
                SessionData = new ImageSessionData() { UserName = userName, SessionToken = sessionToken },
                ImageID = imageID
            }, cancellationToken: token).ResponseAsync;
            grpctask.ContinueWith(t =>
            {
                channel.Dispose();
                if (!t.Result.IsSessionValid)
                {
                    _authenticatorProvider.ForceLogout();
                }
            });
            return grpctask.ContinueWith((t) => (t.Result.SerializedImage.ToByteArray(), t.Result.Description, t.Result.ImageID));
        }

        public Task RemoveImageDataAsync(string userName, string sessionToken, string imageID, CancellationToken token)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5105", new GrpcChannelOptions() { MaxRetryAttempts = 1 });
            var authenticationServiceClient = new ImageDataServiceClient(channel);
            var grpctask = authenticationServiceClient.RemoveImageDataAsync(new ImageIdentiferData()
            {
                SessionData = new ImageSessionData() { UserName = userName, SessionToken = sessionToken },
                ImageID = imageID
            }, cancellationToken: token).ResponseAsync;
            grpctask.ContinueWith(t =>
            {
                channel.Dispose();
                if (!t.Result.IsSessionValid)
                {
                    _authenticatorProvider.ForceLogout();
                }
            });
            return grpctask;
        }

        public Task<string> SaveImageDataAsync(string userName, string sessionToken, byte[] imageData, string description, CancellationToken token)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5105", new GrpcChannelOptions() { MaxRetryAttempts = 1 });
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
                if (!t.Result.IsSessionValid)
                {
                    _authenticatorProvider.ForceLogout();
                }
            });
            return grpctask.ContinueWith((t) => t.Result.ImageID);
        }
    }
}