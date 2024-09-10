using System.Globalization;
using System.Text;
using TransferBatch.Domain.Entities;
using TransferBatch.Services.Interfaces;

namespace TransferBatch.Services
{
    public class FileService : IFileService
    {
        //<Account_ID>,<Transfer_ID>,<Total_Transfer_Amount>
        public List<Transfer> ReadLargeFile(string filePath)
        {
            var bufferSize = 1024 * 1024;
            var buffer = new byte[bufferSize];
            var transfers = new List<Transfer>(10000); 

            using (var fs = File.OpenRead(filePath))
            {
                var bytesBuffered = 0;
                var bytesConsumed = 0;

                while (true)
                {                    
                    var bytesRead = fs.Read(buffer, bytesBuffered, buffer.Length - bytesBuffered);
                    if (bytesRead == 0) break;
                    bytesBuffered += bytesRead;

                    int newlinePosition;
                    do
                    {                        
                        newlinePosition = Array.IndexOf(buffer, (byte)'\n', bytesConsumed, bytesBuffered - bytesConsumed);

                        if (newlinePosition >= 0)
                        {
                            
                            var lineLength = newlinePosition - bytesConsumed;
                            var line = new Span<byte>(buffer, bytesConsumed, lineLength);
                            bytesConsumed += lineLength + 1; 
                            
                            ProcessLine(line, transfers);
                        }

                    } while (newlinePosition >= 0);

                    // Realinhar o buffer para processar dados restantes
                    Array.Copy(buffer, bytesConsumed, buffer, 0, bytesBuffered - bytesConsumed);
                    bytesBuffered -= bytesConsumed;
                    bytesConsumed = 0;
                }
                
                if (bytesBuffered > 0)
                {
                    var remainingLine = new Span<byte>(buffer, 0, bytesBuffered);
                    ProcessLine(remainingLine, transfers);
                }
            }

            return transfers;
        }        
        private void ProcessLine(ReadOnlySpan<byte> line, List<Transfer> transfers)
        {            
            var firstCommaPos = line.IndexOf((byte)',');
            var accountId = Encoding.UTF8.GetString(line.Slice(0, firstCommaPos));
            
            var remainingSpan = line.Slice(firstCommaPos + 1);
            var secondCommaPos = remainingSpan.IndexOf((byte)',');
            var transferId = Encoding.UTF8.GetString(remainingSpan.Slice(0, secondCommaPos));
            
            var totalTransferAmountSpan = remainingSpan.Slice(secondCommaPos + 1);
            var totalTransferAmount = ParseDecimal(totalTransferAmountSpan);
            
            transfers.Add(new Transfer(accountId, transferId, totalTransferAmount));
        }
        
        private static decimal ParseDecimal(ReadOnlySpan<byte> span)
        {
            return decimal.Parse(Encoding.UTF8.GetString(span), CultureInfo.InvariantCulture);
        }
    }
}
