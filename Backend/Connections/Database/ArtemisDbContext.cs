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
using Microsoft.EntityFrameworkCore;

namespace Artemis.Backend.Connections.Database
{
    public class ArtemisDbContext : DbContext
    {
        public ArtemisDbContext(DbContextOptions<ArtemisDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Setup Sequences
            ConfigureSequences(modelBuilder);

            // Configure Relationships
            ConfigureRelationships(modelBuilder);

            // Other Configurations
            ConfigureProperties(modelBuilder);
        }

        private static void ConfigureSequences(ModelBuilder modelBuilder)
        {
            // Analytics
            modelBuilder.HasSequence<int>("AccessDashboardMapS");
            modelBuilder.Entity<AccessDashboardMap>(b => b.Property(e => e.Id).UseHiLo("AccessDashboardMapS"));

            modelBuilder.HasSequence<int>("DashboardChartPropertiesS");
            modelBuilder.Entity<DashboardChartProperties>(b => b.Property(e => e.Id).UseHiLo("DashboardChartPropertiesS"));

            modelBuilder.HasSequence<int>("DashboardChartS");
            modelBuilder.Entity<DashboardChart>(b => b.Property(e => e.Id).UseHiLo("DashboardChartS"));

            modelBuilder.HasSequence<int>("DashboardGroupMapS");
            modelBuilder.Entity<DashboardGroupMap>(b => b.Property(e => e.Id).UseHiLo("DashboardGroupMapS"));

            modelBuilder.HasSequence<int>("DashboardGroupS");
            modelBuilder.Entity<DashboardGroup>(b => b.Property(e => e.Id).UseHiLo("DashboardGroupS"));

            // Authentication
            modelBuilder.HasSequence<int>("AccessHistoryS");
            modelBuilder.Entity<AccessHistory>(b => b.Property(e => e.Id).UseHiLo("AccessHistoryS"));

            modelBuilder.HasSequence<int>("ExternalUserS");
            modelBuilder.Entity<ExternalUser>(b => b.Property(e => e.Id).UseHiLo("ExternalUserS"));

            modelBuilder.HasSequence<int>("PersonS");
            modelBuilder.Entity<Person>(b => b.Property(e => e.Id).UseHiLo("PersonS"));

            modelBuilder.HasSequence<int>("RoleS");
            modelBuilder.Entity<Role>(b => b.Property(e => e.Id).UseHiLo("RoleS"));

            modelBuilder.HasSequence<int>("UserAreaMapS");
            modelBuilder.Entity<UserAreaMap>(b => b.Property(e => e.Id).UseHiLo("UserAreaMapS"));

            modelBuilder.HasSequence<int>("UserImageS");
            modelBuilder.Entity<UserImage>(b => b.Property(e => e.Id).UseHiLo("UserImageS"));

            modelBuilder.HasSequence<int>("UserPropertiesS");
            modelBuilder.Entity<UserProperties>(b => b.Property(e => e.Id).UseHiLo("UserPropertiesS"));

            modelBuilder.HasSequence<int>("UserRoleS");
            modelBuilder.Entity<UserRole>(b => b.Property(e => e.Id).UseHiLo("UserRoleS"));

            modelBuilder.HasSequence<int>("UserS");
            modelBuilder.Entity<User>(b => b.Property(e => e.Id).UseHiLo("UserS"));

            modelBuilder.HasSequence<int>("LoginAttemptS");
            modelBuilder.Entity<LoginAttempt>(b => b.Property(e => e.Id).UseHiLo("LoginAttemptS"));

            // Devices
            modelBuilder.HasSequence<int>("ContainmentS");
            modelBuilder.Entity<Containment>(b => b.Property(e => e.Id).UseHiLo("ContainmentS"));

            modelBuilder.HasSequence<int>("DeviceBlacklistS");
            modelBuilder.Entity<DeviceBlacklist>(b => b.Property(e => e.Id).UseHiLo("DeviceBlacklistS"));

            modelBuilder.HasSequence<int>("DeviceConsumptionS");
            modelBuilder.Entity<DeviceConsumption>(b => b.Property(e => e.Id).UseHiLo("DeviceConsumptionS"));

            modelBuilder.HasSequence<int>("DeviceHistoryS");
            modelBuilder.Entity<DeviceHistory>(b => b.Property(e => e.Id).UseHiLo("DeviceHistoryS"));

            modelBuilder.HasSequence<int>("DevicePropertiesS");
            modelBuilder.Entity<DeviceProperties>(b => b.Property(e => e.Id).UseHiLo("DevicePropertiesS"));

            modelBuilder.HasSequence<int>("DeviceResultsS");
            modelBuilder.Entity<DeviceResults>(b => b.Property(e => e.Id).UseHiLo("DeviceResultsS"));

            modelBuilder.HasSequence<int>("DeviceS");
            modelBuilder.Entity<Device>(b => b.Property(e => e.Id).UseHiLo("DeviceS"));

            // Employee Management
            modelBuilder.HasSequence<int>("AgencyPropertiesS");
            modelBuilder.Entity<AgencyProperties>(b => b.Property(e => e.Id).UseHiLo("AgencyPropertiesS"));

            modelBuilder.HasSequence<int>("AgencyS");
            modelBuilder.Entity<Agency>(b => b.Property(e => e.Id).UseHiLo("AgencyS"));

            modelBuilder.HasSequence<int>("ClockS");
            modelBuilder.Entity<Clock>(b => b.Property(e => e.Id).UseHiLo("ClockS"));

            modelBuilder.HasSequence<int>("DepartmentS");
            modelBuilder.Entity<Department>(b => b.Property(e => e.Id).UseHiLo("DepartmentS"));

            modelBuilder.HasSequence<int>("EmployeeAttendanceS");
            modelBuilder.Entity<EmployeeAttendance>(b => b.Property(e => e.Id).UseHiLo("EmployeeAttendanceS"));

            modelBuilder.HasSequence<int>("EmployeeHistoryS");
            modelBuilder.Entity<EmployeeHistory>(b => b.Property(e => e.Id).UseHiLo("EmployeeHistoryS"));

            modelBuilder.HasSequence<int>("EmployeePropertiesS");
            modelBuilder.Entity<EmployeeProperties>(b => b.Property(e => e.Id).UseHiLo("EmployeePropertiesS"));

            modelBuilder.HasSequence<int>("EmployeeS");
            modelBuilder.Entity<Employee>(b => b.Property(e => e.Id).UseHiLo("EmployeeS"));

            modelBuilder.HasSequence<int>("JobTitleS");
            modelBuilder.Entity<JobTitle>(b => b.Property(e => e.Id).UseHiLo("JobTitleS"));

            // External Integration
            modelBuilder.HasSequence<int>("ExternalEventIdPropertiesS");
            modelBuilder.Entity<ExternalEventProperties>(b => b.Property(e => e.Id).UseHiLo("ExternalEventIdPropertiesS"));

            modelBuilder.HasSequence<int>("ExternalEventS");
            modelBuilder.Entity<ExternalEvent>(b => b.Property(e => e.Id).UseHiLo("ExternalEventS"));

            modelBuilder.HasSequence<int>("RequestPropertiesS");
            modelBuilder.Entity<RequestProperties>(b => b.Property(e => e.Id).UseHiLo("RequestPropertiesS"));

            modelBuilder.HasSequence<int>("RequestS");
            modelBuilder.Entity<Request>(b => b.Property(e => e.Id).UseHiLo("RequestS"));

            // Inventory Control  
            modelBuilder.HasSequence<int>("FolioNonSerializedS");
            modelBuilder.Entity<FolioNonSerialized>(b => b.Property(e => e.Id).UseHiLo("FolioNonSerializedS"));

            modelBuilder.HasSequence<int>("FolioS");
            modelBuilder.Entity<Folio>(b => b.Property(e => e.Id).UseHiLo("FolioS"));

            modelBuilder.HasSequence<int>("FolioSerializedS");
            modelBuilder.Entity<FolioSerialized>(b => b.Property(e => e.Id).UseHiLo("FolioSerializedS"));

            modelBuilder.HasSequence<int>("InventoryAreaMapS");
            modelBuilder.Entity<InventoryAreaMap>(b => b.Property(e => e.Id).UseHiLo("InventoryAreaMapS"));

            modelBuilder.HasSequence<int>("InventoryMaterialMapS");
            modelBuilder.Entity<InventoryMaterialMap>(b => b.Property(e => e.Id).UseHiLo("InventoryMaterialMapS"));

            modelBuilder.HasSequence<int>("InventoryProductMapS");
            modelBuilder.Entity<InventoryProductMap>(b => b.Property(e => e.Id).UseHiLo("InventoryProductMapS"));

            modelBuilder.HasSequence<int>("InventoryS");
            modelBuilder.Entity<Inventory>(b => b.Property(e => e.Id).UseHiLo("InventoryS"));

            // Materials
            modelBuilder.HasSequence<int>("BomHeaderS");
            modelBuilder.Entity<BomHeader>(b => b.Property(e => e.Id).UseHiLo("BomHeaderS"));

            modelBuilder.HasSequence<int>("BomComponentS");
            modelBuilder.Entity<BomComponent>(b => b.Property(e => e.Id).UseHiLo("BomComponentS"));

            modelBuilder.HasSequence<int>("BomComponentMaterialS");
            modelBuilder.Entity<BomComponentMaterial>(b => b.Property(e => e.Id).UseHiLo("BomComponentMaterialS"));

            modelBuilder.HasSequence<int>("BomUsageRuleS");
            modelBuilder.Entity<BomUsageRule>(b => b.Property(e => e.Id).UseHiLo("BomUsageRuleS"));

            modelBuilder.HasSequence<int>("BomRevisionHistoryS");
            modelBuilder.Entity<BomRevisionHistory>(b => b.Property(e => e.Id).UseHiLo("BomRevisionHistoryS"));

            modelBuilder.HasSequence<int>("DefectS");
            modelBuilder.Entity<Defect>(b => b.Property(e => e.Id).UseHiLo("DefectS"));

            modelBuilder.HasSequence<int>("LocationPropertiesS");
            modelBuilder.Entity<LocationProperties>(b => b.Property(e => e.Id).UseHiLo("LocationPropertiesS"));

            modelBuilder.HasSequence<int>("LocationS");
            modelBuilder.Entity<Location>(b => b.Property(e => e.Id).UseHiLo("LocationS"));

            modelBuilder.HasSequence<int>("MaterialBlacklistS");
            modelBuilder.Entity<MaterialBlacklist>(b => b.Property(e => e.Id).UseHiLo("MaterialBlacklistS"));

            modelBuilder.HasSequence<int>("MaterialHistoryS");
            modelBuilder.Entity<MaterialHistory>(b => b.Property(e => e.Id).UseHiLo("MaterialHistoryS"));

            modelBuilder.HasSequence<int>("MaterialLocationPropertiesS");
            modelBuilder.Entity<MaterialLocationProperties>(b => b.Property(e => e.Id).UseHiLo("MaterialLocationPropertiesS"));

            modelBuilder.HasSequence<int>("MaterialLocationS");
            modelBuilder.Entity<MaterialLocation>(b => b.Property(e => e.Id).UseHiLo("MaterialLocationS"));

            modelBuilder.HasSequence<int>("MaterialPropertiesS");
            modelBuilder.Entity<MaterialProperties>(b => b.Property(e => e.Id).UseHiLo("MaterialPropertiesS"));

            modelBuilder.HasSequence<int>("MaterialS");
            modelBuilder.Entity<Material>(b => b.Property(e => e.Id).UseHiLo("MaterialS"));

            modelBuilder.HasSequence<int>("OnHandMaterialS");
            modelBuilder.Entity<OnHandMaterial>(b => b.Property(e => e.Id).UseHiLo("OnHandMaterialS"));

            modelBuilder.HasSequence<int>("ReferenceDesignatorS");
            modelBuilder.Entity<ReferenceDesignator>(b => b.Property(e => e.Id).UseHiLo("ReferenceDesignatorS"));

            modelBuilder.HasSequence<int>("RepairActionS");
            modelBuilder.Entity<RepairAction>(b => b.Property(e => e.Id).UseHiLo("RepairActionS"));

            modelBuilder.HasSequence<int>("RepairGroupS");
            modelBuilder.Entity<RepairGroup>(b => b.Property(e => e.Id).UseHiLo("RepairGroupS"));

            // Notifications
            modelBuilder.HasSequence<int>("NotificationAcknowledgmentS");
            modelBuilder.Entity<NotificationAcknowledgment>(b => b.Property(e => e.Id).UseHiLo("NotificationAcknowledgmentS"));

            modelBuilder.HasSequence<int>("NotificationS");
            modelBuilder.Entity<Notification>(b => b.Property(e => e.Id).UseHiLo("NotificationS"));

            // Order Processing
            modelBuilder.HasSequence<int>("OrderHistoryS");
            modelBuilder.Entity<OrderHistory>(b => b.Property(e => e.Id).UseHiLo("OrderHistoryS"));

            modelBuilder.HasSequence<int>("OrderLinePropertiesS");
            modelBuilder.Entity<OrderLineProperties>(b => b.Property(e => e.Id).UseHiLo("OrderLinePropertiesS"));

            modelBuilder.HasSequence<int>("OrderLineS");
            modelBuilder.Entity<OrderLine>(b => b.Property(e => e.Id).UseHiLo("OrderLineS"));

            modelBuilder.HasSequence<int>("OrderPropertiesS");
            modelBuilder.Entity<OrderProperties>(b => b.Property(e => e.Id).UseHiLo("OrderPropertiesS"));

            modelBuilder.HasSequence<int>("OrderS");
            modelBuilder.Entity<Order>(b => b.Property(e => e.Id).UseHiLo("OrderS"));

            modelBuilder.HasSequence<int>("OrderTypePropertiesS");
            modelBuilder.Entity<OrderTypeProperties>(b => b.Property(e => e.Id).UseHiLo("OrderTypePropertiesS"));

            modelBuilder.HasSequence<int>("OrderTypeS");
            modelBuilder.Entity<OrderType>(b => b.Property(e => e.Id).UseHiLo("OrderTypeS"));

            modelBuilder.HasSequence<int>("PreloadPropertiesS");
            modelBuilder.Entity<PreloadProperties>(b => b.Property(e => e.Id).UseHiLo("PreloadPropertiesS"));

            modelBuilder.HasSequence<int>("PreloadS");
            modelBuilder.Entity<Preload>(b => b.Property(e => e.Id).UseHiLo("PreloadS"));

            modelBuilder.HasSequence<int>("OrderLineMapS");
            modelBuilder.Entity<OrderLineMap>(b => b.Property(e => e.Id).UseHiLo("OrderLineMapS"));

            modelBuilder.HasSequence<int>("OrderLinePackageMapS");
            modelBuilder.Entity<OrderLinePackageMap>(b => b.Property(e => e.Id).UseHiLo("OrderLinePackageMapS"));

            modelBuilder.HasSequence<int>("OrderLineRequestMapS");
            modelBuilder.Entity<OrderLineRequestMap>(b => b.Property(e => e.Id).UseHiLo("OrderLineRequestMapS"));

            // Packaging
            modelBuilder.HasSequence<int>("PackageContainmentS");
            modelBuilder.Entity<PackageContainment>(b => b.Property(e => e.Id).UseHiLo("PackageContainmentS"));

            modelBuilder.HasSequence<int>("PackageHistoryS");
            modelBuilder.Entity<PackageHistory>(b => b.Property(e => e.Id).UseHiLo("PackageHistoryS"));

            modelBuilder.HasSequence<int>("PackagePropertiesS");
            modelBuilder.Entity<PackageProperties>(b => b.Property(e => e.Id).UseHiLo("PackagePropertiesS"));

            modelBuilder.HasSequence<int>("PackageS");
            modelBuilder.Entity<Package>(b => b.Property(e => e.Id).UseHiLo("PackageS"));

            // Process Control
            modelBuilder.HasSequence<int>("DispositionS");
            modelBuilder.Entity<Disposition>(b => b.Property(e => e.Id).UseHiLo("DispositionS"));

            modelBuilder.HasSequence<int>("OperationBomMapS");
            modelBuilder.Entity<OperationBomMap>(b => b.Property(e => e.Id).UseHiLo("OperationBomMapS"));

            modelBuilder.HasSequence<int>("OperationDefectMapS");
            modelBuilder.Entity<OperationDefectMap>(b => b.Property(e => e.Id).UseHiLo("OperationDefectMapS"));

            modelBuilder.HasSequence<int>("OperationPropertiesS");
            modelBuilder.Entity<OperationProperties>(b => b.Property(e => e.Id).UseHiLo("OperationPropertiesS"));

            modelBuilder.HasSequence<int>("OperationRepairActionMapS");
            modelBuilder.Entity<OperationRepairActionMap>(b => b.Property(e => e.Id).UseHiLo("OperationRepairActionMapS"));

            modelBuilder.HasSequence<int>("OperationS");
            modelBuilder.Entity<Operation>(b => b.Property(e => e.Id).UseHiLo("OperationS"));

            modelBuilder.HasSequence<int>("RoutingS");
            modelBuilder.Entity<Routing>(b => b.Property(e => e.Id).UseHiLo("RoutingS"));

            // Setup
            modelBuilder.HasSequence<int>("ActionParameterS");
            modelBuilder.Entity<ActionParameter>(b => b.Property(e => e.Id).UseHiLo("ActionParameterS"));

            modelBuilder.HasSequence<int>("ApplicationPropertiesS");
            modelBuilder.Entity<ApplicationProperties>(b => b.Property(e => e.Id).UseHiLo("ApplicationPropertiesS"));

            modelBuilder.HasSequence<int>("ApplicationS");
            modelBuilder.Entity<Application>(b => b.Property(e => e.Id).UseHiLo("ApplicationS"));

            modelBuilder.HasSequence<int>("ApplicationRoleS");
            modelBuilder.Entity<ApplicationRole>(b => b.Property(e => e.Id).UseHiLo("ApplicationRoleS"));

            modelBuilder.HasSequence<int>("AreaS");
            modelBuilder.Entity<Area>(b => b.Property(e => e.Id).UseHiLo("AreaS"));

            modelBuilder.HasSequence<int>("BusinessS");
            modelBuilder.Entity<Business>(b => b.Property(e => e.Id).UseHiLo("BusinessS"));

            modelBuilder.HasSequence<int>("CustomerS");
            modelBuilder.Entity<Customer>(b => b.Property(e => e.Id).UseHiLo("CustomerS"));

            modelBuilder.HasSequence<int>("LabelTemplateS");
            modelBuilder.Entity<LabelTemplate>(b => b.Property(e => e.Id).UseHiLo("LabelTemplateS"));

            modelBuilder.HasSequence<int>("ProductPropertiesS");
            modelBuilder.Entity<ProductProperties>(b => b.Property(e => e.Id).UseHiLo("ProductPropertiesS"));

            modelBuilder.HasSequence<int>("ProductS");
            modelBuilder.Entity<Product>(b => b.Property(e => e.Id).UseHiLo("ProductS"));
        }

        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            ConfigureAnalyticsRelationships(modelBuilder);
            ConfigureAuthenticationRelationships(modelBuilder);
            ConfigureDevicesRelationships(modelBuilder);
            ConfigureEmployeeManagementRelationships(modelBuilder);
            ConfigureExternalIntegrationRelationships(modelBuilder);
            ConfigureInventoryControlRelationships(modelBuilder);
            ConfigureMaterialsRelationships(modelBuilder);
            ConfigureNotificationsRelationships(modelBuilder);
            ConfigureOrderProcessingRelationships(modelBuilder);
            ConfigurePackagingRelationships(modelBuilder);
            ConfigureProcessControlRelationships(modelBuilder);
            ConfigureSetupRelationships(modelBuilder);
        }

