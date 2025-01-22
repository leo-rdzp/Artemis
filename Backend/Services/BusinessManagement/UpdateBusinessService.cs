using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.DTO.Setup;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Text.RegularExpressions;

namespace Artemis.Backend.Services.BusinessManagement
{
    public class UpdateBusinessService(
        ArtemisDbContext context,
        IMapper mapper,
        ILogger<UpdateBusinessService> logger,
        ITransactionScope transactionScope,
        ItemListTools itemListTools) : BaseService<BusinessDTO>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UpdateBusinessService> _logger = logger;
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

        public override async Task<ResultNotifier> ExecuteAsync(BusinessDTO businessDto)
        {
            await _transactionScope.GetTransactionAsync();
            try
            {
                ValidateParameters(businessDto);

                if (businessDto.Id < 1)
                {
                    return ResultNotifier.Failure("Valid Business Id is Required!");
                }

                var business = await _context.Business
                    .FirstOrDefaultAsync(b => b.Id == businessDto.Id);

                if (business == null)
                {
                    return ResultNotifier.Failure("Business not found");
                }

                if (business.Name != businessDto.Name &&
                    _context.Business.Any(b => b.Name == businessDto.Name))
                {
                    return ResultNotifier.Failure("Business name already exists");
                }
                else if(!string.IsNullOrEmpty(businessDto.Name))
                {
                    business.Name = businessDto.Name;
                }

                if (!string.IsNullOrEmpty(businessDto.SapPlant))
                {
                    business.SapPlant = businessDto.SapPlant;
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
                    business.Icon = businessDto.Icon;
                }

                if (!string.IsNullOrEmpty(businessDto.Type))
                {
                    if (!CommonTags.BusinessTypes.Contains(businessDto.Type))
                    {
                        return ResultNotifier.Failure($"Invalid Type. Valid values are: {string.Join(", ", CommonTags.BusinessTypes)}");
                    }
                    business.Type = businessDto.Type;
                }

                if (!string.IsNullOrEmpty(businessDto.Status))
                {
                    if (!CommonTags.BusinessStatuses.Contains(businessDto.Status))
                    {
                        return ResultNotifier.Failure($"Invalid status. Valid values are: {string.Join(", ", CommonTags.BusinessStatuses)}");
                    }
                    business.Status = businessDto.Status;
                }

                business.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                if (IsAutoCommit())
                {
                    await _transactionScope.CommitAsync();
                }

                var updatedBusinessDto = _mapper.Map<BusinessDTO>(business);
                return ResultNotifier.Success(updatedBusinessDto);
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
                _logger.LogError(ex, "Error updating business");
                return ResultNotifier.Failure("Error updating business");
            }
        }
    }
}
