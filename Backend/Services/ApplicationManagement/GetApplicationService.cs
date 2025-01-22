using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.DTO.Setup;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Backend.Services.ApplicationManagement
{
    public class GetApplicationService(
        ArtemisDbContext context,
        IMapper mapper,
        ILogger<GetApplicationService> logger) : BaseService<int>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetApplicationService> _logger = logger;

        public override void PrepareMandatoryParameters()
        {
        }

        public override async Task<ResultNotifier> ExecuteAsync(int id)
        {
            try
            {
                var application = await _context.Applications
                    .AsNoTracking()
                    .Include(a => a.Business)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (application == null)
                {
                    return ResultNotifier.Failure("Application not found");
                }

                var applicationDto = _mapper.Map<ApplicationDTO>(application);
                return ResultNotifier.Success(applicationDto);
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
                _logger.LogError(ex, "Error retrieving application {applicationId}", id);
                return ResultNotifier.Failure("Error retrieving application");
            }
        }
    }
}
