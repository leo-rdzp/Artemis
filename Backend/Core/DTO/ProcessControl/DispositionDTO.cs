namespace Artemis.Backend.Core.DTO.ProcessControl
{
    public class DispositionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime InsertDate { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
