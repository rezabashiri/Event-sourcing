using Shared.Core.Domain;

namespace Modules.EmployeeManagement.Core.Entities
{
    /// <summary>
    /// Represent all staffs in company
    /// </summary>
    public class StaffMember : BaseEntity
    {

        public string NationalCode { get; set; }

        public string Name { get; set; }

        public string Family { get; set; }
        public DateTime EmployeedAt { get; set; }

        public ICollection<StaffTask> StaffTasks { get; set; } = new HashSet<StaffTask>();

        public static StaffMember InitializeStaff() => new StaffMember() { EmployeedAt = DateTime.UtcNow };

        public void AddTaskToStaff(IEnumerable<StaffTask> tasks)
        {
            foreach (var staffTask in tasks)
            {
                StaffTasks.Add(staffTask);
            }
        }
    }
}