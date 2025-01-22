using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Backend.Services.UserManagement
{
    public class GetUserService(
        ArtemisDbContext context,
        IMapper mapper,
        ILogger<GetUserService> logger) : BaseService<int>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetUserService> _logger = logger;

        public override void PrepareMandatoryParameters()
        {
        }

        public override async Task<ResultNotifier> ExecuteAsync(int userId)
        {
            try
            {
                var user = await _context.Users
                    .AsNoTracking()
                    .Include(u => u.Person)
                    .Include(u => u.UserRoles!)
                        .ThenInclude(ur => ur.Role)
                    .Include(u => u.Properties!)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    return ResultNotifier.Failure("User not found");
                }

                var userDto = _mapper.Map<UserDTO>(user);
                return ResultNotifier.Success(userDto);
            }
            catch (ArtemisException ex)
            {
                _logger.LogError(
                    "Artemis Exception occurred. Message: {Message}, Details: {Details}",
                    ex.Message,
                    ex.DetailedMessage);
                return ResultNotifier.Failure($"{ex.Message} - {ex.DetailedMessage}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user {userId}", userId);
                return ResultNotifier.Failure("Error retrieving user");
            }
        }
    }
}
