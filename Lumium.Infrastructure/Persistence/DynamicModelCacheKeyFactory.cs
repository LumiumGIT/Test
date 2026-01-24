using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Lumium.Infrastructure.Persistence;

public class DynamicModelCacheKeyFactory : IModelCacheKeyFactory
{
    public object Create(DbContext context, bool designTime = false)
    {
        if (context is ApplicationDbContext appContext)
        {
            return (context.GetType(), appContext.GetTenantId(), designTime);
        }

        return (context.GetType(), designTime);
    }
}