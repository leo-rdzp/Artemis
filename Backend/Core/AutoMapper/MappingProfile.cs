using Artemis.Backend.Core.DTO.Analytics;
using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Core.DTO.Devices;
using Artemis.Backend.Core.DTO.EmployeeManagement;
using Artemis.Backend.Core.DTO.ExternalIntegration;
using Artemis.Backend.Core.DTO.InventoryControl;
using Artemis.Backend.Core.DTO.Materials;
using Artemis.Backend.Core.DTO.Notifications;
using Artemis.Backend.Core.DTO.OrderProcessing;
using Artemis.Backend.Core.DTO.Packaging;
using Artemis.Backend.Core.DTO.ProcessControl;
using Artemis.Backend.Core.DTO.Setup;
using Artemis.Backend.Core.Models.Analytics;
using Artemis.Backend.Core.Models.Authentication;
using Artemis.Backend.Core.Models.Devices;
using Artemis.Backend.Core.Models.EmployeeManagement;
using Artemis.Backend.Core.Models.ExternalIntegration;
using Artemis.Backend.Core.Models.InventoryControl;
using Artemis.Backend.Core.Models.Materials;
using Artemis.Backend.Core.Models.Notifications;
using Artemis.Backend.Core.Models.OrderProcessing;
using Artemis.Backend.Core.Models.Packaging;
using Artemis.Backend.Core.Models.ProcessControl;
using Artemis.Backend.Core.Models.Setup;
using Artemis.Backend.Core.Utilities;
using AutoMapper;

namespace Artemis.Backend.Core.AutoMapper
{
    public class MappingProfile : Profile
    {
        private static readonly ItemListTools _itemListTools;
        private static readonly MappingTools _mappingTools;

        static MappingProfile()
        {
            _itemListTools = new ItemListTools();
            _mappingTools = new MappingTools();
        }

        public MappingProfile()
        {
            #region Analytics Domain
            CreateMap<DashboardChart, DashboardChartDTO>()
                .ForMember(dest => dest.Properties, opt => opt.MapFrom((src, dest) => {
                    if (src.Properties?.Any() == true)
                    {
                        return _itemListTools.ConvertPropertiesDTOToItemList(src.Properties.Cast<object>().ToList());
                    }
                    return null;
                }))
                .MaxDepth(2)
                .ReverseMap();
            CreateMap<DashboardChartProperties, DashboardChartPropertiesDTO>().ReverseMap();
            CreateMap<DashboardGroup, DashboardGroupDTO>().ReverseMap();
            #endregion

            #region Authentication Domain
            CreateMap<AccessHistory, AccessHistoryDTO>().ReverseMap();
            CreateMap<ExternalUser, ExternalUserDTO>().ReverseMap();
            CreateMap<Person, PersonDTO>().ReverseMap();
            CreateMap<Role, RoleDTO>()
                .BeforeMap((src, dest) => {
                    if (src.UserRoles?.Any() == true)
                    {
                        dest.Users = _mappingTools.MapUsers(src.UserRoles);
                    }
                });
            CreateMap<RoleDTO, Role>()
                .ForMember(dest => dest.Business, opt => opt.Ignore());
            CreateMap<User, UserDTO>()
                .BeforeMap((src, dest) => {
                    if (src.UserRoles?.Any() == true)
                    {
                        dest.Roles = _mappingTools.MapRoles(src.UserRoles);
                    }
                })
                .ForMember(dest => dest.Properties, opt => opt.MapFrom((src, dest) => {
                    if (src.Properties?.Any() == true)
                    {
                        return _itemListTools.ConvertPropertiesDTOToItemList(src.Properties.Cast<object>().ToList());
                    }
                    return null;
                }))                
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .MaxDepth(2);
            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.Person, opt => opt.Ignore());
            CreateMap<UserImage, UserImageDTO>().ReverseMap();
            CreateMap<UserProperties, UserPropertiesDTO>().ReverseMap();
            CreateMap<LoginAttempt, LoginAttemptDTO>().ReverseMap();
            CreateMap<UserRole, UserRoleDTO>().ReverseMap();
            #endregion

