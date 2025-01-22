using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Core.DTO.Devices;
using Artemis.Backend.Core.DTO.ExternalIntegration;
using Artemis.Backend.Core.DTO.OrderProcessing;
using Artemis.Backend.Core.DTO.Packaging;
using Artemis.Backend.Core.Models.Authentication;
using Artemis.Backend.Core.Models.Devices;
using Artemis.Backend.Core.Models.OrderProcessing;
using Artemis.Backend.Core.Models.Packaging;
using AutoMapper;

namespace Artemis.Backend.Core.Utilities
{
    public class MappingTools
    {
        private readonly IMapper? _mapper;
        private readonly ILogger<MappingTools>? _logger;

        public MappingTools(IMapper mapper, ILogger<MappingTools> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

        public MappingTools()
        {
            // Empty constructor for static usage
        }

        #region Entity Collections Through Maps
        public ICollection<UserDTO>? MapUsers(ICollection<UserRole>? maps)
        {
            try
            {
                if (maps?.Any() != true) return null;
                if (maps.Any(m => m.User is null)) return null;
                return maps.Select(map =>
                    _mapper?.Map<UserDTO>(map.User) ??
                    new UserDTO { 
                        Id = map.User.Id, 
                        UserName = map.User.UserName, 
                        Status = map.User.Status, 
                        Type = map.User.Type, 
                        PasswordExpirationDate = map.User.PasswordExpirationDate 
                    }).ToList();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error mapping roles from {Count} maps", maps?.Count);
                throw;
            }
        }

        public ICollection<RoleDTO>? MapRoles(ICollection<UserRole>? maps)
        {
            try
            {
                if (maps?.Any() != true) return null;
                if (maps.Any(m => m.Role is null)) return null;
                return maps.Select(map =>
                    _mapper?.Map<RoleDTO>(map.Role) ??
                    new RoleDTO { Id = map.Role.Id, Name = map.Role.Name }).ToList();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error mapping roles from {Count} maps", maps?.Count);
                throw;
            }
        }

        public ICollection<DeviceDTO>? MapDevices(ICollection<OrderLineMap>? maps)
        {
            try
            {
                if (maps?.Any() != true) return null;
                return maps.Select(map => GetCleanDevice(map.Device)).ToList();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error mapping devices from {Count} maps", maps?.Count);
                throw;
            }
        }

        public ICollection<OrderLineDTO>? MapOrderLines(ICollection<OrderLineMap>? maps)
        {
            try
            {
                if (maps?.Any() != true) return null;
                return maps.Select(map => GetCleanOrderLine(map.OrderLine)).ToList();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error mapping order lines from {Count} maps", maps?.Count);
                throw;
            }
        }

        public ICollection<OrderLineDTO>? MapOrderLines(ICollection<OrderLinePackageMap>? maps)
        {
            try
            {
                if (maps?.Any() != true) return null;
                return maps.Select(map => GetCleanOrderLine(map.OrderLine)).ToList();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error mapping order lines from {Count} maps", maps?.Count);
                throw;
            }
        }

        public ICollection<PackageDTO>? MapPackages(ICollection<OrderLinePackageMap>? maps)
        {
            try
            {
                if (maps?.Any() != true) return null;
                return maps.Select(map => GetCleanPackage(map.Package)).ToList();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error mapping packages from {Count} maps", maps?.Count);
                throw;
            }
        }

        public ICollection<RequestDTO>? MapRequests(ICollection<OrderLineRequestMap>? maps)
        {
            try
            {
                if (maps?.Any() != true) return null;
                return maps.Select(map =>
                    _mapper?.Map<RequestDTO>(map.Request) ??
                    new RequestDTO { Id = map.Request.Id }).ToList();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error mapping requests from {Count} maps", maps?.Count);
                throw;
            }
        }
        #endregion

        #region Clean Entity Retrieval
        private OrderLineDTO GetCleanOrderLine(OrderLine line)
        {
            try
            {
                line.Devices = null;  // Remove related collections
                line.Packages = null;
                line.OrderLineMaps = null;
                line.OrderLinePackageMaps = null;
                return _mapper?.Map<OrderLineDTO>(line) ?? new OrderLineDTO { Id = line.Id };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error cleaning OrderLine {LineId}", line.Id);
                throw;
            }
        }

        private DeviceDTO GetCleanDevice(Device device)
        {
            try
            {
                // Break circular reference
                device.OrderLineMaps = null;
                device.OrderLines = null;
                device.Package = null;
                return _mapper?.Map<DeviceDTO>(device) ?? new DeviceDTO { Id = device.Id };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error cleaning Device {DeviceId}", device.Id);
                throw;
            }
        }

        private PackageDTO GetCleanPackage(Package package)
        {
            try
            {
                // Remove related collections
                package.Devices = null;
                package.OrderLinePackageMaps = null;
                return _mapper?.Map<PackageDTO>(package) ?? new PackageDTO { Id = package.Id };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error cleaning Package {PackageId}", package.Id);
                throw;
            }
        }
        #endregion
    }
}