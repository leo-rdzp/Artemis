namespace Artemis.Backend.Core.DTO.EmployeeManagement
{
    public class EmployeePropertiesDTO
    {
        public int Id { get; set; }
        public string Tag { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public byte[] Value { get; set; } = null!;
        public string? Comments { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime InsertDate { get; set; }
    }
}
