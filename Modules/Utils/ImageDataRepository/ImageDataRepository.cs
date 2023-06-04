using Microsoft.Extensions.Logging;
using NPU.Interfaces;
using NPU.Utils.EncriptionServices;
using NPU.Utils.FileIOHelpers;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace NPU.Utils.ImageDataRepository
{
    public class ImageDataRepository : IImageDataRepository
    {
        private static readonly string _imageDictionary = "ImageData";
        private static readonly string _dataFileExtension = ".dat";
        private static readonly string _imageDescriptionFile = "\\ImageDescriptions.dat";

        private ILogger<ImageDataRepository> _logger;

        public ImageDataRepository(ILogger<ImageDataRepository> logger)
        {
            _logger = logger;
        }


        public async Task<string> SaveImage(string userName, byte[] imagedata, CancellationToken cancellationToken)
        {
            using MemoryStream stream = new(imagedata);
            var filename = GetUserSpecificDirectory(userName) + "\\" + GetUniqueGUID(imagedata) + _dataFileExtension;
            _logger.LogDebug($"Saving image data to {filename}");
            await FileIOHelpers.FileIOHelpers.Save(stream, filename, cancellationToken);
            return GetUniqueGUID(imagedata);
        }


        public async Task SaveImageDescription(string userName, string imageID, string description, CancellationToken cancellationToken)
        {
            var imageDescriptionFile = GetImageDescriptionFile(userName);
            _logger.LogDebug($"Saving image description of image {imageID} to {imageDescriptionFile}");
            await FileIOHelpers.FileIOHelpers.Append(imageID + ";" + description, imageDescriptionFile, cancellationToken);
        }


        public Task<byte[]> GetNextImageByID(string username, CancellationToken cancellationToken, string? imageID = null)
        {
            var nextImageId = GetNextImageId(username, imageID);
            if(nextImageId == null)
            {
                _logger.LogInformation($"No next image found, returning empty image data");
                return Task.FromResult(new byte[0]);
            }
            return GetImageByID(username, cancellationToken, nextImageId);
        }


        public Task<byte[]> GetImageByID(string username, CancellationToken cancellationToken, string? imageID = null)
        {
            var files = Directory.GetFiles(GetUserSpecificDirectory(username));
            var filename = Directory.GetFiles(GetUserSpecificDirectory(username)).FirstOrDefault(x => x.EndsWith(Path.GetFileNameWithoutExtension( imageID) + _dataFileExtension));
            _logger.LogDebug($"Loading image data from {filename}");
            if (filename == null)
            {
                _logger.LogWarning($"Cannot find requested file, returning empty image data");
                return Task.FromResult(new byte[0]);
            }
            return FileIOHelpers.FileIOHelpers.LoadBytes(filename, cancellationToken);
        }


        public async Task<string> GetImageDescription(string userName, CancellationToken cancellationToken, string? imageID = null)
        {
            var imageDescriptions = (await FileIOHelpers.FileIOHelpers.LoadLines(GetImageDescriptionFile(userName), cancellationToken))
                                                                      .Select(x => new KeyValuePair<string, string>(x.Split(";")[0], x.Split(";")[1]));

            return imageDescriptions.FirstOrDefault(x => x.Key.Equals(imageID)).Value ?? string.Empty;
        }


        public Task<string> GetImageId(byte[] imageData, CancellationToken cancellationToken)
            => Task.FromResult(GetUniqueGUID(imageData));


        public async Task RemoveImage(string userName, string imageID, CancellationToken cancellationToken)
        {
            var filename = Directory.GetFiles(GetUserSpecificDirectory(userName))
                                    .Select(x=>Path.GetFileNameWithoutExtension(x))
                                    .FirstOrDefault(x => x.Equals(imageID));
            if (filename==null)
            {
                _logger.LogWarning($"Cannot find file associated with image ID {imageID}");
                return;
            }
            await FileIOHelpers.FileIOHelpers.RemoveFile(filename, cancellationToken);
        }


        private string? GetNextImageId(string username, string? imageID = null)
        {
            var info = new DirectoryInfo(GetUserSpecificDirectory(username));
            if (!info.Exists)
            {
                Directory.CreateDirectory(info.FullName);
            }
            var files = info.GetFiles().OrderBy(p => p.CreationTime).Where(x => x.FullName != GetUserSpecificDirectory(username) + _imageDescriptionFile).ToArray();
            if (imageID != null)
            {
                var currentfileinfo = files.FirstOrDefault(x => x.Name.Equals(imageID + _dataFileExtension));
                if (currentfileinfo == null)
                {
                    _logger.LogWarning($"Cannot find file associated with image ID {imageID}");
                    return null;
                }

                var index = files.ToList().IndexOf(currentfileinfo) + 1;
                if (index >= files.Length)
                {
                    _logger.LogInformation($"Image with ID {imageID} is the last image of user {username}");
                    return null;
                }
                return files[index].FullName;
            }
            else
            {
                _logger.LogInformation($"Requested image ID is null, returning the first image ID");
                return Path.GetFileNameWithoutExtension(files.FirstOrDefault()?.FullName);
            }
        }


        private static string GetUniqueGUID(byte[] imageData)
        {
            if ((imageData?.Length ?? 0)==0)
            {
                return string.Empty;
            }
            using var md5 = MD5.Create();
            md5.TransformFinalBlock(imageData, 0, imageData.Length);
            if (md5.Hash == null)
            {
                throw new Exception("Cannot create unique ID for image");
            }
            return string.Concat(md5.Hash.Select(x => x.ToString("X2", CultureInfo.InvariantCulture)));
        }


        private static string GetUserSpecificDirectory(string userName)
            => Path.Combine(Directory.GetCurrentDirectory(), _imageDictionary + "\\" + userName.EncryptToStoredString());


        private static string GetImageDescriptionFile(string userName)
            => GetUserSpecificDirectory(userName) + _imageDescriptionFile;
    }
}