using TransferBatch.Domain.DataTransfer;
using TransferBatch.Domain.Entities;
using TransferBatch.Services.Interfaces;

namespace TransferBatch.Services
{
    public class TransferService : ITransferService
    {
        const decimal commission = 0.1m;
        public List<CommissionDTO> CalculateCommissions(List<Transfer> transfers)
        {
            var result = new List<CommissionDTO>();
            
            var transfersByAccount = transfers.GroupBy(t => t.AccountId);
            
            foreach (var transferGroup in transfersByAccount)
            {
                var accountId = transferGroup.Key;
                var transfersList = transferGroup.ToList();
                
                var highestTransferAmount = transfersList.Max(t => t.Amount);
                
                decimal totalByAccount = 0;
                bool highestTransferExcluded = false;

                foreach (var transfer in transfersList)
                {
                    if (!highestTransferExcluded && transfer.Amount == highestTransferAmount)
                    {
                        // Excluir apenas a primeira transferência encontrada com o valor máximo
                        highestTransferExcluded = true;
                        continue;
                    }
                    totalByAccount += transfer.Amount;
                }
                
                if (transfersList.Count == 1)
                {
                    totalByAccount = highestTransferAmount;
                }
                
                var totalCommission = totalByAccount * commission;
                result.Add(new CommissionDTO { AccountId = accountId, TotalCommission = totalCommission });
            }

            return result;
        }

    }
}
