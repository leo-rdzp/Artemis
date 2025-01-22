using Artemis.Backend.Core.DTO.Authentication;

namespace Artemis.Backend.Core.DTO.Materials
{
    public class MaterialHistoryDTO
    {
        public int Id { get; set; }
        public LocationDTO? FromLocation { get; set; }
        public LocationDTO ToLocation { get; set; } = null!;
        public decimal Quantity { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public string ReferenceType { get; set; } = string.Empty;
        public int ReferenceId { get; set; }
        public UserDTO User { get; set; } = null!;
        public DateTime InsertDate { get; set; }
    }
}
