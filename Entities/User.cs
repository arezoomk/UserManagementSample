namespace Entities
{
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public bool IsAvailable { get; set; } = true;
    }
}