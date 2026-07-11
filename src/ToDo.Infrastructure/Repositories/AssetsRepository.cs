using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DB;
using Microsoft.AspNetCore.Hosting;

namespace Infrastructure.Repositories
{
    internal class AssetsRepository : IAssetsRepository
    {
        private readonly ToDoDBContext _dbContext;
        private readonly string _storageFolder;
        private const string WebVirtualPath = "Assets/Icons";

        public AssetsRepository(ToDoDBContext context, IWebHostEnvironment environment)
        {
            _storageFolder = Path.Combine(environment.WebRootPath, WebVirtualPath);
            _dbContext = context;

            if (!Directory.Exists(_storageFolder))
            {
                Directory.CreateDirectory(_storageFolder);
            }
        }

        public async Task<Icon> SaveAssetAsync(Stream fileStream, string extension)
        {
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(_storageFolder, uniqueFileName);

            using (var destinationStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await fileStream.CopyToAsync(destinationStream);
            }

            var icon = new Icon { Name = uniqueFileName };
            await _dbContext.Icons.AddAsync(icon);

            return icon;
        }

        public void DeleteIconRecord(Icon icon)
        {
            if (icon == null) return;

            _dbContext.Icons.Remove(icon);
        }

        public void DeletePhysicalFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return;

            var fullPath = Path.Combine(_storageFolder, fileName);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public string GetAssetUrl(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return string.Empty;

            return $"{WebVirtualPath}/{fileName}";
        }
    }
}
