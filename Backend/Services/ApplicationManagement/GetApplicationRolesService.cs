using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Backend.Services.ApplicationManagement
{
    public class GetApplicationRolesService(
    ArtemisDbContext context,
    IMapper mapper,
    ILogger<GetApplicationRolesService> logger) : BaseService<int>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetApplicationRolesService> _logger = logger;

        public override void PrepareMandatoryParameters()
        {
        }

        public override async Task<ResultNotifier> ExecuteAsync(int applicationId)
        {
            try
            {
                var applicationRoles = await _context.ApplicationRoles
                    .AsNoTracking()
                    .Include(ar => ar.Role)
                    .Include(ar => ar.Application)
                    .Where(ar => ar.Application!.Id == applicationId)
                    .ToListAsync();

                var applicationRoleDtos = _mapper.Map<List<ApplicationRoleDTO>>(applicationRoles);
                return ResultNotifier.Success(applicationRoleDtos);
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
                _logger.LogError(ex, "Error getting application roles");
                return ResultNotifier.Failure("Error getting application roles");
            }
        }
    }
}
