using Modules.EmployeeManagement.Core.Entities;
using Shared.Core.Domain;

namespace Modules.EmployeeManagement.Core.Features.Staff.Events;

public record StaffEmployedEvent : Event
{

    public StaffEmployedEvent(StaffMember staffMember)
    {
        Id = staffMember.Id;
        FullName = $"{staffMember.Name} {staffMember.Family}";
        RelatedEntities = new[]
        {
            typeof(StaffMember)
        };
    }
    public Guid Id { get; }

    public string FullName { get; }
}