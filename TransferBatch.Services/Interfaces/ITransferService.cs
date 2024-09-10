using TransferBatch.Domain.DataTransfer;
using TransferBatch.Domain.Entities;

namespace TransferBatch.Services.Interfaces
{
    public interface ITransferService
    {
        List<CommissionDTO> CalculateCommissions(List<Transfer> transfers);
    }
}
