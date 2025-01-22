namespace Artemis.Backend.Core.DTO.Authentication
{
    public class UserRoleDTO
    {
        public int Id { get; set; }
        public UserDTO User { get; set; } = new();
        public int UserId { get; set; }
        public RoleDTO Role { get; set; } = new();
        public int RoleId { get; set; }        
        public DateTime AssignedDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? AssignedBy { get; set; }
    }
}
