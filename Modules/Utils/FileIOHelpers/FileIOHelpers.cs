namespace NPU.Utils.FileIOHelpers
{
    public static class FileIOHelpers
    {

        public static async Task Save(IEnumerable<string> data,string filename)
        {
            using (StreamWriter file = new StreamWriter(GetPath(filename), append: false))
            {
                foreach (var item in data)
                {
                    if (string.IsNullOrEmpty(item) || string.IsNullOrWhiteSpace(item)) continue;
                    await file.WriteLineAsync(item);
                }
            }
        }

        public static async Task Save(Stream stream,string filename)
        {
            using (var filestream=new FileStream(GetPath(filename),FileMode.OpenOrCreate))
            {
                stream.CopyTo(filestream);
                await filestream.FlushAsync();
            }
        }

        public static async Task Append(string data, string filename)
        {
            await File.AppendAllLinesAsync(filename, Enumerable.Repeat(data, 1));
        }

        public static async Task Save(string data, string filename)
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
            await Save(list,filename);
        }

        public static async Task<IEnumerable<string>> Load(string filename)
        {
            string pathToLoad = GetPath(filename);
            if (File.Exists(pathToLoad))
            {
                return await File.ReadAllLinesAsync(pathToLoad);
            }
            return await Task.FromResult(Enumerable.Empty<string>());
        }

        private static string GetPath(string filename)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), filename);
        }
    }
}