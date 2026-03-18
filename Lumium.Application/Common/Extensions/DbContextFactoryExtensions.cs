using Lumium.Application.Common.Interfaces;

namespace Lumium.Application.Common.Extensions;

public static class DbContextFactoryExtensions
{
    extension(IApplicationDbContextFactory factory)
    {
        public async Task<TResult> ExecuteInContextAsync<TResult>(Func<IApplicationDbContext, Task<TResult>> operation,
            CancellationToken cancellationToken = default)
        {
            await using var context = await factory.CreateDbContextAsync(cancellationToken);
            return await operation(context);
        }

        public async Task ExecuteInContextAsync(Func<IApplicationDbContext, Task> operation,
            CancellationToken cancellationToken = default)
        {
            await using var context = await factory.CreateDbContextAsync(cancellationToken);
            await operation(context);
        }
    }
}