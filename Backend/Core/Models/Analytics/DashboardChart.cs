using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Analytics
{
    [Table("DashboardChart")]
    public class DashboardChart
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [StringLength(50)]
        public string? Title { get; set; }

        [StringLength(50)]
        public string? Subtitle { get; set; }

        [StringLength(50)]
        public string? Legend { get; set; }

        [Required]
        [StringLength(15)]
        public required string ChartType { get; set; }

        [Required]
        public required byte[] Content { get; set; }

        [Required]
        [StringLength(15)]
        public required string Status { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }

        [Required]
        public required DateTime UpdateDate { get; set; }

        public ICollection<DashboardChartProperties>? Properties { get; set; }

        public ICollection<DashboardGroupMap>? GroupMaps { get; set; }
    }
}
