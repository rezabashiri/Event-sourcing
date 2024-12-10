using Microsoft.EntityFrameworkCore;
using Modules.EmployeeManagement.Core.Entities;
using Shared.Core.Interfaces;

namespace Modules.EmployeeManagement.Core.Abstractions;
public interface IStaffManagementDbContext:IDbContext
{
    DbSet<StaffMember>  StaffMembers { get; set; }
    StaffTask StaffTasks { get; set; }
}