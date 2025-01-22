using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Core.Models.Authentication;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Artemis.Backend.Services.RoleManagement
{
    public class AssignUserRoleService(
        ArtemisDbContext context,
        IMapper mapper,
        ILogger<AssignUserRoleService> logger,
        ITransactionScope transactionScope) : BaseService<UserRoleDTO>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<AssignUserRoleService> _logger = logger;
        private readonly ITransactionScope _transactionScope = transactionScope;

        public override void PrepareMandatoryParameters()
        {
            SetMandatoryParameters(["UserId", "RoleId", "AssignedBy"]);
        }

        protected override void OnServiceInitialize()
        {
            base.OnServiceInitialize();
            if (!IsAutoCommit())
            {
                _context.Database.UseTransaction(_context.Database.CurrentTransaction?.GetDbTransaction());
            }
        }

        public override async Task<ResultNotifier> ExecuteAsync(UserRoleDTO assignmentDto)
        {
            await _transactionScope.GetTransactionAsync();
            try
            {
                ValidateParameters(assignmentDto);

                // Check if user exists and is active
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == assignmentDto.UserId && u.Status == CommonTags.Active);
                if (user == null)
                    return ResultNotifier.Failure("User not found or inactive");

                // Check if role exists and is active
                var role = await _context.Roles
                    .FirstOrDefaultAsync(r => r.Id == assignmentDto.RoleId && r.Status == CommonTags.Active);
                if (role == null)
                    return ResultNotifier.Failure("Role not found or inactive");

                // Check if assignment already exists
                if (await _context.UserRoles.AnyAsync(ur =>
                    ur.User == user &&
                    ur.Role == role))
                {
                    return ResultNotifier.Failure("User already has this role assigned");
                }

                var userRole = _mapper.Map<UserRole>(assignmentDto);
                userRole.User = user;
                userRole.Role = role;
                userRole.AssignedBy = assignmentDto.AssignedBy!;
                userRole.AssignedDate = DateTime.UtcNow;

                if(assignmentDto.ExpirationDate.HasValue)
                {
                    userRole.ExpirationDate = assignmentDto.ExpirationDate.Value;
                }

                _context.UserRoles.Add(userRole);

                await _context.SaveChangesAsync();
                if (IsAutoCommit())
                {
                    await _transactionScope.CommitAsync();
                }

                return ResultNotifier.Success(_mapper.Map<UserRoleDTO>(userRole));
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
                _logger.LogError(ex, "Error assigning user role");
                return ResultNotifier.Failure("Error assigning user role");
            }
        }
    }
}
