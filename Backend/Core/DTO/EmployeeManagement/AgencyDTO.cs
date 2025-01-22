namespace Artemis.Backend.Core.DTO.EmployeeManagement
{
    public class AgencyDTO
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime InsertDate { get; set; }
        public string UserName { get; set; } = string.Empty;
        public ICollection<EmployeeDTO>? Employees { get; set; }
        public ICollection<AgencyPropertiesDTO>? Properties { get; set; }
    }
}
