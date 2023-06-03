using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientInterfaces
{
    public interface IImageDataClient
    {
        Task<(byte[] ImageData, string Description, string ImageID)> GetFirstImageAsync(string username, string sessiontoken, CancellationToken token);
        Task<(byte[] ImageData, string Description, string ImageID)> GetImageDataAsync(string userName, string sessionToken, string imageID, CancellationToken token);
        Task<(byte[] ImageData, string Description, string ImageID)> GetNextImageDataAsync(string userName, string sessionToken, string imageID, CancellationToken token);
        Task RemoveImageDataAsync(string userName, string sessionToken, string imageID, CancellationToken token);
        Task<string> SaveImageDataAsync(string userName, string sessionToken, byte[] imageData, string description, CancellationToken token);
    }

    public static class ImageDataClientExtensions
    {
        public static Task<(byte[] ImageData, string Description, string ImageID)> GetFirstImageAsync(this IImageDataClient client, string username, string sessiontoken)
            => client.GetFirstImageAsync(username, sessiontoken, CancellationToken.None);
        public static (byte[] ImageData, string Description, string ImageID) GetFirstImage(this IImageDataClient client, string username, string sessiontoken)
            => client.GetFirstImageAsync(username, sessiontoken, CancellationToken.None).Result;
        public static Task<(byte[] ImageData, string Description, string ImageID)> GetImageDataAsync(this IImageDataClient client, string username, string sessiontoken, string imageID)
            => client.GetImageDataAsync(username, sessiontoken, imageID, CancellationToken.None);
        public static (byte[] ImageData, string Description, string ImageID) GetImageData(this IImageDataClient client, string username, string sessiontoken, string imageID)
            => client.GetImageDataAsync(username, sessiontoken, imageID, CancellationToken.None).Result;
        public static Task<(byte[] ImageData, string Description, string ImageID)> GetNextImageDataAsync(this IImageDataClient client, string username, string sessiontoken, string imageID)
            => client.GetNextImageDataAsync(username, sessiontoken, imageID, CancellationToken.None);
        public static (byte[] ImageData, string Description, string ImageID) GetNextImageData(this IImageDataClient client, string username, string sessiontoken, string imageID)
            => client.GetNextImageDataAsync(username, sessiontoken, imageID, CancellationToken.None).Result;
        public static Task RemoveImageDataAsync(this IImageDataClient client, string username, string sessiontoken, string imageID)
            => client.RemoveImageDataAsync(username, sessiontoken, imageID, CancellationToken.None);
        public static void RemoveImageDataA(this IImageDataClient client, string username, string sessiontoken, string imageID)
            => client.RemoveImageDataAsync(username, sessiontoken, imageID, CancellationToken.None).Wait();
        public static Task<string> SaveImageDataAsync(this IImageDataClient client, string username, string sessiontoken, byte[] imageData, string description)
            => client.SaveImageDataAsync(username, sessiontoken, imageData, description, CancellationToken.None);
        public static string SaveImageData(this IImageDataClient client, string username, string sessiontoken, byte[] imageData, string description)
            => client.SaveImageDataAsync(username, sessiontoken, imageData, description, CancellationToken.None).Result;
    }

}
