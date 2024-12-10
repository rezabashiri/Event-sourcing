using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Modules.EmployeeManagement.Core.Abstractions;
using Modules.EmployeeManagement.Core.Exception;
using Modules.EmployeeManagement.Core.Features.Staff.Events;
using Shared.Core.Wrapper;

namespace Modules.EmployeeManagement.Core.Features.Staff.Commands;
public class StaffCommandHandler : IRequestHandler<EmployStaffCommand, Result<Guid>>
{
    private readonly IStringLocalizer<StaffCommandHandler> _localizer;
    private readonly IStaffManagementDbContext _staffManagementDbContext;
    private readonly IMapper _mapper;

    public StaffCommandHandler(IStringLocalizer<StaffCommandHandler> localizer, IStaffManagementDbContext staffManagementDbContext,IMapper mapper)
    {
        _localizer = localizer;
        _staffManagementDbContext = staffManagementDbContext;
        _mapper = mapper;
    }

    public async Task<Result<Guid>> Handle(EmployStaffCommand request, CancellationToken cancellationToken)
    {
        if (await _staffManagementDbContext.StaffMembers.AnyAsync(x => x.NationalCode == request.NationalCode))
            throw new StaffException(_localizer["Staff with the same nationalcode already exists."],HttpStatusCode.BadRequest);

        var toAdd = _mapper.Map<Entities.StaffMember>(request);


        await _staffManagementDbContext.StaffMembers.AddAsync(toAdd);
        toAdd.AddDomainEvent(new StaffEmployedEvent(toAdd));
        await _staffManagementDbContext.SaveChangesAsync(cancellationToken);

        return await Result<Guid>.SuccessAsync(toAdd.Id);
    }
}