using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Data;

public partial class ApplicationDbContext
{
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return SaveChangesAsync(true, cancellationToken);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken)
    {
        var dateTimeUtcNow = DateTime.UtcNow;
        var trackChangeGuid = Guid.NewGuid();
        UpdateAuditableValues(dateTimeUtcNow);
        var newAddedEntities = GetAddedEntities();

        var changesAsync = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        if (newAddedEntities.Count > 0)
        {
            await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        return changesAsync;
    }

    public override int SaveChanges()
    {
        if (Database.ProviderName is not null && Database.ProviderName.Contains("Sqlite"))
        {
            return SaveChanges(true);
        }
        throw new AppException("Use SaveChangesAsync instead.", AppStatusCodeError.InternalServerError500);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        if (Database.ProviderName is not null && Database.ProviderName.Contains("Sqlite"))
        {
            var dateTimeUtcNow = DateTime.UtcNow;
            var trackChangeGuid = Guid.NewGuid();
            UpdateAuditableValues(dateTimeUtcNow);
            var newAddedEntities = GetAddedEntities();
            var changes = base.SaveChanges(acceptAllChangesOnSuccess);
            if (newAddedEntities.Count > 0)
            {
                changes = base.SaveChanges(acceptAllChangesOnSuccess);
            }
            return changes;
        }
        throw new AppException("Use SaveChangesAsync instead.", AppStatusCodeError.InternalServerError500);
    }

    private void UpdateAuditableValues(DateTime dateTimeUtcNow)
    {
        var userName = (Database.ProviderName is not null && Database.ProviderName.Contains("Sqlite"))
           ? "Unit/Integration Tests"
           : this.GetService<IHttpContextAccessor>().HttpContext?.User.UserName();

        var userId = (Database.ProviderName is not null && Database.ProviderName.Contains("Sqlite"))
          ? "0"
          : this.GetService<IHttpContextAccessor>().HttpContext?.User.UserId();

        var userType = (Database.ProviderName is not null && Database.ProviderName.Contains("Sqlite"))
          ? "0"
          : this.GetService<IHttpContextAccessor>().HttpContext?.User.UserType();

        var createdFromEntitiesChanged = ChangeTracker.Entries().Where(x => x.Entity is AuditableCreatedFrom).Where(x => x.State == EntityState.Added).ToList();

        foreach (var createdFromEntityChanged in createdFromEntitiesChanged)
        {
            if (createdFromEntityChanged.State == EntityState.Added)
            {
                HandleAuditableCreatedFrom(createdFromEntityChanged, Convert.ToByte(userType), Convert.ToInt32(userId), dateTimeUtcNow);
            }
            else if (createdFromEntityChanged.State == EntityState.Modified)
            {
                HandleAuditableModifiedCreatedFrom(createdFromEntityChanged, dateTimeUtcNow);
            }
        }

        var entitiesChanged = ChangeTracker
            .Entries()
            .Where(x => x.Entity is Auditable || x.Entity is User || IsInstanceOfGenericType(x.Entity, typeof(Auditable<>)))
            .Where(x =>
                x.State == EntityState.Added ||
                x.State == EntityState.Modified ||
                x.State == EntityState.Deleted);

        if (!entitiesChanged.Any())
        {
            return;
        }



        foreach (var entityEntry in entitiesChanged)
        {
            (entityEntry.State switch
            {
                EntityState.Added => HandleAuditableAdded,
                EntityState.Modified => HandleAuditableModified,
                EntityState.Deleted => HandleAuditableDelete,
                _ => (Action<EntityEntry, string?, int, DateTime>)((_, _, _, _) => { })
            })(entityEntry, userName, Convert.ToInt32(userId), dateTimeUtcNow);
        }
    }

    private void HandleAuditableAdded(EntityEntry entry, string? userName, int userId, DateTime dateTimeUtcNow)
    {
        entry.Property(nameof(Auditable.CreatedAt)).CurrentValue = dateTimeUtcNow;
        entry.Property(nameof(Auditable.UpdateAt)).CurrentValue = dateTimeUtcNow;
        entry.Property(nameof(Auditable.CreatedBy)).CurrentValue = userId;
    }

    private void HandleAuditableModified(EntityEntry entry, string? userName, int userId, DateTime dateTimeUtcNow)
    {


        entry.Property(nameof(Auditable.CreatedAt)).IsModified = false;
        entry.Property(nameof(Auditable.CreatedBy)).IsModified = false;
        entry.Property(nameof(Auditable.UpdateAt)).CurrentValue = dateTimeUtcNow;
    }

    private void HandleAuditableDelete(EntityEntry entry, string? userName, int userId, DateTime dateTimeUtcNow)
    {
        entry.Property(nameof(Auditable.DeletedAt)).CurrentValue = dateTimeUtcNow;
        entry.State = EntityState.Modified;
    }

    private void HandleAuditableCreatedFrom(EntityEntry entry, byte CreatedFrom, int userId, DateTime dateTimeUtcNow)
    {
        entry.Property(nameof(Auditable.CreatedAt)).CurrentValue = dateTimeUtcNow;
        entry.Property(nameof(Auditable.UpdateAt)).CurrentValue = dateTimeUtcNow;
        entry.Property(nameof(Auditable.CreatedBy)).CurrentValue = userId;
        entry.Property(nameof(AuditableCreatedFrom.CreatedFrom)).CurrentValue = CreatedFrom;
    }

    private void HandleAuditableModifiedCreatedFrom(EntityEntry entry, DateTime dateTimeUtcNow)
    {
        entry.Property(nameof(Auditable.CreatedAt)).IsModified = false;
        entry.Property(nameof(Auditable.CreatedBy)).IsModified = false;
        entry.Property(nameof(AuditableCreatedFrom.CreatedFrom)).IsModified = false;
        entry.Property(nameof(Auditable.UpdateAt)).CurrentValue = dateTimeUtcNow;

    }

    private static bool IsInstanceOfGenericType(object instance, Type genericType)
    {
        Type? type = instance.GetType();
        while (type != null)
        {
            if (type.IsGenericType &&
                type.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }
            type = type.BaseType;
        }
        return false;
    }


    private List<EntityEntry> GetAddedEntities()
    {
        List<EntityEntry> newAddedEntries = new List<EntityEntry>();
        var entitiesChanged = ChangeTracker
           .Entries()
           .Where(x => x.State == EntityState.Added);

        if (!entitiesChanged.Any())
        {
            return newAddedEntries;
        }

        var trackChangeGuid = Guid.NewGuid();
        foreach (var change in entitiesChanged)
        {
            newAddedEntries.Add(change);
        }

        return newAddedEntries;
    }


    public int TestCaseSaveChanges()
    {
        return base.SaveChanges(true);
    }
}
