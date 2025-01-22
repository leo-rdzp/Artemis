using Artemis.Frontend.Models.Setup;

namespace Artemis.Frontend.Services.Users
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetUsers();
        Task<UserDTO> GetById(int id);
        Task<UserDTO> Create(UserDTO user);
        Task<UserDTO> Update(UserDTO user);
        Task Delete(int id);
    }
}
