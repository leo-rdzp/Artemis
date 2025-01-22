namespace Artemis.Backend.Core.DTO.Authentication
{
    public class UserImageDTO
    {
        public int Id { get; set; }
        public byte[] Image { get; set; } = null!;
        public DateTime InsertDate { get; set; }
    }
}
