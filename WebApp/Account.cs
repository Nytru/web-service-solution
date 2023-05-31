namespace WebApp
{
    public class Account
    {
        public string UserName { get; set; }
        public string ExternalId { get; set; }
        public long InternalId { get; set; }
        public int Counter { get; set; }
        public string Role { get; set; }

        public Account Clone() => (Account) MemberwiseClone();
    }
}