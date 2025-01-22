using Artemis.Frontend.Models.Share;

namespace Artemis.Frontend.Models.Navigation
{
    public class Application
    {
        public int Id { get; set; }
        public string? ApplicationType { get; set; }
        public string? Station { get; set; }
        public string? Title { get; set; }
        public string Route { get; set; } = string.Empty;
        public string? Status { get; set; }
        public ICollection<Properties>? Properties { get; set; }
    }
}
