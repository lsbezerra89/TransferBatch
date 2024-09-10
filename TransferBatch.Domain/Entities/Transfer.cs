namespace TransferBatch.Domain.Entities
{
    public record Transfer(
        string AccountId,
        string TransferId,
        decimal Amount
     )
    {
        public Transfer() : this(default!, default!, default) { }
    }
}

