namespace Artemis.Backend.Core.DTO.Setup
{
    public class ApplicationDTO
    {
        public int Id { get; set; }
        public BusinessDTO Business { get; set; } = new();
        public int BusinessId { get; set; }
        public string? ApplicationType { get; set; }
        public string? Station { get; set; }
        public string? Title { get; set; }
        public string Route { get; set; } = string.Empty;
        public string? Status { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public ICollection<ApplicationPropertiesDTO>? Properties { get; set; }

    }
}
