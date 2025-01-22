using Artemis.Backend.Core.DTO.Authentication;

namespace Artemis.Backend.Core.DTO.Analytics
{
    public class DashboardGroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public UserDTO Owner { get; set; } = null!;
        public string Status { get; set; } = string.Empty;
        public DateTime InsertDate { get; set; }
        public ICollection<AccessHistoryDTO>? AccessHistory { get; set; }
        public ICollection<DashboardGroupDTO>? Groups { get; set; }
    }
}
