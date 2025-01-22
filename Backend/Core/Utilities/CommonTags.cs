using Artemis.Backend.Core.Models.Materials;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client.Extensions.Msal;
using OfficeOpenXml.Packaging.Ionic.Zlib;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Net.Sockets;
using System.Numerics;
using System.Reflection;
using System.Threading;

namespace Artemis.Backend.Core.Utilities
{
    public static class CommonTags
    {
        /// <summary>
        /// Common status constants used throughout the application
        /// </summary>
        public const string Success = "Success";
        public const string Fail = "Fail";
        public const string Active = "Active";
        public const string Inactive = "Inactive";
        public const string Available = "Available";
        public const string Suspended = "Suspended";
        public const string Found = "Found";
        public const string NotFound = "NotFound";
        public const string Granted = "Granted";
        public const string Denied = "Denied";
        public const string Obsolete = "Obsolete";
        public const string Reserved = "Reserved";
        public const string OnHold = "OnHold";
        public const string Removed = "Removed";
        public const string QualityHold = "QualityHold";

        /// <summary>
        /// Manufacturing Types
        /// </summary>
        public const string Manufacturing = "Manufacturing";
        public const string Service = "Service";
        public const string Repair = "Repair";
        public const string Maintenance = "Maintenance";
        public const string Engineering = "Engineering";

        /// <summary>
        /// Order-related status constants
        /// </summary>
        public const string Created = "Created";
        public const string Added = "Added";
        public const string Deleted = "Deleted";
        public const string Canceled = "Canceled";
        public const string Updated = "Updated";
        public const string Delivered = "Delivered";
        public const string Received = "Received";
        public const string Completed = "Completed";
        public const string InProgress = "InProgress";
        public const string Open = "Open";
        public const string InProcess = "InProcess";
        public const string Cancelled = "Cancelled";
        public const string Closed = "Closed";
        public const string Shipped = "Shipped";

        public const string Packed = "Packed";
        public const string Palletized = "Palletized";
        public const string Fulfilled = "Fulfilled";
        public const string Draft = "Draft";
        public const string ToCancel = "statusToCancel";
        public const string ToUpdate = "statusToUpdate";
        public const string ToDelivery = "statusToDelivery";
        public const string ToReceive = "statusToReceive";

        /// <summary>
        /// Authentication and user-related constants
        /// </summary>
        public const string UserInfo = "userInfo";
        public const string User = "user";
        public const string Password = "password";
        public const string UserId = "UserId";
        public const string PublicUser = "Public";
        public const string PartialLogin = "Partial";
        public const string Locked = "Locked";
        public const string Internal = "Internal";
        public const string External = "External";

        /// <summary>
        /// Application type constants
        /// </summary>
        public const string Admin = "Admin";
        public const string Report = "Report";
        public const string Setup = "Setup";
        public const string Normal = "Normal";
        public const string Multiple = "Multiple";
        public const string Backend = "Backend";
        public const string Frontend = "Frontend";
        public const string CompositeBackend = "_COMPOSITE_Backend";
        public const string CompositeFrontend = "_COMPOSITE_Frontend";

        /// <summary>
        /// Template-related constants
        /// </summary>
        public const string Base = "template";
        public const string TemplateName = "templateName";
        public const string Type = "templateType";
        public const string Group = "templateGroup";
        public const string Optional = "[Optional]";
        public const string Mandatory = "[Mandatory]";
        public const string Kitting = "kittingLabelTemplate";
        public const string Pack = "packLabelTemplate";
        public const string PalletLabelTemplate = "palletLabelTemplate";
        public const string ProcessLabelTemplate = "processLabelTemplate";
        public const string Order = "orderLabelTemplate";
        public const string Login = "LoginLabelTemplate";
        public const string Fulfill = "fulfillLabelTemplate";

        /// <summary>
        /// Request-related constants
        /// </summary>
        public const string RequestId = "RequestId";
        public const string AreaFrom = "AreaFrom";
        public const string AreaTo = "AreaTo";
        public const string RequestName = "name";
        public const string LastName = "lastName";
        public const string Date = "Date";
        public const string Comments = "comments";
        public const string Delivery = "delivery";
        public const string Receiving = "receiving";

        /// <summary>
        /// File transfer protocol constants
        /// </summary>
        public const string FTPPassword = "ftpPassword";
        public const string FTPUser = "ftpUser";
        public const string FTPPath = "ftpPath";
        public const string FTPPort = "ftpPort";
        public const string FTPServer = "ftpServer";
        public const string FTPType = "ftpType";
        public const string Unread = "Unread";
        public const string Read = "Read";
        public const string Archived = "Archived";

