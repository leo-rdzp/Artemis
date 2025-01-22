using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.DTO.Util
{
    public class ArtExcelWorkbook
    {
        [Required]
        public string? FileName { get; set; }
        [Required]
        public string? FileExt { get; set; }
        public string? FileTemplate { get; set; }
        [Required]
        public string? PathToSave { get; set; }
        [Required]
        [MinLength(1)]
        public List<ArtExcelWorksheet>? Worksheets { get; set; }
    }
}
