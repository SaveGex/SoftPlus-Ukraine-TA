using Domain.Models;

namespace Domain.Interfaces
{
    public interface IAssetsRepository
    {
        Task<Icon> SaveAssetAsync(Stream fileStream, string extension);
        void DeleteIconRecord(Icon icon);
        void DeletePhysicalFile(string fileName);
        string GetAssetUrl(string fileName);
    }
}