        /// <summary>
        /// Email-related constants
        /// </summary>
        public const string From = "emailAddressFrom";
        public const string To = "emailAddressTo";
        public const string Cc = "emailAddressToCC";
        public const string Bcc = "emailAddressToBCC";
        public const string Body = "emailBody";
        public const string Subject = "emailSubject";

        /// <summary>
        /// Devices constants
        /// </summary>
        public const string New = "New";
        public const string Assembled = "Assembled";
        public const string Disassembled = "Disassembled";
        public const string Replaced = "Replaced";
        public const string Failed = "Failed";
        public const string Moved = "Moved";
        public const string Rework = "Rework";

        /// <summary>
        /// Process verification constants
        /// </summary>
        public const string ProcessVerifFlag = "ProcessVerifFlag";
        public const string AssignGradeFlag = "AssignGradeFlag";
        public const string MaterialMoveFlag = "MaterialMoveFlag";
        public const string PrintLabel = "PrintLabel";
        public const string PrintSheet = "PrintSheet";
        public const string SaveSheet = "SaveSheet";
        public const string Device = "Device";
        public const string Carton = "Carton";
        public const string Pallet = "Pallet";
        public const string Box = "Box";
        public const string Container = "Container";
        public const string Full = "Full";

        /// <summary>
        /// Pallet-related constants
        /// </summary>
        public const string Prefix = "palletPrefix";
        public const string NameLength = "palletNameLength";
        public const string Suffix = "palletSuffix";
        public const string Sequence = "palletSequence";
        public const string PalletizeInfo = "paletizeInfo";
        public const string AllowMultipleProducts = "allowMultipleProducts";
        public const string SetConfigInProduct = "setPalletConfigInProduct";
        public const string Status = "palletStatus";

        /// <summary>
        /// Warehouse Location constants 
        /// </summary>
        public const string Shelf = "Shelf";
        public const string Bin = "Bin";
        public const string Rack = "Rack";
        public const string Floor = "Floor";
        public const string Storage = "Storage";
        public const string Line = "Line";
        public const string Quantity = "Quantity";
        public const string Configuration = "Configuration";
        public const string Process = "Process";
        public const string Quality = "Quality";

        /// <summary>
        /// HR-related constants
        /// </summary>
        public const string Business = "HR";
        public const string NewHire = "NEW_HIRE";
        public const string PrintCard = "printCard";
        public const string String = "String";
        public const string Picture = "Picture";
        public const string Document = "Document";
        public const string Pdf = "PDF";
        public const string EmployeeType = "employeeType";
        public const string Present = "Present";
        public const string Absent = "Absent";
        public const string Leave = "Leave";
        public const string Holiday = "Holiday";
        public const string Weekend = "Weekend";

        /// <summary>
        /// Notification constants
        /// </summary>
        public const string System = "System";
        public const string Alert = "Alert";
        public const string Warning = "Warning";
        public const string Info = "Info";
        public const string Error = "Error";
        public const string In = "In";
        public const string Out = "Out";
        public const string Rest = "Rest";
        public const string Soap = "Soap";
        public const string File = "File";
        public const string Mq = "Mq";
        public const string Submitted = "Submitted";
        public const string Material = "Material";
        public const string Movement = "Movement";

        /// <summary>
        /// Station Modules
        /// </summary>
        #region Station Modules
        // Setup
        public const string DataFeed = "DataFeed";
        // Process
        public const string Initialize = "Initialize";
        public const string Tracking = "Tracking";
        public const string Inspection = "Inspection";        
        public const string Kit = "Kit";
        public const string Diagnostic = "Diagnostic";
        public const string PassFail = "PassFail";
        public const string Packing = "Packing";
        public const string Shipping = "Shipping";

        // List of valid icon format prefixes
        public static readonly List<string> StationModules =
        [
            // Setup Modules
            DataFeed,
            // Process Modules
            Initialize,
            Tracking,
            Inspection,
            Kit,
            Diagnostic,
            PassFail,
            Packing,
            Shipping
        ];
        #endregion

        /// <summary>
        /// Common List of Miscelaneous
        /// </summary>
        #region Miscellaneous
        // Icon prefixes
        public const string FontAwesomeSolid = "fas";
        public const string FontAwesomeRegular = "far";
        public const string FontAwesomeBrands = "fab";
        public const string MaterialIcons = "mat";
        public const string CustomIcon = "custom";

