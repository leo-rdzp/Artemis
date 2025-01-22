using Artemis.Backend.Core.DTO.Setup;

namespace Artemis.Backend.Core.DTO.Authentication
{
    public class ApplicationRoleDTO
    {
        public int Id { get; set; }
        public ApplicationDTO Application { get; set; } = new();
        public int ApplicationId { get; set; }
        public RoleDTO Role { get; set; } = new();
        public int RoleId { get; set; }        
        public DateTime InsertDate { get; set; }
        public string? AssignedBy { get; set; }
    }
}