        private void ConfigureAnalyticsRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DashboardGroup>(entity =>
            {
                entity.HasMany(d => d.GroupMaps)
                    .WithOne(m => m.DashboardGroup)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(d => d.AccessMaps)
                    .WithOne(m => m.DashboardGroup)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<DashboardChart>(entity =>
            {
                entity.HasMany(d => d.Properties)
                    .WithOne(p => p.DashboardChart)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(d => d.GroupMaps)
                    .WithOne(m => m.DashboardChart)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureAuthenticationRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasOne(u => u.Person)
                    .WithMany(p => p.Users)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.UserRoles)
                    .WithOne(ur => ur.User)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.Properties)
                    .WithOne(p => p.User)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.AreaMaps)
                    .WithOne(m => m.User)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.AccessHistory)
                    .WithOne(h => h.User)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(u => u.UserImage)
                    .WithOne(e => e.User)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.LoginAttempts)
                    .WithOne(h => h.User)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasMany(r => r.UserRoles)
                    .WithOne(ur => ur.Role)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LoginAttempt>(entity =>
            {
                entity.HasOne(la => la.User)
                    .WithMany(u => u.LoginAttempts)
                    .HasForeignKey(la => la.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureDevicesRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>(entity =>
            {
                entity.HasMany(d => d.Properties)
                    .WithOne(p => p.Device)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(d => d.History)
                    .WithOne(h => h.Device)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(d => d.Consumptions)
                    .WithOne(c => c.Device)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(d => d.ParentContainments)
                    .WithOne(c => c.ParentDevice)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(d => d.ChildContainments)
                    .WithOne(c => c.ChildDevice)
                    .OnDelete(DeleteBehavior.Restrict);               
            });

            modelBuilder.Entity<DeviceHistory>(entity =>
            {
                entity.HasMany(h => h.Results)
                    .WithOne(r => r.DeviceHistory)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureEmployeeManagementRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasOne(e => e.Person)
                    .WithOne(p => p.Employee)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.JobTitle)
                    .WithMany(j => j.Employees)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Agency)
                    .WithMany(a => a.Employees)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Properties)
                    .WithOne(p => p.Employee)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.History)
                    .WithOne(h => h.Employee)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Attendances)
                    .WithOne(a => a.Employee)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasMany(d => d.JobTitles)
                    .WithOne(j => j.Department)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureExternalIntegrationRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExternalEvent>(entity =>
            {
                entity.HasMany(e => e.Properties)
                    .WithOne(p => p.ExternalEvent)
                    .OnDelete(DeleteBehavior.Cascade);

                // Configure string length constraints based on schema
                entity.Property(e => e.Direction)
                    .HasMaxLength(5);

                entity.Property(e => e.InterfaceType)
                    .HasMaxLength(20);

                entity.Property(e => e.Event)
                    .HasMaxLength(30);

                entity.Property(e => e.Process)
                    .HasMaxLength(30);

                entity.Property(e => e.ServiceName)
                    .HasMaxLength(50);

                entity.Property(e => e.CommunicationType)
                    .HasMaxLength(10);

                entity.Property(e => e.EventStatus)
                    .HasMaxLength(20);

                entity.Property(e => e.EventResponseCode)
                    .HasMaxLength(20)
                    .IsRequired(false);
            });

            modelBuilder.Entity<ExternalEventProperties>(entity =>
            {
                entity.HasOne(p => p.ExternalEvent)
                    .WithMany(e => e.Properties)
                    .OnDelete(DeleteBehavior.Cascade);

                // Configure string length constraints
                entity.Property(e => e.Tag)
                    .HasMaxLength(50);

                entity.Property(e => e.Value)
                    .HasMaxLength(255);

                entity.Property(e => e.Comments)
                    .HasMaxLength(255)
                    .IsRequired(false);
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.HasMany(r => r.Properties)
                    .WithOne(p => p.Request)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Business)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Restrict);

                // Configure string length constraints
                entity.Property(e => e.Number)
                    .HasMaxLength(30);

                entity.Property(e => e.Type)
                    .HasMaxLength(20);

                entity.Property(e => e.Status)
                    .HasMaxLength(15);
            });

            modelBuilder.Entity<RequestProperties>(entity =>
            {
                entity.HasOne(p => p.Request)
                    .WithMany(r => r.Properties)
                    .OnDelete(DeleteBehavior.Cascade);

                // Configure string length constraints
                entity.Property(e => e.Tag)
                    .HasMaxLength(50);

                entity.Property(e => e.Value)
                    .HasMaxLength(160);
            });
        }

        private void ConfigureInventoryControlRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasMany(i => i.AreaMaps)
                    .WithOne(m => m.Inventory)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(i => i.MaterialMaps)
                    .WithOne(m => m.Inventory)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(i => i.ProductMaps)
                    .WithOne(m => m.Inventory)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(i => i.Folios)
                    .WithOne(f => f.Inventory)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Folio>(entity =>
            {
                entity.HasMany(f => f.SerializedItems)
                    .WithOne(i => i.Folio)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(f => f.NonSerializedItems)
                    .WithOne(i => i.Folio)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureMaterialsRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Material>(entity =>
            {
                entity.HasMany(m => m.Properties)
                    .WithOne(p => p.Material)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(m => m.Locations)
                    .WithOne(l => l.Material)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(m => m.History)
                    .WithOne(h => h.Material)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasMany(l => l.Properties)
                    .WithOne(p => p.Location)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(l => l.MaterialLocations)
                    .WithOne(ml => ml.Location)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<MaterialLocation>(entity =>
            {
                entity.HasMany(ml => ml.Properties)
                    .WithOne(p => p.MaterialLocation)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureNotificationsRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasMany(n => n.Acknowledgments)
                    .WithOne(a => a.Notification)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureOrderProcessingRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasMany(o => o.OrderLines)
                    .WithOne(l => l.Order)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(o => o.Properties)
                    .WithOne(p => p.Order)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(o => o.History)
                    .WithOne(h => h.Order)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderLine>(entity =>
            {
                entity.HasMany(l => l.Properties)
                    .WithOne(p => p.OrderLine)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderType>(entity =>
            {
                entity.HasMany(t => t.Properties)
                    .WithOne(p => p.OrderType)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigurePackagingRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Package>(entity =>
            {
                entity.HasMany(p => p.Containments)
                    .WithOne(c => c.Package)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.Properties)
                    .WithOne(p => p.Package)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.History)
                    .WithOne(h => h.Package)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PackageContainment>(entity =>
            {
                entity.HasOne(pc => pc.Package)
                    .WithMany(p => p.Containments)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureProcessControlRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Operation>(entity =>
            {
                entity.HasMany(o => o.Properties)
                    .WithOne(p => p.Operation)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(o => o.BomMap)
                    .WithOne(b => b.Operation)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(o => o.DefectMaps)
                    .WithOne(d => d.Operation)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(o => o.RepairActionMaps)
                    .WithOne(r => r.Operation)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Routing>(entity =>
            {
                entity.HasOne(r => r.Product)
                    .WithMany(p => p.Routing)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureSetupRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Business>(entity =>
            {
                entity.HasMany(b => b.Products)
                    .WithOne(p => p.Business)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(b => b.Applications)
                    .WithOne(a => a.Business)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Application>(entity =>
            {
                entity.HasMany(a => a.ApplicationRoles)
                    .WithOne(ar => ar.Application)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasMany(p => p.Properties)
                    .WithOne(p => p.Product)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.Devices)
                    .WithOne(d => d.Product)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(p => p.DeviceBlacklist)
                    .WithOne(p => p.Product)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.Business)
                    .WithMany(b => b.Products)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Area>(entity =>
            {
                entity.HasOne(a => a.Business)
                    .WithMany(b => b.Areas)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureProperties(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeviceResults>().Property(e => e.LowerLimit).HasPrecision(18, 6);
            modelBuilder.Entity<DeviceResults>().Property(e => e.UpperLimit).HasPrecision(18, 6);
            modelBuilder.Entity<DeviceResults>().Property(e => e.MeasuredValue).HasPrecision(18, 6);

            modelBuilder.Entity<OnHandMaterial>().Property(e => e.Quantity).HasPrecision(18, 6);

            modelBuilder.Entity<Material>().Property(e => e.StdCost).HasPrecision(18, 6);

            modelBuilder.Entity<MaterialLocation>().Property(e => e.Quantity).HasPrecision(18, 6);
            modelBuilder.Entity<MaterialLocation>().Property(e => e.MinQuantity).HasPrecision(18, 6);
            modelBuilder.Entity<MaterialLocation>().Property(e => e.MaxQuantity).HasPrecision(18, 6);

            modelBuilder.Entity<MaterialHistory>().Property(e => e.Quantity).HasPrecision(18, 6);
        }

        #region DbSet Declarations
        // Analytics
        public DbSet<DashboardGroup> DashboardGroups { get; set; }
        public DbSet<DashboardChart> DashboardCharts { get; set; }
        public DbSet<DashboardChartProperties> DashboardChartProperties { get; set; }
        public DbSet<AccessDashboardMap> AccessDashboardMaps { get; set; }
        public DbSet<DashboardGroupMap> DashboardGroupMaps { get; set; }

        // Authentication
        public DbSet<Person> Persons { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserProperties> UserProperties { get; set; }
        public DbSet<UserImage> UserImages { get; set; }
        public DbSet<UserAreaMap> UserAreaMaps { get; set; }
        public DbSet<AccessHistory> AccessHistory { get; set; }
        public DbSet<ExternalUser> ExternalUsers { get; set; }
        public DbSet<LoginAttempt> LoginAttempts { get; set; }

        // Devices
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceProperties> DeviceProperties { get; set; }
        public DbSet<DeviceHistory> DeviceHistory { get; set; }
        public DbSet<DeviceResults> DeviceResults { get; set; }
        public DbSet<DeviceConsumption> DeviceConsumption { get; set; }
        public DbSet<Containment> Containment { get; set; }
        public DbSet<DeviceBlacklist> DeviceBlacklist { get; set; }

        // Employee Management
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<AgencyProperties> AgencyProperties { get; set; }
        public DbSet<Clock> Clocks { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeAttendance> EmployeeAttendance { get; set; }
        public DbSet<EmployeeHistory> EmployeeHistory { get; set; }
        public DbSet<EmployeeProperties> EmployeeProperties { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }

        // External Integration
        public DbSet<ExternalEvent> ExternalEvents { get; set; }
        public DbSet<ExternalEventProperties> ExternalEventProperties { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestProperties> RequestProperties { get; set; }

        // Inventory Control
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryAreaMap> InventoryAreaMaps { get; set; }
        public DbSet<InventoryMaterialMap> InventoryMaterialMaps { get; set; }
        public DbSet<InventoryProductMap> InventoryProductMaps { get; set; }
        public DbSet<Folio> Folios { get; set; }
        public DbSet<FolioSerialized> FolioSerialized { get; set; }
        public DbSet<FolioNonSerialized> FolioNonSerialized { get; set; }

        // Materials
        public DbSet<Material> Materials { get; set; }
        public DbSet<MaterialProperties> MaterialProperties { get; set; }
        public DbSet<MaterialLocation> MaterialLocations { get; set; }
        public DbSet<MaterialLocationProperties> MaterialLocationProperties { get; set; }
        public DbSet<MaterialHistory> MaterialHistory { get; set; }
        public DbSet<MaterialBlacklist> MaterialBlacklist { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationProperties> LocationProperties { get; set; }
        public DbSet<OnHandMaterial> OnHandMaterials { get; set; }
        public DbSet<ReferenceDesignator> ReferenceDesignators { get; set; }
        public DbSet<RepairGroup> RepairGroups { get; set; }
        public DbSet<RepairAction> RepairActions { get; set; }
        public DbSet<Defect> Defects { get; set; }
        public DbSet<BomHeader> BomHeader { get; set; }
        public DbSet<BomComponent> BomComponents { get; set; }
        public DbSet<BomComponentMaterial> BomComponentMaterials { get; set; }
        public DbSet<BomUsageRule> BomUsageRules { get; set; }
        public DbSet<BomRevisionHistory> BomRevisionHistory { get; set; }

        // Notifications
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationAcknowledgment> NotificationAcknowledgments { get; set; }

        // Order Processing
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderType> OrderTypes { get; set; }
        public DbSet<OrderTypeProperties> OrderTypeProperties { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<OrderLineProperties> OrderLineProperties { get; set; }
        public DbSet<OrderProperties> OrderProperties { get; set; }
        public DbSet<OrderHistory> OrderHistory { get; set; }
        public DbSet<Preload> Preloads { get; set; }
        public DbSet<PreloadProperties> PreloadProperties { get; set; }

        // Packaging
        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageContainment> PackageContainments { get; set; }
        public DbSet<PackageProperties> PackageProperties { get; set; }
        public DbSet<PackageHistory> PackageHistory { get; set; }

        // Process Control
        public DbSet<Disposition> Dispositions { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<OperationBomMap> OperationBomMaps { get; set; }
        public DbSet<OperationDefectMap> OperationDefectMaps { get; set; }
        public DbSet<OperationProperties> OperationProperties { get; set; }
        public DbSet<OperationRepairActionMap> OperationRepairActionMaps { get; set; }
        public DbSet<Routing> Routings { get; set; }

        // Setup
        public DbSet<ActionParameter> ActionParameters { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<ApplicationProperties> ApplicationProperties { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Business> Business { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<LabelTemplate> LabelTemplates { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductProperties> ProductProperties { get; set; }
        #endregion
    }
}