        // Regex pattern for icon validation
        public const string IconPattern = @"^(fas|far|fab|mat|custom):.+$";

        // List of valid icon format prefixes
        public static IReadOnlyCollection<string> IconPrefixes =>
        [
            FontAwesomeSolid,
            FontAwesomeRegular,
            FontAwesomeBrands,
            MaterialIcons,
            CustomIcon
        ];
        #endregion

        /// <summary>
        /// Common List of Table Statuses
        /// </summary>
        #region Status Lists
        public static IReadOnlyCollection<string> BusinessStatuses => [Active, Inactive, Suspended];
        public static IReadOnlyCollection<string> BusinessTypes => [Manufacturing, Service, Repair];

        public static IReadOnlyCollection<string> ApplicationStatuses => [Active, Inactive, Obsolete];
        public static IReadOnlyCollection<string> ApplicationTypes => [Admin, User, Report, Setup, Process];

        public static IReadOnlyCollection<string> ProductStatuses => [Active, Inactive, Obsolete];
        public static IReadOnlyCollection<string> OperationalStatuses => [Active, Inactive, Maintenance];

        public static IReadOnlyCollection<string> PersonStatuses => [Active, Inactive, Suspended, Deleted];
        public static IReadOnlyCollection<string> UserStatuses => [Active, Inactive, Suspended, Locked, Deleted];
        public static IReadOnlyCollection<string> UserTypes => [Internal, External];
        public static IReadOnlyCollection<string> EmployeeStatuses => [Present, Absent, Leave, Holiday, Weekend];

        public static IReadOnlyCollection<string> RoleStatuses => [Active, Inactive];
        public static IReadOnlyCollection<string> LocationStatuses => [Active, Inactive, Full];
        public static IReadOnlyCollection<string> LocationTypes => [Shelf, Bin, Rack, Floor, Storage, Line];

        public static IReadOnlyCollection<string> MaterialStatuses => [Active, Inactive, Obsolete];
        public static IReadOnlyCollection<string> MaterialLocationStatuses => [Active, Inactive, Maintenance, Full];
        public static IReadOnlyCollection<string> BomStatuses => [Draft, Active, Inactive, Obsolete];
        public static IReadOnlyCollection<string> BomTypes => [Engineering, Manufacturing, Service];
        public static IReadOnlyCollection<string> BomComponentStatuses => [Active, Inactive];
        public static IReadOnlyCollection<string> BomUsageRuleTypes => [Quantity, Configuration, Process, Quality];
        public static IReadOnlyCollection<string> OnHandMaterialStatuses => [Available, Reserved, OnHold, QualityHold];

        public static IReadOnlyCollection<string> OrderStatuses => [Open, InProcess, Completed, Cancelled];
        public static IReadOnlyCollection<string> PackageStatuses => [Open, Closed, Shipped, Received];
        public static IReadOnlyCollection<string> PackageTypes => [Box, Carton, Pallet, Container];
        public static IReadOnlyCollection<string> PackageContainmentStatuses => [Active, Removed];

        public static IReadOnlyCollection<string> DeviceStatuses => [New, InProcess, Completed, Failed, Shipped];
        public static IReadOnlyCollection<string> DeviceConsumptionStatuses => [Completed, Failed];
        public static IReadOnlyCollection<string> ContainmentStatuses => [Active, Removed, Replaced];
        public static IReadOnlyCollection<string> DeviceHistoryStatuses => [InProcess, Disassembled, Completed, Failed, Rework];
        public static IReadOnlyCollection<string> DeviceHistoryActions => [Assembled, Disassembled, Replaced, Failed, Moved];

        public static IReadOnlyCollection<string> InventoryStatuses => [Open, Closed, InProgress];

        public static IReadOnlyCollection<string> NotificationStatuses => [Unread, Read, Archived, Deleted];
        public static IReadOnlyCollection<string> NotificationTypes => [System, Alert, Warning, Info, Error];

        public static IReadOnlyCollection<string> ExternalEventDirections => [In, Out];
        public static IReadOnlyCollection<string> ExternalEventTypes => [Rest, Soap, File, Mq];
        public static IReadOnlyCollection<string> ExternalEventStatuses => [New, InProcess, Completed, Failed];
        public static IReadOnlyCollection<string> RequestStatuses => [Draft, Submitted, InProcess, Completed, Cancelled];
        public static IReadOnlyCollection<string> RequestTypes => [Material, Movement, Quality, Maintenance];
        #endregion
    }
}
