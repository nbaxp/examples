using Medallion.Threading.FileSystem;

namespace Wta.Infrastructure.Locking;

public class FileLock : ILock
{
    public async Task<IDisposable?> Acquire(string key, TimeSpan timeout = default)
    {
        var lockFileDirectory = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "lock"));
        var @lock = new FileDistributedLock(lockFileDirectory, key);
        return await @lock.AcquireAsync(timeout).ConfigureAwait(false);
    }

    public async Task<IDisposable?> TryAcquireAsync(string key, TimeSpan timeout = default)
    {
        var lockFileDirectory = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "lock"));
        var @lock = new FileDistributedLock(lockFileDirectory, key);
        return await @lock.TryAcquireAsync(timeout).ConfigureAwait(false);
    }
}
