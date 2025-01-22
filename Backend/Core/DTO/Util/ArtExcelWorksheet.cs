using Artemis.Backend.Core.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Backend.Core.DTO.Util
{
    public class ArtExcelWorksheet
    {
        [Required]
        public int Index { get; set; }
        public string? Name { get; set; }
        [Required]
        [MinLength(1)]
        public List<ItemList>? Data { get; set; }
        public ArtExcelStyles? HeaderStyle { get; set; }
        public ArtExcelStyles? ContentStyle { get; set; }

        public ItemList? ReplaceVariables { get; set; }
        public int? StartRow { get; set; }
        public int? StartColumn { get; set; }

    }
}
