using Medallion.Threading.Redis;
using StackExchange.Redis;

namespace Wta.Infrastructure.Locking;

public class DistributedLock : ILock, IDisposable
{
    private bool disposedValue;

    public DistributedLock(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("redis") ?? "127.0.0.1:6379";
        Connection = ConnectionMultiplexer.ConnectAsync(connectionString).GetAwaiter().GetResult();
    }

    public ConnectionMultiplexer Connection { get; }

    public async Task<IDisposable?> Acquire(string key, TimeSpan timeout = default)
    {
        var @lock = new RedisDistributedLock(key, Connection.GetDatabase());
        return await @lock.AcquireAsync(timeout).ConfigureAwait(false);
    }

    public async Task<IDisposable?> TryAcquireAsync(string key, TimeSpan timeout = default)
    {
        var @lock = new RedisDistributedLock(key, Connection.GetDatabase());
        return await @lock.TryAcquireAsync(timeout).ConfigureAwait(false);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Connection.Dispose();
            }
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