            #region Devices Domain
            CreateMap<Containment, ContainmentDTO>().ReverseMap();
            CreateMap<Device, DeviceDTO>()
                .BeforeMap((src, dest) => {
                    if (src.OrderLineMaps?.Any() == true)
                    {
                        dest.OrderLines = _mappingTools.MapOrderLines(src.OrderLineMaps);
                    }
                })
                .ForMember(dest => dest.Properties, opt => opt.MapFrom((src, dest) => {
                    if (src.Properties?.Any() == true)
                    {
                        return _itemListTools.ConvertPropertiesDTOToItemList(src.Properties.Cast<object>().ToList());
                    }
                    return null;
                }))
                .MaxDepth(2)
                .ReverseMap();
            CreateMap<DeviceBlacklist, DeviceBlacklistDTO>().ReverseMap();
            CreateMap<DeviceConsumption, DeviceConsumptionDTO>().ReverseMap();
            CreateMap<DeviceHistory, DeviceHistoryDTO>().ReverseMap();
            CreateMap<DeviceProperties, DevicePropertiesDTO>().ReverseMap();
            CreateMap<DeviceResults, DeviceResultsDTO>().ReverseMap();
            #endregion

            #region Employee Management Domain
            CreateMap<Agency, AgencyDTO>()
                .ForMember(dest => dest.Properties, opt => opt.MapFrom((src, dest) => {
                    if (src.Properties?.Any() == true)
                    {
                        return _itemListTools.ConvertPropertiesDTOToItemList(src.Properties.Cast<object>().ToList());
                    }
                    return null;
                }))
                .MaxDepth(2)
                .ReverseMap();
            CreateMap<AgencyProperties, AgencyPropertiesDTO>().ReverseMap();
            CreateMap<Clock, ClockDTO>().ReverseMap();
            CreateMap<Department, DepartmentDTO>().ReverseMap();
            CreateMap<Employee, EmployeeDTO>()
                .ForMember(dest => dest.Properties, opt => opt.MapFrom((src, dest) => {
                    if (src.Properties?.Any() == true)
                    {
                        return _itemListTools.ConvertPropertiesDTOToItemList(src.Properties.Cast<object>().ToList());
                    }
                    return null;
                }))
                .MaxDepth(2)
                .ReverseMap();
            CreateMap<EmployeeAttendance, EmployeeAttendanceDTO>().ReverseMap();
            CreateMap<EmployeeHistory, EmployeeHistoryDTO>().ReverseMap();
            CreateMap<EmployeeProperties, EmployeePropertiesDTO>().ReverseMap();
            CreateMap<JobTitle, JobTitleDTO>().ReverseMap();
            #endregion

            #region External Integration Domain
            CreateMap<ExternalEvent, ExternalEventDTO>()
                .ForMember(dest => dest.Properties, opt => opt.MapFrom((src, dest) => {
                    if (src.Properties?.Any() == true)
                    {
                        return _itemListTools.ConvertPropertiesDTOToItemList(src.Properties.Cast<object>().ToList());
                    }
                    return null;
                }))
                .MaxDepth(2)
                .ReverseMap();
            CreateMap<ExternalEventProperties, ExternalEventPropertiesDTO>().ReverseMap();
            CreateMap<Request, RequestDTO>()
                .ForMember(dest => dest.Properties, opt => opt.MapFrom((src, dest) => {
                    if (src.Properties?.Any() == true)
                    {
                        return _itemListTools.ConvertPropertiesDTOToItemList(src.Properties.Cast<object>().ToList());
                    }
                    return null;
                }))
                .MaxDepth(2)
                .ReverseMap();
            CreateMap<RequestProperties, RequestPropertiesDTO>().ReverseMap();
            #endregion

            #region Inventory Control Domain
            CreateMap<Folio, FolioDTO>().ReverseMap();
            CreateMap<FolioNonSerialized, FolioNonSerializedDTO>().ReverseMap();
            CreateMap<FolioSerialized, FolioSerializedDTO>().ReverseMap();
            CreateMap<Inventory, InventoryDTO>().ReverseMap();
            #endregion

