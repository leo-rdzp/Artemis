namespace Artemis.Backend.Core.DTO.Devices
{
    public class DeviceResultsDTO
    {
        public int Id { get; set; }
        public string? TestCode { get; set; }
        public string? TestName { get; set; }
        public string? LowSpecification { get; set; }
        public string? HighSpecification { get; set; }
        public string? TestTime { get; set; }
        public string? ResultReading { get; set; }
        public string? PassFailIndicator { get; set; }
        public DateTime InsertDate { get; set; }
        public byte[]? TestDetail { get; set; }
        public decimal? UpperLimit { get; set; }
        public decimal? LowerLimit { get; set; }
        public decimal? MeasuredValue { get; set; }
        public string? UnitOfMeasure { get; set; }
    }
}
