using Microsoft.EntityFrameworkCore;
using Modules.Accounting.Domain.Entities;
using Shared.Core.Interfaces;

namespace Modules.Accounting.Domain.Abstractions;

public interface IAccountingDbContext : IDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
}