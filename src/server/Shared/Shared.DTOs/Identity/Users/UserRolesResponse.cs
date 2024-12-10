using System.Collections.Generic;

namespace Shared.DTOs.Identity.Users
{
    public class UserRolesResponse
    {
        public List<UserRoleModel> UserRoles { get; set; } = new();
    }

    public class UserRoleModel
    {
        public string RoleId { get; set; }

        public string RoleName { get; set; }

        public bool Selected { get; set; }
    }
}