            #region Materials Domain
            CreateMap<BomHeader, BomHeaderDTO>().ReverseMap();
            CreateMap<BomComponent, BomComponentDTO>().ReverseMap();
            CreateMap<BomComponentMaterial, BomComponentMaterialDTO>().ReverseMap();
            CreateMap<BomUsageRule, BomUsageRuleDTO>().ReverseMap();
            CreateMap<BomRevisionHistory, BomRevisionHistoryDTO>().ReverseMap();
            CreateMap<Defect, DefectDTO>().ReverseMap();
            CreateMap<Location, LocationDTO>()
                .ForMember(dest => dest.Properties, opt => opt.MapFrom((src, dest) => {
                    if (src.Properties?.Any() == true)
                    {
                        return _itemListTools.ConvertPropertiesDTOToItemList(src.Properties.Cast<object>().ToList());
                    }
                    return null;
                }))
                .MaxDepth(2)
                .ReverseMap();
            CreateMap<LocationProperties, LocationPropertiesDTO>().ReverseMap();
            CreateMap<Material, MaterialDTO>()
                .ForMember(dest => dest.Properties, opt => opt.MapFrom((src, dest) => {
                    if (src.Properties?.Any() == true)
                    {
                        return _itemListTools.ConvertPropertiesDTOToItemList(src.Properties.Cast<object>().ToList());
                    }
                    return null;
                }))
                .MaxDepth(2)
                .ReverseMap();
            CreateMap<MaterialBlacklist, MaterialBlacklistDTO>().ReverseMap();
            CreateMap<MaterialHistory, MaterialHistoryDTO>().ReverseMap();
            CreateMap<MaterialLocation, MaterialLocationDTO>()
                .ForMember(dest => dest.Properties, opt => opt.MapFrom((src, dest) => {
                    if (src.Properties?.Any() == true)
                    {
                        return _itemListTools.ConvertPropertiesDTOToItemList(src.Properties.Cast<object>().ToList());
                    }
                    return null;
                }))
                .MaxDepth(2)
                .ReverseMap();
            CreateMap<MaterialLocationProperties, MaterialLocationPropertiesDTO>().ReverseMap();
            CreateMap<MaterialProperties, MaterialPropertiesDTO>().ReverseMap();
            CreateMap<OnHandMaterial, OnHandMaterialDTO>().ReverseMap();
            CreateMap<ReferenceDesignator, ReferenceDesignatorDTO>().ReverseMap();
            CreateMap<RepairAction, RepairActionDTO>().ReverseMap();
            CreateMap<RepairGroup, RepairGroupDTO>().ReverseMap();
            #endregion

            #region Order Notifications Domain
            CreateMap<Notification, NotificationDTO>().ReverseMap();
            CreateMap<NotificationAcknowledgment, NotificationAcknowledgmentDTO>().ReverseMap();
            #endregion

