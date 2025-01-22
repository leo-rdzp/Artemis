using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.DTO.Common;
using Artemis.Backend.Core.DTO.Setup;
using Artemis.Backend.Core.Models.Setup;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Backend.Services.BusinessManagement
{
    public class GetBusinessesService(
        ArtemisDbContext context,
        IMapper mapper,
        ILogger<GetBusinessesService> logger) : BaseService<FilterDTO>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetBusinessesService> _logger = logger;

        public override void PrepareMandatoryParameters()
        {
        }

        public override async Task<ResultNotifier> ExecuteAsync(FilterDTO filter)
        {
            try
            {
                IQueryable<Business> query = _context.Business
                    .AsNoTracking();

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
                var businesses = await query.ToListAsync();
                var businessDtos = _mapper.Map<List<BusinessDTO>>(businesses);

                // Create pagination result
                var paginationResult = new PaginationDTO<BusinessDTO>
                {
                    Items = businessDtos,
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
                _logger.LogError(ex, "Error retrieving businesses");
                return ResultNotifier.Failure("Error retrieving businesses");
            }
        }

        private static IQueryable<Business> ApplyFilters(IQueryable<Business> query, FilterDTO? filter)
        {
            if (!string.IsNullOrEmpty(filter?.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                query = query.Where(b =>
                    b.Name.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                    b.SapPlant.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                    b.Type.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                );
            }

            if (!string.IsNullOrEmpty(filter?.Status))
            {
                query = query.Where(b => b.Status == filter.Status);
            }

            return query;
        }
    }
}
