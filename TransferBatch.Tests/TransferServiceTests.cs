using Microsoft.Extensions.DependencyInjection;
using TransferBatch.Domain.Entities;
using TransferBatch.Services.Interfaces;

namespace TransferBatch.Services.Tests
{
    public class TransferServiceTests
    {
        private readonly ITransferService _transferService;

        public TransferServiceTests()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            _transferService = serviceProvider.GetRequiredService<ITransferService>();
        }

        private void ConfigureServices(IServiceCollection services)
        {            
            services.AddTransient<ITransferService, TransferService>();                                                                         
        }

        [Fact]
        public void CalculateCommissions_WithNoTransfers_ReturnsEmptyList()
        {
            // Arrange

            var transfers = new List<Transfer>();

            // Act
            var result = _transferService.CalculateCommissions(transfers);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void CalculateCommissions_WithOneTransfer_ReturnsNoCommission()
        {
            // Arrange            
            var transfers = new List<Transfer> { new Transfer() { AccountId = "A10", TransferId = "T1000", Amount = 100.00m } };

            // Act
            var result = _transferService.CalculateCommissions(transfers);

            // Assert
            var commission = Assert.Single(result);
            Assert.Equal("A10", commission.AccountId);
            Assert.Equal(0.00m, commission.TotalCommission);
        }

        [Fact]
        public void CalculateCommissions_WithMultipleTransfers_ExcludesHighestTransfer()
        {
            // Arrange            
            var transfers = new List<Transfer>{
                new Transfer { AccountId = "A10", TransferId = "T1000", Amount = 100.00m },        
                new Transfer { AccountId = "A10", TransferId = "T1001", Amount = 200.00m },
                new Transfer { AccountId = "A10", TransferId = "T1002", Amount = 300.00m }};

            // Act
            var result = _transferService.CalculateCommissions(transfers);

            // Assert
            var commission = Assert.Single(result);
            Assert.Equal("A10", commission.AccountId);
            Assert.Equal(30.00m, commission.TotalCommission); 
        }

        [Fact]
        public void CalculateCommissions_WithMultipleAccounts_ReturnsCommissionsForEachAccount()
        {
            // Arrange            
            var transfers = new List<Transfer>{new Transfer { AccountId = "A10", TransferId = "T1000", Amount = 100.00m },
                                               new Transfer { AccountId = "A10", TransferId = "T1001", Amount = 200.00m },
                                               new Transfer { AccountId = "A11", TransferId = "T1002", Amount = 150.00m },
                                               new Transfer { AccountId = "A11", TransferId = "T1003", Amount = 250.00m }                        };

            // Act
            var result = _transferService.CalculateCommissions(transfers);

            // Assert
            var a10Commission = result.Single(c => c.AccountId == "A10");
            Assert.Equal(30.00m, a10Commission.TotalCommission); 

            var a11Commission = result.Single(c => c.AccountId == "A11");
            Assert.Equal(15.00m, a11Commission.TotalCommission); 
        }
    }
}