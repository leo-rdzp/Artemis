namespace Artemis.Backend.Core.DTO.Materials
{
    public class OnHandMaterialDTO
    {
        public int Id { get; set; }
        public MaterialDTO Material { get; set; } = null!;
        public LocationDTO Location { get; set; } = null!;
        public decimal Quantity { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime InsertDate { get; set; }
    }
}
