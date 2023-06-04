using Microsoft.Extensions.Logging;

namespace NPU.Utils.FileIOHelpers
{
    public static class FileIOHelpers
    {
        public static async Task Save(Stream stream, string filename, CancellationToken token)
        {
            var directoryName = Path.GetDirectoryName(GetPath(filename)) ?? string.Empty;
            if (!Directory.Exists(directoryName))
            {
                //this should be done with a logger
                await Console.Out.WriteLineAsync($"{directoryName} does not exists, creating empty directory");
                Directory.CreateDirectory(directoryName);
            }
            using (var filestream = new FileStream(GetPath(filename), FileMode.OpenOrCreate))
            {
                stream.CopyTo(filestream);
                await filestream.FlushAsync(token);
            }
        }

        public static async Task Append(string data, string filename, CancellationToken token)
        {
            await File.AppendAllLinesAsync(filename, Enumerable.Repeat(data, 1), token);
        }

        public static async Task<IEnumerable<string>> LoadLines(string filename, CancellationToken token)
        {
            string pathToLoad = GetPath(filename);
            if (File.Exists(pathToLoad))
            {
                return await File.ReadAllLinesAsync(pathToLoad, token);
            }
            //this should be done with a logger
            await Console.Out.WriteLineAsync($"{pathToLoad} does not exists");
            return await Task.FromResult(Enumerable.Empty<string>());
        }

        public static Task<byte[]> LoadBytes(string filename, CancellationToken token)
        {
            return File.ReadAllBytesAsync(filename, token);
        }

        public static Task RemoveFile(string filename, CancellationToken cancellationToken)
        {
            File.Delete(filename);
            return Task.CompletedTask;
        }

        private static string GetPath(string filename)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), filename);
        }
    }
}