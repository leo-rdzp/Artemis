namespace Artemis.Frontend.Models.Authentication
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public PersonInfo Person { get; set; } = new();
        public string Status { get; set; } = string.Empty;        
    }
}
