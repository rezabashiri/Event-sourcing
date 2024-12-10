using MediatR;
using Shared.Core.Wrapper;

namespace Modules.EmployeeManagement.Core.Features.Staff.Commands;
public record EmployStaffCommand(string NationalCode, string Name, string Family, DateTime EmployedAt) : IRequest<Result<Guid>>;