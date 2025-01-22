using Artemis.Backend.Core.DTO.Setup;

namespace Artemis.Backend.Core.DTO.Authentication
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }

        public BusinessDTO Business { get; set; } = new();
        
        public int BusinessId { get; set; }

        public ICollection<UserDTO>? Users { get; internal set; }
    }
}
