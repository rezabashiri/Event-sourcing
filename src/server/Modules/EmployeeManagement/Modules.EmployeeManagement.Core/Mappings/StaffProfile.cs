using AutoMapper;
using Modules.EmployeeManagement.Core.Entities;
using Modules.EmployeeManagement.Core.Features.Staff.Commands;

namespace Modules.EmployeeManagement.Core.Mappings;
public class StaffProfile : Profile
{
    public StaffProfile()
    {
        this.CreateMap<EmployStaffCommand, StaffMember>().ReverseMap();
    }
}