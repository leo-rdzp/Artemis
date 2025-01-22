using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Backend.Services.RoleManagement
{
    public class GetRoleService(
        ArtemisDbContext context,
        IMapper mapper,
        ILogger<GetRoleService> logger) : BaseService<int>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetRoleService> _logger = logger;

        public override void PrepareMandatoryParameters()
        {
        }

        public override async Task<ResultNotifier> ExecuteAsync(int id)
        {
            try
            {
                var role = await _context.Roles
                    .AsNoTracking()
                    .Include(r => r.Business)
                    .Include(r => r.UserRoles!)
                        .ThenInclude(ur => ur.User)
                    .Include(r => r.ApplicationRoles!)
                        .ThenInclude(ar => ar.Application)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (role == null)
                {
                    return ResultNotifier.Failure("Role not found");
                }

                var roleDto = _mapper.Map<RoleDTO>(role);
                return ResultNotifier.Success(roleDto);
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
                _logger.LogError(ex, "Error retrieving role {roleId}", id);
                return ResultNotifier.Failure("Error retrieving role");
            }
        }
    }
}
