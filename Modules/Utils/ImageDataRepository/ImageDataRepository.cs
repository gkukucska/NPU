using NPU.Utils.EncriptionServices;
using System.Diagnostics.CodeAnalysis;

namespace NPU.Utils.ImageDataRepository
{
    public class ImageDataRepository
    {
        private static readonly string _imageDictionary = "ImageData";
        public async Task<string> SaveImage(string userName, byte[] imagedata, CancellationToken cancellationToken)
        {
            MemoryStream stream = new(imagedata);
            await FileIOHelpers.FileIOHelpers.Save(stream, GetUserSpecificDirectory(userName) + "/" + imagedata.GetHashCode() + ".dat", cancellationToken);
            return userName.EncryptToStoredString() + "/" + imagedata.GetHashCode();
        }

        public async Task SaveImageDescription(string userName, string imageID, string description, CancellationToken cancellationToken)
        {
            var imageDescriptionFile = GetImageDescriptionFile(userName);
            await FileIOHelpers.FileIOHelpers.Append(imageID + ";" + description, imageDescriptionFile, cancellationToken);
        }

        public Task<byte[]> GetImageFromID(string username, string imageID, CancellationToken cancellationToken)
        {
            var filename = Directory.GetFiles(GetUserSpecificDirectory(username)).First(x => x.Equals(imageID + ".dat"));
            return FileIOHelpers.FileIOHelpers.LoadBytes(filename, cancellationToken);
        }

        public Task<byte[]> GetNextImageFromID(string username, string imageID, CancellationToken cancellationToken)
        {
            var files = Directory.GetFiles(GetUserSpecificDirectory(username));
            var filename = files.First(x => x.Equals(imageID + ".dat"));
            var index = files.ToList().IndexOf(filename) + 1;
            if (index >= files.Count())
            {
                return null;
            }
            return FileIOHelpers.FileIOHelpers.LoadBytes(files.ToList()[index], cancellationToken);
        }

        public async Task<string> GetImageDescriptionFromId(string userName, string imageID, CancellationToken cancellationToken)
        {
            var data = (await FileIOHelpers.FileIOHelpers.Load(GetImageDescriptionFile(userName), cancellationToken))
                                .Select(x => new KeyValuePair<string, string>(x.Split(";")[0], x.Split(";")[1]));
            return data.FirstOrDefault(x => x.Key.Equals(imageID)).Value;
        }

        public async Task<string> GetNextImageDescriptionFromId(string userName, string imageID, CancellationToken cancellationToken)
        {
            var data = (await FileIOHelpers.FileIOHelpers.Load(GetImageDescriptionFile(userName), cancellationToken))
                                .Select(x => new KeyValuePair<string, string>(x.Split(";")[0], x.Split(";")[1]));
            var kvpair = data.FirstOrDefault(x => x.Key.Equals(imageID));
            var index = data.ToList().IndexOf(kvpair) + 1;
            if (index >= data.Count())
            {
                return String.Empty;
            }
            return data.ToList()[index].Value;
        }

        public Task<byte[]> GetFirstImage(string userName, CancellationToken cancellationToken)
        {
            var filename = Directory.GetFiles(GetUserSpecificDirectory(userName)).First();
            return FileIOHelpers.FileIOHelpers.LoadBytes(filename, cancellationToken);
        }

        public async Task<string> GetFirstImageDescription(string userName, CancellationToken cancellationToken)
        {
            var imageDescriptionFile = GetImageDescriptionFile(userName);
            var descriptions = await FileIOHelpers.FileIOHelpers.Load(imageDescriptionFile, cancellationToken);
            return descriptions.First();
        }

        public async Task RemoveImage(string userName, string imageID, CancellationToken cancellationToken)
        {
            var filename = Directory.GetFiles(GetUserSpecificDirectory(userName)).First(x => x.Equals(imageID + ".dat"));
            await FileIOHelpers.FileIOHelpers.RemoveFile(filename, cancellationToken);
        }


        private static string GetUserSpecificDirectory(string userName)
        {
            return _imageDictionary + "/" + userName.EncryptToStoredString();
        }
        private static string GetImageDescriptionFile(string userName)
        {
            return GetUserSpecificDirectory(userName) + "/ImageDescriptions.dat";
        }
    }
}