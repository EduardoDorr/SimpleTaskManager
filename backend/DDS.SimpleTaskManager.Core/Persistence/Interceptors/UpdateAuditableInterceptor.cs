using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DDS.SimpleTaskManager.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DDS.SimpleTaskManager.Core.Persistence.Interceptors;

public sealed class UpdateAuditableInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
            return await base.SavingChangesAsync(eventData, result, cancellationToken);

        UpdateUpdatedAtAsync(eventData.Context);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateUpdatedAtAsync(DbContext context)
    {
        var entries = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(ct => ct.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            entry.Entity.SetUpdatedAtDate(DateTime.UtcNow);
        }
    }
}