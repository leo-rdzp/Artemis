using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.DTO.Setup;
using Artemis.Backend.Core.Models.Setup;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Artemis.Backend.Services.BusinessManagement
{
    public class CreateBusinessService(
        ArtemisDbContext context,
        IMapper mapper,
        ILogger<CreateBusinessService> logger,
        ITransactionScope transactionScope) : BaseService<BusinessDTO>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CreateBusinessService> _logger = logger;
        private readonly ITransactionScope _transactionScope = transactionScope;

        public override void PrepareMandatoryParameters()
        {
            SetMandatoryParameters(["Name", "SapPlant", "Type"]);
        }

        protected override void OnServiceInitialize()
        {
            base.OnServiceInitialize();
            if (!IsAutoCommit())
            {
                _context.Database.UseTransaction(_context.Database.CurrentTransaction?.GetDbTransaction());
            }
        }

        public override async Task<ResultNotifier> ExecuteAsync(BusinessDTO businessDto)
        {
            await _transactionScope.GetTransactionAsync();
            try
            {
                ValidateParameters(businessDto);

                if (_context.Business.Any(b => b.Name == businessDto.Name))
                {
                    return ResultNotifier.Failure("Business name already exists");
                }

                if (!string.IsNullOrEmpty(businessDto.Status))
                {
                    if (!CommonTags.BusinessStatuses.Contains(businessDto.Status))
                    {
                        return ResultNotifier.Failure($"Invalid status. Valid values are: {string.Join(", ", CommonTags.BusinessStatuses)}");
                    }
                }
                else
                {
                    businessDto.Status = CommonTags.Active;
                }

                if (!CommonTags.BusinessTypes.Contains(businessDto.Type))
                {
                    return ResultNotifier.Failure($"Invalid Type. Valid values are: {string.Join(", ", CommonTags.BusinessTypes)}");
                }

                if (!string.IsNullOrEmpty(businessDto.Icon))
                {
                    if (!Regex.IsMatch(businessDto.Icon, CommonTags.IconPattern))
                    {
                        return ResultNotifier.Failure($"Invalid Icon format. Format should be 'prefix:name' where prefix is one of: {string.Join(", ", CommonTags.IconPrefixes)}");
                    }

                    var prefix = businessDto.Icon.Split(':')[0];
                    if (!CommonTags.IconPrefixes.Contains(prefix))
                    {
                        return ResultNotifier.Failure($"Invalid Icon prefix. Valid prefixes are: {string.Join(", ", CommonTags.IconPrefixes)}");
                    }
                }

                var business = _mapper.Map<Business>(businessDto);
                business.InsertDate = DateTime.UtcNow;
                business.UpdateDate = DateTime.UtcNow;

                _context.Business.Add(business);
                await _context.SaveChangesAsync();
                if (IsAutoCommit())
                {
                    await _transactionScope.CommitAsync();
                }

                var createdBusinessDto = _mapper.Map<BusinessDTO>(business);
                return ResultNotifier.Success(createdBusinessDto);
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
                _logger.LogError(ex, "Error creating business");
                return ResultNotifier.Failure("Error creating business");
            }
        }
    }
}
