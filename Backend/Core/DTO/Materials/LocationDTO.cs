using Artemis.Backend.Core.DTO.Setup;

namespace Artemis.Backend.Core.DTO.Materials
{
    public class LocationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public AreaDTO Area { get; set; } = null!;
        public BusinessDTO Business { get; set; } = null!;
        public DateTime InsertDate { get; set; }
        public string UserName { get; set; } = string.Empty;
        public ICollection<LocationPropertiesDTO>? Properties { get; set; }
        public ICollection<MaterialLocationDTO>? MaterialLocations { get; set; }
    }
}
