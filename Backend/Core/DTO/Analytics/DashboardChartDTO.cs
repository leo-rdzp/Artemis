namespace Artemis.Backend.Core.DTO.Analytics
{
    public class DashboardChartDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Subtitle { get; set; }
        public string? Legend { get; set; }
        public string ChartType { get; set; } = string.Empty;
        public byte[] Content { get; set; } = null!;
        public string Status { get; set; } = string.Empty;
        public DateTime InsertDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public ICollection<DashboardChartPropertiesDTO>? Properties { get; set; }
        public ICollection<DashboardGroupDTO>? Groups { get; set; }
    }
}
