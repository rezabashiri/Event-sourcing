using System.Collections.Generic;

namespace Shared.DTOs.Identity.Users
{
    public class UserRolesRequest
    {
        public List<UserRoleModel> UserRoles { get; set; } = new();
    }
}