using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.DTO.Setup;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Artemis.Backend.Services.ApplicationManagement
{
    public class UpdateApplicationService(
        ArtemisDbContext context,
        IMapper mapper,
        ILogger<UpdateApplicationService> logger,
        ITransactionScope transactionScope,
        ItemListTools itemListTools) : BaseService<ApplicationDTO>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UpdateApplicationService> _logger = logger;
        private readonly ITransactionScope _transactionScope = transactionScope;
        private readonly ItemListTools _itemListTools = itemListTools;

        public override void PrepareMandatoryParameters()
        {
            SetMandatoryParameters(["Id"]);
        }

        protected override void OnServiceInitialize()
        {
            base.OnServiceInitialize();
            if (!IsAutoCommit())
            {
                _context.Database.UseTransaction(_context.Database.CurrentTransaction?.GetDbTransaction());
            }
        }

        public override async Task<ResultNotifier> ExecuteAsync(ApplicationDTO applicationDto)
        {
            await _transactionScope.GetTransactionAsync();
            try
            {
                ValidateParameters(applicationDto);

                var application = await _context.Applications
                    .Include(a => a.Business)
                    .FirstOrDefaultAsync(a => a.Id == applicationDto.Id);

                if (application == null)
                {
                    return ResultNotifier.Failure("Application not found");
                }

                if (application.Title != applicationDto.Title &&
                    _context.Applications.Any(a => a.Title == applicationDto.Title))
                {
                    return ResultNotifier.Failure("Application title already exists");
                }
                
                if (!string.IsNullOrEmpty(applicationDto.ApplicationType))
                {
                    if (!CommonTags.ApplicationTypes.Contains(applicationDto.ApplicationType))
                    {
                        return ResultNotifier.Failure($"Invalid Application type. Valid values are: {string.Join(", ", CommonTags.ApplicationTypes)}");
                    }
                    application.ApplicationType = applicationDto.ApplicationType;
                }

                if (!string.IsNullOrEmpty(applicationDto.Station))
                {
                    if (!CommonTags.StationModules.Contains(applicationDto.Station))
                    {
                        return ResultNotifier.Failure($"Invalid Station module. Valid values are: {string.Join(", ", CommonTags.StationModules)}");
                    }
                    application.Station = applicationDto.Station;
                }

                if (!string.IsNullOrEmpty(applicationDto.Route))
                {
                    application.Route = applicationDto.Route;
                }

                // Check if BusinessId was included and is valid
                var paramList = _itemListTools.ObjectToItemList(applicationDto);
                if (paramList.ContainsKey("BusinessId") && applicationDto.BusinessId > 0)
                {
                    var business = await _context.Business
                        .FirstOrDefaultAsync(b => b.Id == applicationDto.BusinessId && b.Status == CommonTags.Active);

                    if (business == null)
                    {
                        return ResultNotifier.Failure($"Business with ID {applicationDto.BusinessId} not found or is inactive");
                    }

                    application.Business = business;
                }

                if (!string.IsNullOrEmpty(applicationDto.Status))
                {
                    if (!CommonTags.ApplicationStatuses.Contains(applicationDto.Status))
                    {
                        return ResultNotifier.Failure($"Invalid status. Valid values are: {string.Join(", ", CommonTags.ApplicationStatuses)}");
                    }
                    application.Status = applicationDto.Status;
                }

                application.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                if (IsAutoCommit())
                {
                    await _transactionScope.CommitAsync();
                }

                var updatedApplicationDto = _mapper.Map<ApplicationDTO>(application);
                return ResultNotifier.Success(updatedApplicationDto);
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
                await _transactionScope.RollbackAsync();
                _logger.LogError(ex, "Error updating application");
                return ResultNotifier.Failure("Error updating application");
            }
        }
    }
}
