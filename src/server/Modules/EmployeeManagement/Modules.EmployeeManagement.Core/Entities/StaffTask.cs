using Shared.Core.Domain;

namespace Modules.EmployeeManagement.Core.Entities
{
    /// <summary>
    /// All tasks assigned to a single staff
    /// </summary>
    public class StaffTask: BaseEntity
    {
        public Guid StaffId { get; set; }

        public StaffMember StaffMember { get; set; }

        public string Name { get; set; }

        public int Priority { get; set; }

        public DateTime Start { get; set; }

        public DateTime Due { get; set; }

    }
}