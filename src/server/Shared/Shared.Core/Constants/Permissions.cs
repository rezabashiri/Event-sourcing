using System.ComponentModel;

namespace Shared.Core.Constants
{
    public static class Permissions
    {
        [DisplayName("Users")]
        [Description("Users Permissions")]
        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
        }

        [DisplayName("Users Extended Attributes")]
        [Description("Users Extended Attributes Permissions")]
        public static class UsersExtendedAttributes
        {
            public const string View = "Permissions.Users.ExtendedAttributes.View";
            public const string ViewAll = "Permissions.Users.ExtendedAttributes.ViewAll";
            public const string Add = "Permissions.Users.ExtendedAttributes.Add";
            public const string Update = "Permissions.Users.ExtendedAttributes.Update";
            public const string Remove = "Permissions.Users.ExtendedAttributes.Remove";
        }

        [DisplayName("Roles")]
        [Description("Roles Permissions")]
        public static class Roles
        {
            public const string View = "Permissions.Roles.View";
            public const string Create = "Permissions.Roles.Create";
            public const string Edit = "Permissions.Roles.Edit";
            public const string Delete = "Permissions.Roles.Delete";
        }

        [DisplayName("Roles Extended Attributes")]
        [Description("Roles Extended Attributes Permissions")]
        public static class RolesExtendedAttributes
        {
            public const string View = "Permissions.Roles.ExtendedAttributes.View";
            public const string ViewAll = "Permissions.Roles.ExtendedAttributes.ViewAll";
            public const string Add = "Permissions.Roles.ExtendedAttributes.Add";
            public const string Update = "Permissions.Roles.ExtendedAttributes.Update";
            public const string Remove = "Permissions.Roles.ExtendedAttributes.Remove";
        }

        [DisplayName("Role Claims")]
        [Description("Role Claims Permissions")]
        public static class RoleClaims
        {
            public const string View = "Permissions.RoleClaims.View";
            public const string Create = "Permissions.RoleClaims.Create";
            public const string Edit = "Permissions.RoleClaims.Edit";
            public const string Delete = "Permissions.RoleClaims.Delete";
        }

        [DisplayName("Event Logs")]
        [Description("Event Logs Permissions")]
        public static class EventLogs
        {
            public const string ViewAll = "Permissions.EventLogs.ViewAll";
            public const string Create = "Permissions.EventLogs.Create";
        }

    }
}