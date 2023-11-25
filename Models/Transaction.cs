namespace OnlineWallet.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Amount { get; set; }
        public string? Type { get; set; }
        public int? SenderUserId { get; set; }
        public User SenderUser { get; set; }

        public int ReceiverUserId { get; set; }
        public User ReceiverUser { get; set; }

    }
}
