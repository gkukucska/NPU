using ClientInterfaces;
using Google.Protobuf;
using Grpc.Net.Client;
using NPU.Protocols;
using static NPU.Protocols.ImageDataService;
using static System.Net.Mime.MediaTypeNames;

namespace NPU.Clients.ImageDataClient
{
    public class ImageDataClient : IImageDataClient
    {

        public Task<Tuple<byte[], string>> GetFirstImageAsync(string username, string sessiontoken, CancellationToken token)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5105", new GrpcChannelOptions() { MaxRetryAttempts = 1 });
            var authenticationServiceClient = new ImageDataServiceClient(channel);
            var grpctask = authenticationServiceClient.GetFirstImageDataAsync(new ImageSessionData()
            {
                UserName = username,
                SessionToken = sessiontoken,
            }, cancellationToken: token).ResponseAsync;
            grpctask.ContinueWith(t => { channel.Dispose(); });
            return grpctask.ContinueWith((t) => new Tuple<byte[], string>(t.Result.SerializedImage.ToByteArray(), t.Result.Description));
        }

        public Task<Tuple<byte[], string>> GetImageDataAsync(string userName, string sessionToken, string imageID, CancellationToken token)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5105", new GrpcChannelOptions() { MaxRetryAttempts = 1 });
            var authenticationServiceClient = new ImageDataServiceClient(channel);
            var grpctask = authenticationServiceClient.GetImageDataAsync(new ImageIdentiferData()
            {
                SessionData = new ImageSessionData() { UserName = userName, SessionToken = sessionToken },
                ImageID = imageID
            }, cancellationToken: token).ResponseAsync;
            grpctask.ContinueWith(t => { channel.Dispose(); });
            return grpctask.ContinueWith((t) => new Tuple<byte[], string>(t.Result.SerializedImage.ToByteArray(), t.Result.Description));
        }

        public Task<Tuple<byte[], string>> GetNextImageDataAsync(string userName, string sessionToken, string imageID, CancellationToken token)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5105", new GrpcChannelOptions() { MaxRetryAttempts = 1 });
            var authenticationServiceClient = new ImageDataServiceClient(channel);
            var grpctask = authenticationServiceClient.GetNextImageDataAsync(new ImageIdentiferData()
            {
                SessionData = new ImageSessionData() { UserName = userName, SessionToken = sessionToken },
                ImageID = imageID
            }, cancellationToken: token).ResponseAsync;
            grpctask.ContinueWith(t => { channel.Dispose(); });
            return grpctask.ContinueWith((t) => new Tuple<byte[], string>(t.Result.SerializedImage.ToByteArray(), t.Result.Description));
        }

        public Task RemoveImageDataAsync(string userName, string sessionToken, string imageID, CancellationToken token)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5105", new GrpcChannelOptions() { MaxRetryAttempts = 1 });
            var authenticationServiceClient = new ImageDataServiceClient(channel);
            var grpctask= authenticationServiceClient.RemoveImageDataAsync(new ImageIdentiferData()
            {
                SessionData = new ImageSessionData() { UserName = userName, SessionToken = sessionToken },
                ImageID = imageID
            }, cancellationToken: token).ResponseAsync;
            return grpctask;
        }

        public Task<string> SaveImageDataAsync(string userName, string sessionToken, byte[] imageData,string description, CancellationToken token)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5105", new GrpcChannelOptions() { MaxRetryAttempts = 1 });
            var authenticationServiceClient = new ImageDataServiceClient(channel);
            var grpctask = authenticationServiceClient.SaveImageDataAsync(new ImageUploadData()
            {
                SessionData = new ImageSessionData() { UserName = userName, SessionToken = sessionToken },
                SerializedImage = ByteString.CopyFrom(imageData),
                Description=description
            }, cancellationToken: token).ResponseAsync;
            grpctask.ContinueWith(t => { channel.Dispose(); });
            return grpctask.ContinueWith((t) => t.Result.ImageID);
        }
    }
}