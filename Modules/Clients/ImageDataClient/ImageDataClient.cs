using ClientInterfaces;
using Google.Protobuf;
using Grpc.Net.Client;
using NPU.Protocols;
using static NPU.Protocols.ImageDataService;

namespace NPU.Clients.ImageDataClient
{
    public class ImageDataClient:IImageDataClient
    {
        private ImageDataServiceClient _authenticationServiceClient;
        private GrpcChannel _channel;
        public ImageDataClient()
        {
            _channel = GrpcChannel.ForAddress("http://localhost:5105", new GrpcChannelOptions() { MaxRetryAttempts = 1 });
            _authenticationServiceClient = new ImageDataServiceClient(_channel);
        }

        public Task<Tuple<byte[], string>> GetFirstImageAsync(string username, string sessiontoken, CancellationToken token)
            => _authenticationServiceClient.GetFirstImageDataAsync(new ImageSessionData()
            {
                UserName = username,
                SessionToken = sessiontoken,
            }, cancellationToken: token).ResponseAsync.ContinueWith((t) => new Tuple<byte[], string>(t.Result.SerializedImage.ToByteArray(), t.Result.Description));

        public Task<Tuple<byte[], string>> GetImageDataAsync(string userName, string sessionToken, string imageID, CancellationToken token)
            => _authenticationServiceClient.GetImageDataAsync(new ImageIdentiferData()
            {
                SessionData = new ImageSessionData() { UserName = userName, SessionToken = sessionToken },
                ImageID = imageID
            }, cancellationToken: token).ResponseAsync.ContinueWith((t) => new Tuple<byte[], string>(t.Result.SerializedImage.ToByteArray(), t.Result.Description));

        public Task<Tuple<byte[], string>> GetNextImageDataAsync(string userName, string sessionToken, string imageID, CancellationToken token)
            => _authenticationServiceClient.GetNextImageDataAsync(new ImageIdentiferData()
            {
                SessionData = new ImageSessionData() { UserName = userName, SessionToken = sessionToken },
                ImageID = imageID
            }, cancellationToken: token).ResponseAsync.ContinueWith((t) => new Tuple<byte[], string>(t.Result.SerializedImage.ToByteArray(), t.Result.Description));

        public Task RemoveImageDataAsync(string userName, string sessionToken, string imageID, CancellationToken token)
            => _authenticationServiceClient.RemoveImageDataAsync(new ImageIdentiferData()
            {
                SessionData = new ImageSessionData() { UserName = userName, SessionToken = sessionToken },
                ImageID = imageID
            }, cancellationToken: token).ResponseAsync;

        public Task<string> SaveImageDataAsync(string userName, string sessionToken, byte[] imageData, CancellationToken token)
            => _authenticationServiceClient.SaveImageDataAsync(new ImageUploadData()
            {
                SessionData = new ImageSessionData() { UserName = userName, SessionToken = sessionToken },
                SerializedImage = ByteString.CopyFrom(imageData),
            }, cancellationToken: token).ResponseAsync.ContinueWith((t) => t.Result.ImageID);
    }
}