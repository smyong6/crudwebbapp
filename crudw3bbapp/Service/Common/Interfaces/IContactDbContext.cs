
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Service.Common.Interfaces;

public interface IContactDbContext
{
    DbSet<Contact> Contacts { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
