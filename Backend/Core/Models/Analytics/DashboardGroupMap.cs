using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.Models.Analytics
{
    [Table("DashboardGroupMap")]
    public class DashboardGroupMap
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [ForeignKey("DashboardGroupId")]
        public required DashboardGroup DashboardGroup { get; set; }

        [ForeignKey("DashboardChartId")]
        public required DashboardChart DashboardChart { get; set; }

        [Required]
        public required int PageZ { get; set; }

        [Required]
        public required int RowY { get; set; }

        [Required]
        public required int ColumnX { get; set; }

        [Required]
        public required DateTime InsertDate { get; set; }
    }
}
