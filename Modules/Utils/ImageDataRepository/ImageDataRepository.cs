using NPU.Utils.EncriptionServices;
using NPU.Utils.FileIOHelpers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace NPU.Utils.ImageDataRepository
{
    public class ImageDataRepository
    {
        private static readonly string _imageDictionary = "ImageData";
        public async Task<string> SaveImage(string userName, byte[] imagedata, CancellationToken cancellationToken)
        {
            MemoryStream stream = new(imagedata);
            await FileIOHelpers.FileIOHelpers.Save(stream, GetUserSpecificDirectory(userName) + "\\" + GetUniqueGUID(imagedata) + ".dat", cancellationToken);
            return userName.EncryptToStoredString() + "\\" + GetUniqueGUID(imagedata);
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
            DirectoryInfo info = new DirectoryInfo(GetUserSpecificDirectory(username));
            FileInfo[] files = info.GetFiles().OrderBy(p => p.CreationTime).Where(x=>x.FullName!= GetUserSpecificDirectory(username) + "\\ImageDescriptions.dat").ToArray();
            foreach (FileInfo file in files)
            {
                // DO Something...
            }
            var filename = files.FirstOrDefault(x => x.Name.Equals(imageID+".dat"));
            if (filename == null)
            {
                return Task.FromResult(new byte[0]);
            }
            var index = files.ToList().IndexOf(filename) + 1;
            if (index >= files.Count())
            {
                return Task.FromResult(new byte[0]);
            }
            return FileIOHelpers.FileIOHelpers.LoadBytes(files[index].FullName, cancellationToken);
        }

        public async Task<string> GetImageDescription(string userName, string imageID, CancellationToken cancellationToken)
        {
            var data = (await FileIOHelpers.FileIOHelpers.Load(GetImageDescriptionFile(userName), cancellationToken))
                                .Select(x => new KeyValuePair<string, string>(x.Split(";")[0], x.Split(";")[1]));
            
            var kvpair=data.FirstOrDefault(x => x.Key.EndsWith(imageID));
            return kvpair.Value;
        }

        public Task<byte[]> GetFirstImage(string userName, CancellationToken cancellationToken)
        {
            var filename = Directory.GetFiles(GetUserSpecificDirectory(userName)).FirstOrDefault();
            if (filename == null)
            {
                return Task.FromResult(new byte[0]);
            }
            return FileIOHelpers.FileIOHelpers.LoadBytes(filename, cancellationToken);
        }

        public Task<string> GetImageId(byte[] imageData, CancellationToken cancellationToken)
        {
            return Task.FromResult(GetUniqueGUID(imageData).ToString());
        }

        public async Task<string> GetFirstImageDescription(string userName, CancellationToken cancellationToken)
        {
            var imageDescriptionFile = GetImageDescriptionFile(userName);
            var descriptions = await FileIOHelpers.FileIOHelpers.Load(imageDescriptionFile, cancellationToken);

            return descriptions.FirstOrDefault()?.Split(';')[1];
        }

        public async Task RemoveImage(string userName, string imageID, CancellationToken cancellationToken)
        {
            var filename = Directory.GetFiles(GetUserSpecificDirectory(userName)).First(x => x.Equals(imageID + ".dat"));
            await FileIOHelpers.FileIOHelpers.RemoveFile(filename, cancellationToken);
        }


        private static string GetUserSpecificDirectory(string userName)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), _imageDictionary + "\\" + userName.EncryptToStoredString());
        }
        private static string GetImageDescriptionFile(string userName)
        {
            return GetUserSpecificDirectory(userName) + "\\ImageDescriptions.dat";
        }

        private static string GetUniqueGUID(byte[] imageData)
        {
            using (var md5 = MD5.Create())
            {
                md5.TransformFinalBlock(imageData, 0, imageData.Length);
                return string.Concat(md5.Hash.Select(x => x.ToString("X2")));
            }
        }
    }
}