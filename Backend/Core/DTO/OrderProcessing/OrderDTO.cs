using Artemis.Backend.Core.DTO.Setup;

namespace Artemis.Backend.Core.DTO.OrderProcessing
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public OrderTypeDTO OrderType { get; set; } = null!;
        public int Quantity { get; set; }
        public int CurrentQty { get; set; }
        public string Status { get; set; } = string.Empty;
        public CustomerDTO? Customer { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public BusinessDTO Business { get; set; } = null!;
        public ICollection<OrderLineDTO>? OrderLines { get; set; }
        public ICollection<OrderPropertiesDTO>? Properties { get; set; }
        public ICollection<OrderHistoryDTO>? History { get; set; }
    }
}
