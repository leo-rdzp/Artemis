namespace Artemis.Backend.Core.DTO.Analytics
{
    public class DashboardChartPropertiesDTO
    {
        public int Id { get; set; }
        public string Tag { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string? Comments { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime InsertDate { get; set; }
    }
}
