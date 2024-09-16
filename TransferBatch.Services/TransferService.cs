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
            if (transfers == null || transfers.Count == 0) { return []; }

            var result = new List<CommissionDTO>();
            
            var highestTransferAmount = transfers.Max(t => t.Amount);

            var transfersByAccount = transfers.GroupBy(t => t.AccountId);

            bool highestTransferExcluded = false;

            foreach (var transferGroup in transfersByAccount)
            {
                var accountId = transferGroup.Key;
                var transfersList = transferGroup.ToList();

                decimal totalByAccount = 0;

                foreach (var transfer in transfersList)
                {
                    // Excluir a transação com o maior valor do ficheiro apenas uma vez
                    if (!highestTransferExcluded && transfer.Amount == highestTransferAmount)
                    {
                        highestTransferExcluded = true;
                        continue;
                    }
                    totalByAccount += transfer.Amount;
                }

                if (transfersList.Count == 1)
                {
                    totalByAccount = transfersList.First().Amount;
                }

                var totalCommission = totalByAccount * commission;
                result.Add(new CommissionDTO { AccountId = accountId, TotalCommission = totalCommission });
            }

            return result;
        }



    }
}
