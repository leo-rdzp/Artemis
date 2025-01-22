using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.DTO.Setup;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Backend.Services.Authentication
{
    public class MenuService : BaseService<string>
    {
        private readonly ArtemisDbContext _context;
        private readonly ITransactionScope _transactionScope;
        private readonly ILogger<MenuService> _logger;

        public MenuService(
            ArtemisDbContext context,
            ITransactionScope transactionScope,
            ILogger<MenuService> logger)
        {
            _context = context;
            _transactionScope = transactionScope;
            _logger = logger;
        }

        public override void PrepareMandatoryParameters()
        {
        }

        public override async Task<ResultNotifier> ExecuteAsync(string userName)
        {
            using var transaction = await _transactionScope.GetTransactionAsync();
            try
            {
                _logger.LogInformation("Getting menu for user: {UserName}", userName);

                var query = await _context.UserRoles
                    .AsNoTracking()
                    .Where(ur => ur.User.UserName == userName &&
                                ur.Role.Status == CommonTags.Active &&
                                (ur.ExpirationDate == null || ur.ExpirationDate > DateTime.UtcNow))
                    .Include(ur => ur.Role)
                        .ThenInclude(r => r.ApplicationRoles!)
                            .ThenInclude(ar => ar.Application)
                                .ThenInclude(a => a.Properties)
                    .ToListAsync();

                var applications = query
                   .SelectMany(ur => ur.Role.ApplicationRoles ?? [])
                   .Select(ar => new ApplicationDTO
                   {
                       Id = ar.Application.Id,
                       Title = ar.Application.Title,
                       Station = ar.Application.Station,
                       Route = ar.Application.Route,
                       ApplicationType = ar.Application.ApplicationType,
                       Status = ar.Application.Status,
                       Properties = (ar.Application.Properties != null)
                           ? new List<ApplicationPropertiesDTO>(
                               ar.Application.Properties.Select(ap => new ApplicationPropertiesDTO
                               {
                                   Tag = ap.Tag,
                                   Value = ap.Value
                               }))
                           : new List<ApplicationPropertiesDTO>()
                   })
                   .Distinct()
                   .ToList();

                if (!applications.Any())
                {
                    _logger.LogWarning("No applications found for user {userName}", userName);
                    return ResultNotifier.Failure($"No applications found for user {userName}");
                }

                var menuByType = applications
                    .GroupBy(a => a.ApplicationType)
                    .Select(g => new ApplicationGroupDTO
                    {
                        Name = g.Key??string.Empty,
                        Applications = g.OrderBy(a => a.Title).ToList()
                    })
                    .OrderBy(at => at.Name)
                    .ToList();

                await transaction.CommitAsync();
                return ResultNotifier.Success(menuByType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting menu for user {userName}", userName);
                await transaction.RollbackAsync();
                return ResultNotifier.Failure(ex.Message);
            }
        }
    }
}