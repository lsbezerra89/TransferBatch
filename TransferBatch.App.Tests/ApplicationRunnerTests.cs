using Moq;
using TransferBatch.Domain.DataTransfer;
using TransferBatch.Domain.Entities;
using TransferBatch.Services.Interfaces;
using Xunit;

namespace TransferBatch.App.Tests
{
    public class ApplicationRunnerTests
    {
        private readonly ApplicationRunner _runner;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly Mock<ITransferService> _transferServiceMock;

        public ApplicationRunnerTests()
        {
            _fileServiceMock = new Mock<IFileService>();
            _transferServiceMock = new Mock<ITransferService>();

            _runner = new ApplicationRunner(_fileServiceMock.Object, _transferServiceMock.Object);
        }

        [Fact]
        public async Task RunAsync_WithValidData_PrintsCorrectCommissions()
        {
            // Arrange
            var filePath = "valid.csv";
            var transfers = new List<Transfer>
        {
            new Transfer("A10", "T1000", 100.00m),
            new Transfer("A11", "T1001", 200.00m)
        };
            var commissions = new List<CommissionDTO>
        {
            new CommissionDTO { AccountId = "A10", TotalCommission = 10.00m },
            new CommissionDTO { AccountId = "A11", TotalCommission = 20.00m }
        };

            _fileServiceMock.Setup(f => f.ReadLargeFile(filePath)).Returns(transfers);
            _transferServiceMock.Setup(t => t.CalculateCommissions(transfers)).Returns(commissions);

            // Act
            // Use a StringWriter to capture Console output
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                await _runner.RunAsync(new[] { filePath });
                var output = sw.ToString().Trim();

                // Assert
                Assert.Contains("A10,10.00", output);
                Assert.Contains("A11,20.00", output);
            }
        }

    }
}
