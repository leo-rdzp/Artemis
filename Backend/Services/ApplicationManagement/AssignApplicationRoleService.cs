using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Core.DTO.Setup;
using Artemis.Backend.Core.Models.Setup;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Artemis.Backend.Services.ApplicationManagement
{
    public class AssignApplicationRoleService(
    ArtemisDbContext context,
    IMapper mapper,
    ILogger<AssignApplicationRoleService> logger,
    ITransactionScope transactionScope) : BaseService<ApplicationRoleDTO>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<AssignApplicationRoleService> _logger = logger;
        private readonly ITransactionScope _transactionScope = transactionScope;

        public override void PrepareMandatoryParameters()
        {
            SetMandatoryParameters(["ApplicationId", "RoleId", "AssignedBy"]);
        }

        protected override void OnServiceInitialize()
        {
            base.OnServiceInitialize();
            if (!IsAutoCommit())
            {
                _context.Database.UseTransaction(_context.Database.CurrentTransaction?.GetDbTransaction());
            }
        }

        public override async Task<ResultNotifier> ExecuteAsync(ApplicationRoleDTO assignmentDto)
        {
            await _transactionScope.GetTransactionAsync();
            try
            {
                ValidateParameters(assignmentDto);

                // Check if application exists and is active
                var application = await _context.Applications
                    .FirstOrDefaultAsync(a => a.Id == assignmentDto.ApplicationId && a.Status == CommonTags.Active);
                if (application == null)
                    return ResultNotifier.Failure("Application not found or inactive");

                // Check if role exists and is active
                var role = await _context.Roles
                    .FirstOrDefaultAsync(r => r.Id == assignmentDto.RoleId && r.Status == CommonTags.Active);
                if (role == null)
                    return ResultNotifier.Failure("Role not found or inactive");

                // Check if assignment already exists
                if (await _context.ApplicationRoles.AnyAsync(ar =>
                    ar.Application == application &&
                    ar.Role == role))
                {
                    return ResultNotifier.Failure("Application already has this role assigned");
                }

                var applicationRole = _mapper.Map<ApplicationRole>(assignmentDto);
                applicationRole.Application = application;
                applicationRole.Role = role;
                applicationRole.AssignedBy = assignmentDto.AssignedBy!;
                applicationRole.InsertDate = DateTime.UtcNow;

                _context.ApplicationRoles.Add(applicationRole);
                await _context.SaveChangesAsync();
                if (IsAutoCommit())
                {
                    await _transactionScope.CommitAsync();
                }

                return ResultNotifier.Success(_mapper.Map<ApplicationRoleDTO>(applicationRole));
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
                _logger.LogError(ex, "Error assigning application role");
                return ResultNotifier.Failure("Error assigning application role");
            }
        }
    }
}
