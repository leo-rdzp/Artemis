using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Core.DTO.Common;
using Artemis.Backend.Core.Models.Authentication;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Backend.Services.PersonManagement
{
    public class GetPersonsService(
        ArtemisDbContext context,
        IMapper mapper,
        ILogger<GetPersonsService> logger) : BaseService<FilterDTO>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetPersonsService> _logger = logger;

        public override void PrepareMandatoryParameters()
        {
        }

        public override async Task<ResultNotifier> ExecuteAsync(FilterDTO filter)
        {
            try
            {
                IQueryable<Person> query = _context.Persons
                    .AsNoTracking();

                // Apply filters
                query = ApplyFilters(query, filter);

                // Get total count before pagination
                var totalItems = await query.CountAsync();

                // Calculate pagination values
                var pageNumber = filter?.Page ?? 1;
                var pageSize = filter?.PageSize ?? 10;  // Default page size
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                // Apply pagination
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                // Get paginated data
                var persons = await query.ToListAsync();
                var personDtos = _mapper.Map<List<PersonDTO>>(persons);

                // Create pagination result
                var paginationResult = new PaginationDTO<PersonDTO>
                {
                    Items = personDtos,
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
                _logger.LogError(ex, "Error retrieving persons");
                return ResultNotifier.Failure("Error retrieving persons");
            }
        }

        private static IQueryable<Person> ApplyFilters(IQueryable<Person> query, FilterDTO filter)
        {
            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                query = query.Where(p =>
                    p.FirstName.ToLower().Contains(searchTerm) ||
                    p.LastName.ToLower().Contains(searchTerm)
                );
            }

            if (!string.IsNullOrEmpty(filter.Status))
            {
                query = query.Where(p => p.Status == filter.Status);
            }

            return query;
        }
    }
}
