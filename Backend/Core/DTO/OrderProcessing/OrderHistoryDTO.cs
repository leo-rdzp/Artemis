using Artemis.Backend.Core.DTO.Authentication;

namespace Artemis.Backend.Core.DTO.OrderProcessing
{
    public class OrderHistoryDTO
    {
        public int Id { get; set; }
        public OrderDTO Order { get; set; } = null!;
        public OrderLineDTO OrderLine { get; set; } = null!;
        public string Status { get; set; } = string.Empty;
        public string? Comments { get; set; }
        public DateTime InsertDate { get; set; }
        public UserDTO User { get; set; } = null!;
    }
}
