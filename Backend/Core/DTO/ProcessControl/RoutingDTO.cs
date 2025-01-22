using Artemis.Backend.Core.DTO.Setup;

namespace Artemis.Backend.Core.DTO.ProcessControl
{
    public class RoutingDTO
    {
        public int Id { get; set; }
        public ProductDTO Product { get; set; } = null!;
        public OperationDTO PreviousOperation { get; set; } = null!;
        public OperationDTO Operation { get; set; } = null!;
        public string? PassFailIndicator { get; set; }
        public DispositionDTO? Disposition { get; set; }
        public string? Priority { get; set; }
        public DateTime InsertDate { get; set; }
        public string UserName { get; set; } = string.Empty;
        public BusinessDTO Business { get; set; } = null!;
    }
}
