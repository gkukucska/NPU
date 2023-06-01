using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientInterfaces
{
    public interface IImageDataClient
    {
        Task<Tuple<byte[], string>> GetFirstImageAsync(string username, string sessiontoken, CancellationToken token);
        Task<Tuple<byte[], string>> GetImageDataAsync(string userName, string sessionToken, string imageID, CancellationToken token);
        Task<Tuple<byte[], string>> GetNextImageDataAsync(string userName, string sessionToken, string imageID, CancellationToken token);
        Task RemoveImageDataAsync(string userName, string sessionToken, string imageID, CancellationToken token);
        Task<string> SaveImageDataAsync(string userName, string sessionToken, byte[] imageData, CancellationToken token);
    }

    public static class ImageDataClientExtensions
    {
        public static Task<Tuple<byte[], string>> GetFirstImageAsync(this IImageDataClient client, string username, string sessiontoken)
            => client.GetFirstImageAsync(username, sessiontoken, CancellationToken.None);
        public static Tuple<byte[], string> GetFirstImage(this IImageDataClient client, string username, string sessiontoken)
            => client.GetFirstImageAsync(username, sessiontoken, CancellationToken.None).Result;
        public static Task<Tuple<byte[], string>> GetImageDataAsync(this IImageDataClient client, string username, string sessiontoken, string imageID)
            => client.GetImageDataAsync(username, sessiontoken, imageID, CancellationToken.None);
        public static Tuple<byte[], string> GetImageData(this IImageDataClient client, string username, string sessiontoken, string imageID)
            => client.GetImageDataAsync(username, sessiontoken, imageID, CancellationToken.None).Result;
        public static Task<Tuple<byte[], string>> GetNextImageDataAsync(this IImageDataClient client, string username, string sessiontoken, string imageID)
            => client.GetNextImageDataAsync(username, sessiontoken, imageID, CancellationToken.None);
        public static Tuple<byte[], string> GetNextImageData(this IImageDataClient client, string username, string sessiontoken, string imageID)
            => client.GetNextImageDataAsync(username, sessiontoken, imageID, CancellationToken.None).Result;
        public static Task RemoveImageDataAsync(this IImageDataClient client, string username, string sessiontoken, string imageID)
            => client.RemoveImageDataAsync(username, sessiontoken, imageID, CancellationToken.None);
        public static void RemoveImageDataA(this IImageDataClient client, string username, string sessiontoken, string imageID)
            => client.RemoveImageDataAsync(username, sessiontoken, imageID, CancellationToken.None).Wait();
        public static Task< string> SaveImageDataAsync(this IImageDataClient client, string username, string sessiontoken, byte[] imageData)
            => client.SaveImageDataAsync(username, sessiontoken, imageData, CancellationToken.None);
        public static string SaveImageData(this IImageDataClient client, string username, string sessiontoken, byte[] imageData)
            => client.SaveImageDataAsync(username, sessiontoken, imageData, CancellationToken.None).Result;
    }

}
