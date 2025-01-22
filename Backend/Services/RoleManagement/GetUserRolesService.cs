using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Backend.Services.RoleManagement
{
    public class GetUserRolesService(
    ArtemisDbContext context,
    IMapper mapper,
    ILogger<GetUserRolesService> logger) : BaseService<int>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetUserRolesService> _logger = logger;

        public override void PrepareMandatoryParameters()
        {
        }

        public override async Task<ResultNotifier> ExecuteAsync(int userId)
        {
            try
            {
                var userRoles = await _context.UserRoles
                    .AsNoTracking()
                    .Include(ur => ur.Role)
                    .Include(ur => ur.User)
                    .Where(ur => ur.User!.Id == userId)
                    .ToListAsync();

                var userRoleDtos = _mapper.Map<List<UserRoleDTO>>(userRoles);
                return ResultNotifier.Success(userRoleDtos);
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
                _logger.LogError(ex, "Error getting user roles");
                return ResultNotifier.Failure("Error getting user roles");
            }
        }
    }
}
