using TransferBatch.Domain.Entities;

namespace TransferBatch.Services.Interfaces
{
    public interface IFileService
    {
        List<Transfer> ReadLargeFile(string filePath);        
    }
}