            #region Order Processing Domain
            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.Properties, opt => opt.MapFrom((src, dest) => {
                    if (src.Properties?.Any() == true)
                    {
                        return _itemListTools.ConvertPropertiesDTOToItemList(src.Properties.Cast<object>().ToList());
                    }
                    return null;
                }))
                .MaxDepth(2)
                .ReverseMap();
            CreateMap<OrderHistory, OrderHistoryDTO>().ReverseMap();
            CreateMap<OrderLine, OrderLineDTO>()
                .BeforeMap((src, dest) => {
                    if (src.OrderLineMaps?.Any() == true)
                    {
                        dest.Devices = _mappingTools.MapDevices(src.OrderLineMaps);
                    }
                    if (src.OrderLinePackageMaps?.Any() == true)
                    {
                        dest.Packages = _mappingTools.MapPackages(src.OrderLinePackageMaps);
                    }
                    if (src.OrderLineRequestMaps?.Any() == true)
                    {
                        dest.Requests = _mappingTools.MapRequests(src.OrderLineRequestMaps);
                    }
                })
                .ReverseMap();
            CreateMap<OrderLineProperties, OrderLinePropertiesDTO>().ReverseMap();
            CreateMap<OrderProperties, OrderPropertiesDTO>().ReverseMap();
            CreateMap<OrderType, OrderTypeDTO>()
                .ForMember(dest => dest.Properties, opt => opt.MapFrom((src, dest) => {
                    if (src.Properties?.Any() == true)
                    {
                        return _itemListTools.ConvertPropertiesDTOToItemList(src.Properties.Cast<object>().ToList());
                    }
                    return null;
                }))
                .MaxDepth(2)
                .ReverseMap();
            CreateMap<OrderTypeProperties, OrderTypePropertiesDTO>().ReverseMap();
            CreateMap<Preload, PreloadDTO>()
                .ForMember(dest => dest.Properties, opt => opt.MapFrom((src, dest) => {
                    if (src.Properties?.Any() == true)
                    {
                        return _itemListTools.ConvertPropertiesDTOToItemList(src.Properties.Cast<object>().ToList());
                    }
                    return null;
                }))
                .MaxDepth(2)
                .ReverseMap();
            CreateMap<PreloadProperties, PreloadPropertiesDTO>().ReverseMap();
            #endregion

            #region Packaging Domain
            CreateMap<Package, PackageDTO>()
                .BeforeMap((src, dest) => {
                    if (src.OrderLinePackageMaps?.Any() == true)
                    {
                        dest.OrderLines = _mappingTools.MapOrderLines(src.OrderLinePackageMaps);
                    }
                })
                .ForMember(dest => dest.Properties, opt => opt.MapFrom((src, dest) => {
                    if (src.Properties?.Any() == true)
                    {
                        return _itemListTools.ConvertPropertiesDTOToItemList(src.Properties.Cast<object>().ToList());
                    }
                    return null;
                }))
                .MaxDepth(2)
                .ReverseMap();
            CreateMap<PackageContainment, PackageContainmentDTO>().ReverseMap();
            CreateMap<PackageHistory, PackageHistoryDTO>().ReverseMap();
            CreateMap<PackageProperties, PackagePropertiesDTO>().ReverseMap();
            #endregion

            #region Process Control Domain
            CreateMap<Disposition, DispositionDTO>().ReverseMap();
            CreateMap<Operation, OperationDTO>()
                .ForMember(dest => dest.Properties, opt => opt.MapFrom((src, dest) => {
                    if (src.Properties?.Any() == true)
                    {
                        return _itemListTools.ConvertPropertiesDTOToItemList(src.Properties.Cast<object>().ToList());
                    }
                    return null;
                }))
                .MaxDepth(2)
                .ReverseMap();
            CreateMap<OperationProperties, OperationPropertiesDTO>().ReverseMap();
            CreateMap<Routing, RoutingDTO>().ReverseMap();
            #endregion

            #region Setup Domain
            CreateMap<ActionParameter, ActionParameterDTO>().ReverseMap();
            CreateMap<Application, ApplicationDTO>()
                .ForMember(dest => dest.Properties, opt => opt.MapFrom((src, dest) => {
                    if (src.Properties?.Any() == true)
                    {
                        return _itemListTools.ConvertPropertiesDTOToItemList(src.Properties.Cast<object>().ToList());
                    }
                    return null;
                }))
                .MaxDepth(2)
                .ReverseMap();
            CreateMap<ApplicationRole, ApplicationRoleDTO>().ReverseMap();
            CreateMap<Area, AreaDTO>().ReverseMap();
            CreateMap<Business, BusinessDTO>().ReverseMap();
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<LabelTemplate, LabelTemplateDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.Properties, opt => opt.MapFrom((src, dest) => {
                    if (src.Properties?.Any() == true)
                    {
                        return _itemListTools.ConvertPropertiesDTOToItemList(src.Properties.Cast<object>().ToList());
                    }
                    return null;
                }))
                .MaxDepth(2)
                .ReverseMap();
            CreateMap<ProductProperties, ProductPropertiesDTO>().ReverseMap();
            #endregion
        }
    }
}
