namespace NPU.Utils.FileIOHelpers
{
    public static class FileIOHelpers
    {

        public static async Task Save(IEnumerable<string> data, string filename, CancellationToken token)
        {
            using (StreamWriter file = new StreamWriter(GetPath(filename), append: false))
            {
                foreach (var item in data)
                {
                    if (string.IsNullOrEmpty(item) || string.IsNullOrWhiteSpace(item)) continue;
                    await file.WriteLineAsync(new ReadOnlyMemory<char>(item.ToArray()), cancellationToken: token);
                }
            }
        }

        public static async Task Save(Stream stream, string filename, CancellationToken token)
        {
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

        public static async Task Save(string data, string filename, CancellationToken token)
        {
            List<string> list = new List<string>();
            using (StringReader sr = new StringReader(data))
            {
                string line;
                while ((line = await sr.ReadLineAsync()) != null)
                {
                    list.Add(line);
                }
            }
            list.Add(data);
            await Save(list, filename, token);
        }

        public static async Task<IEnumerable<string>> Load(string filename, CancellationToken token)
        {
            string pathToLoad = GetPath(filename);
            if (File.Exists(pathToLoad))
            {
                return await File.ReadAllLinesAsync(pathToLoad, token);
            }
            return await Task.FromResult(Enumerable.Empty<string>());
        }

        public static Task<byte[]> LoadBytes(string filename, CancellationToken token)
        {
            return File.ReadAllBytesAsync(filename, token);
        }

        private static string GetPath(string filename)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), filename);
        }
    }
}