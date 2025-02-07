using Bat.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CryptoGateway.DataAccess.DbContexts.Extensions;

public static class DbContextExtensions
{
    public static void BasePropertiesInitializer(this DbContext dbContext)
    {
        foreach (EntityEntry<IBaseProperties> item in dbContext.ChangeTracker.Entries<IBaseProperties>())
        {
            switch (item.State)
            {
                case EntityState.Added:
                    if (item.Entity is IInsertDateProperty insertDateProperty)
                    {
                        insertDateProperty.InsertDateMi = DateTime.Now;
                    }

                    if (item.Entity is IInsertDateProperties insertDateProperties)
                    {
                        insertDateProperties.InsertDateMi = DateTime.Now;
                        insertDateProperties.InsertDateSh = PersianDateTime.Now.ToString();
                    }

                    if (item.Entity is IModifyDateProperty modifyDateProperty)
                    {
                        modifyDateProperty.ModifyDateMi = DateTime.Now;
                    }

                    if (item.Entity is IModifyDateProperties modifyDateProperties)
                    {
                        modifyDateProperties.ModifyDateMi = DateTime.Now;
                        modifyDateProperties.ModifyDateSh = PersianDateTime.Now.ToString();
                    }

                    break;
                case EntityState.Modified:
                    if (item.Entity is IModifyDateProperty modifyDateProperty2)
                    {
                        modifyDateProperty2.ModifyDateMi = DateTime.Now;
                    }

                    if (item.Entity is IModifyDateProperties modifyDateProperties2)
                    {
                        modifyDateProperties2.ModifyDateMi = DateTime.Now;
                        modifyDateProperties2.ModifyDateSh = PersianDateTime.Now.ToString();
                    }

                    break;
                case EntityState.Deleted:
                    if (item.Entity is ISoftDeleteProperty softDeleteProperty)
                    {
                        softDeleteProperty.IsDeleted = true;
                        item.State = EntityState.Modified;
                    }

                    break;
            }
        }
    }
}