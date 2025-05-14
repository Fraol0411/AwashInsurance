namespace AwashInsurance.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<UserAccount> UserAccounts { get; set; }
    }

}
