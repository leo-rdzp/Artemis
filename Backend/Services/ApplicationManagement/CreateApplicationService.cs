using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.DTO.Setup;
using Artemis.Backend.Core.Models.Setup;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Artemis.Backend.Services.ApplicationManagement
{
    public class CreateApplicationService(
        ArtemisDbContext context,
        IMapper mapper,
        ILogger<CreateApplicationService> logger,
        ITransactionScope transactionScope) : BaseService<ApplicationDTO>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CreateApplicationService> _logger = logger;
        private readonly ITransactionScope _transactionScope = transactionScope;

        public override void PrepareMandatoryParameters()
        {
            SetMandatoryParameters(["Title", "Station", "Route", "BusinessId", "ApplicationType"]);
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

                if (_context.Applications.Any(a => a.Title == applicationDto.Title))
                {
                    return ResultNotifier.Failure("Application name already exists");
                }

                // Validate Business exists and is active
                var business = await _context.Business
                    .FirstOrDefaultAsync(b => b.Id == applicationDto.BusinessId && b.Status == CommonTags.Active);

                if (business == null)
                {
                    return ResultNotifier.Failure($"Business with ID {applicationDto.BusinessId} not found or is inactive");
                }

                if (!CommonTags.ApplicationTypes.Contains(applicationDto.ApplicationType))
                {
                    return ResultNotifier.Failure($"Invalid Application type. Valid values are: {string.Join(", ", CommonTags.ApplicationTypes)}");
                }

                if (!string.IsNullOrEmpty(applicationDto.Station) && !CommonTags.StationModules.Contains(applicationDto.Station))
                {
                    return ResultNotifier.Failure($"Invalid Station Module Valid values are: {string.Join(", ", CommonTags.StationModules)}");
                }

                if (!string.IsNullOrEmpty(applicationDto.Status))
                {
                    if (!CommonTags.ApplicationStatuses.Contains(applicationDto.Status))
                    {
                        return ResultNotifier.Failure($"Invalid status. Valid values are: {string.Join(", ", CommonTags.ApplicationStatuses)}");
                    }
                }
                else
                {
                    applicationDto.Status = CommonTags.Active;
                }

                var application = _mapper.Map<Application>(applicationDto);
                application.Business = business;                
                application.InsertDate = DateTime.UtcNow;
                application.UpdateDate = DateTime.UtcNow;

                _context.Applications.Add(application);
                await _context.SaveChangesAsync();
                if (IsAutoCommit())
                {
                    await _transactionScope.CommitAsync();
                }

                var createdApplicationDto = _mapper.Map<ApplicationDTO>(application);
                return ResultNotifier.Success(createdApplicationDto);
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
                _logger.LogError(ex, "Error creating application");
                return ResultNotifier.Failure("Error creating application");
            }
        }
    }
}
