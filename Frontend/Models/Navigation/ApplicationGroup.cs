namespace Artemis.Frontend.Models.Navigation
{
    public class ApplicationGroup
    {
        public string Name { get; set; } = string.Empty;
        public List<Application> Applications { get; set; } = [];
    }
}
