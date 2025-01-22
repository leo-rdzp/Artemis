using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Core.DTO.Common;
using Artemis.Backend.Core.Models.Authentication;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Backend.Services.RoleManagement
{
    public class GetRolesService(
        ArtemisDbContext context,
        IMapper mapper,
        ILogger<GetRolesService> logger) : BaseService<FilterDTO>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetRolesService> _logger = logger;

        public override void PrepareMandatoryParameters()
        {
        }

        public override async Task<ResultNotifier> ExecuteAsync(FilterDTO filter)
        {
            try
            {
                IQueryable<Role> query = _context.Roles
                    .AsNoTracking()
                    .Include(r => r.UserRoles!)
                        .ThenInclude(ur => ur.User)
                    .Include(r => r.ApplicationRoles!)
                        .ThenInclude(ar => ar.Application);

                // Apply filters
                query = ApplyFilters(query, filter);

                // Get total count before pagination
                var totalItems = await query.CountAsync();

                // Calculate pagination values
                var pageNumber = filter?.Page ?? 1;
                var pageSize = filter?.PageSize ?? 10;
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                // Apply pagination
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                // Get paginated data
                var roles = await query.ToListAsync();
                var roleDtos = _mapper.Map<List<RoleDTO>>(roles);

                // Create pagination result
                var paginationResult = new PaginationDTO<RoleDTO>
                {
                    Items = roleDtos,
                    Meta = new PaginationMetaDTO
                    {
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        TotalItems = totalItems,
                        TotalPages = totalPages,
                        HasPrevious = pageNumber > 1,
                        HasNext = pageNumber < totalPages
                    }
                };

                return ResultNotifier.Success(paginationResult);
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
                _logger.LogError(ex, "Error retrieving roles");
                return ResultNotifier.Failure("Error retrieving roles");
            }
        }

        private static IQueryable<Role> ApplyFilters(IQueryable<Role> query, FilterDTO? filter)
        {
            if (!string.IsNullOrEmpty(filter?.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                query = query.Where(r =>
                    r.Name.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                    (r.Description != null && r.Description.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase))
                );
            }

            if (!string.IsNullOrEmpty(filter?.Status))
            {
                query = query.Where(r => r.Status == filter.Status);
            }

            return query;
        }
    }
}
