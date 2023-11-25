namespace OnlineWallet.Models
{
    public class ServiceUser
    {
        public int Id { get; set; }
        public int Balance { get; set; }
        public int ServiceProviderId { get; set; }
        public ServiceProvider? ServiceProvider { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AccountNumber { get; set; }
    }
}
