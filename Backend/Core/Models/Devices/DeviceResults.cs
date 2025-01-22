using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Devices
{
    [Table("DeviceResults")]
    public class DeviceResults
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("DeviceHistoryId")]
        public required DeviceHistory DeviceHistory { get; set; }

        [StringLength(30)]
        public string? TestCode { get; set; }

        [StringLength(50)]
        public string? TestName { get; set; }

        [StringLength(255)]
        public string? LowSpecification { get; set; }

        [StringLength(255)]
        public string? HighSpecification { get; set; }

        [StringLength(20)]
        public string? TestTime { get; set; }

        [StringLength(255)]
        public string? ResultReading { get; set; }

        [StringLength(1)]
        public string? PassFailIndicator { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        public byte[]? TestDetail { get; set; }

        public decimal? UpperLimit { get; set; }

        public decimal? LowerLimit { get; set; }

        public decimal? MeasuredValue { get; set; }

        [StringLength(10)]
        public string? UnitOfMeasure { get; set; }
    }
}
