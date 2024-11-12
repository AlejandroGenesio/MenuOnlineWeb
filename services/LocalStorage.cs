
namespace MenuOnlineUdemy.services
{
    public class LocalStorage : IFileStorage
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;

        public LocalStorage(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }
        public Task Delete(string? path, string container)
        {
            if (string.IsNullOrEmpty(path))
            {
                return Task.CompletedTask;
            }

            var fileName = Path.GetFileName(path);
            var folderFile = Path.Combine(env.WebRootPath, container, fileName);

            if (File.Exists(folderFile))
            {
                File.Delete(folderFile);
            }

            return Task.CompletedTask;
        }

        public async Task<string> Storage(string container, IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            string rndValue = Guid.NewGuid().ToString();
            var fileName = $"{file.FileName.Split('.')[0] + '-' + rndValue.Substring(rndValue.Length - 11)}{extension}";
            string folder = Path.Combine(env.WebRootPath, container);

            if (!Directory.Exists(folder)) { }
            {
                Directory.CreateDirectory(folder);
            }

            string path = Path.Combine(folder, fileName);
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                var content = ms.ToArray();
                await File.WriteAllBytesAsync(path, content);
            }

            var url = $"{httpContextAccessor.HttpContext!.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            var urlFile = Path.Combine(url, container, fileName).Replace("\\", "/");

            return urlFile;

        }
    }
}
