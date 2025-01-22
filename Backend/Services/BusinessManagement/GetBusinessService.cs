using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.DTO.Setup;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Backend.Services.BusinessManagement
{
    public class GetBusinessService(
        ArtemisDbContext context,
        IMapper mapper,
        ILogger<GetBusinessService> logger) : BaseService<int>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetBusinessService> _logger = logger;

        public override void PrepareMandatoryParameters()
        {
        }

        public override async Task<ResultNotifier> ExecuteAsync(int id)
        {
            try
            {
                var business = await _context.Business
                    .AsNoTracking()
                    .FirstOrDefaultAsync(b => b.Id == id);

                if (business == null)
                {
                    return ResultNotifier.Failure("Business not found");
                }

                var businessDto = _mapper.Map<BusinessDTO>(business);
                return ResultNotifier.Success(businessDto);
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
                _logger.LogError(ex, "Error retrieving business {businessId}", id);
                return ResultNotifier.Failure("Error retrieving business");
            }
        }
    }
}
