namespace TransferBatch.Services.Tests
{
    using System.IO;
    using TransferBatch.Services.Interfaces;
    using Xunit;

    public class FileServiceTests
    {
        private readonly IFileService _fileService;

        public FileServiceTests()
        {
            _fileService = new FileService();
        }

        [Fact]
        public void ReadLargeFile_WithValidData_ReturnsCorrectTransfers()
        {
            // Arrange
            var fileContent = "A10,T1000,100.00\nA11,T1001,200.00\n";
            var filePath = "test.csv";

            // Write the test content to a temporary file
            File.WriteAllText(filePath, fileContent);

            // Act
            var transfers = _fileService.ReadLargeFile(filePath);

            // Assert
            Assert.Equal(2, transfers.Count);
            Assert.Contains(transfers, t => t.AccountId == "A10" && t.TransferId == "T1000" && t.Amount == 100.00m);
            Assert.Contains(transfers, t => t.AccountId == "A11" && t.TransferId == "T1001" && t.Amount == 200.00m);

            // Clean up
            File.Delete(filePath);
        }

        [Fact]
        public void ReadLargeFile_WithEmptyFile_ReturnsEmptyList()
        {
            // Arrange
            var filePath = "empty.csv";
            File.WriteAllText(filePath, string.Empty);

            // Act
            var transfers = _fileService.ReadLargeFile(filePath);

            // Assert
            Assert.Empty(transfers);

            // Clean up
            File.Delete(filePath);
        }

        [Fact]
        public void ReadLargeFile_WithSingleLine_ReturnsSingleTransfer()
        {
            // Arrange
            var fileContent = "A12,T1002,150.00\n";
            var filePath = "single.csv";
            File.WriteAllText(filePath, fileContent);

            // Act
            var transfers = _fileService.ReadLargeFile(filePath);

            // Assert
            Assert.Single(transfers);
            Assert.Equal("A12", transfers[0].AccountId);
            Assert.Equal("T1002", transfers[0].TransferId);
            Assert.Equal(150.00m, transfers[0].Amount);

            // Clean up
            File.Delete(filePath);
        }

        [Fact]
        public void ReadLargeFile_WithMultipleLinesAndNoNewlinesAtEnd_ReturnsAllTransfers()
        {
            // Arrange
            var fileContent = "A13,T1003,300.00\nA14,T1004,400.00";
            var filePath = "no_newline_at_end.csv";
            File.WriteAllText(filePath, fileContent);

            // Act
            var transfers = _fileService.ReadLargeFile(filePath);

            // Assert
            Assert.Equal(2, transfers.Count);
            Assert.Contains(transfers, t => t.AccountId == "A13" && t.TransferId == "T1003" && t.Amount == 300.00m);
            Assert.Contains(transfers, t => t.AccountId == "A14" && t.TransferId == "T1004" && t.Amount == 400.00m);

            // Clean up
            File.Delete(filePath);
        }
    }

}
