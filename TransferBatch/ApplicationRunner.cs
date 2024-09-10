using TransferBatch.Domain.DataTransfer;
using TransferBatch.Services.Interfaces;

namespace TransferBatch.App
{
    public class ApplicationRunner
    {
        private readonly IFileService _fileService;
        private readonly ITransferService _transferService;

        public ApplicationRunner(IFileService fileService, ITransferService transferService)
        {
            _fileService = fileService;
            _transferService = transferService;
        }

        public async Task RunAsync(string[] args)
        {
            var filePath = GetFilePath(args);
            if (filePath == null)
            {
                Console.WriteLine("Invalid file path.");
                return;
            }

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File {filePath} does not exist.");
                return;
            }

            try
            {
                var transfers = _fileService.ReadLargeFile(filePath);
                var commissions = _transferService.CalculateCommissions(transfers);

                PrintCommissions(commissions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.ReadKey();
        }

        private static string GetFilePath(string[] args)
        {
            if (args.Length > 0)
            {
                return args[0];
            }
            return null;
        }

        private static void PrintCommissions(IEnumerable<CommissionDTO> commissions)
        {
            foreach (var commission in commissions)
            {
                Console.WriteLine($"{commission.AccountId},{commission.TotalCommission:F2}");
            }
        }
    }
}