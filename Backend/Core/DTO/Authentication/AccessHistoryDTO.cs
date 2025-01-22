using Artemis.Backend.Core.DTO.Setup;

namespace Artemis.Backend.Core.DTO.Authentication
{
    public class AccessHistoryDTO
    {
        public int Id { get; set; }
        public UserDTO User { get; set; } = null!;
        public AreaDTO Area { get; set; } = null!;
        public string Status { get; set; } = string.Empty;
        public DateTime InsertDate { get; set; }
    }
}
