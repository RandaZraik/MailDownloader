using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MailDownloader.Domain.Threading
{
    public class KeyedLock<TKey>
    {
        private readonly Dictionary<TKey, (SemaphoreSlim, int)> _perKey;
        private readonly Stack<SemaphoreSlim> _pool;
        private readonly int _poolCapacity;

        public KeyedLock(IEqualityComparer<TKey> keyComparer = null, int poolCapacity = 10)
        {
            _perKey = new Dictionary<TKey, (SemaphoreSlim, int)>(keyComparer);
            _pool = new Stack<SemaphoreSlim>(poolCapacity);
            _poolCapacity = poolCapacity;
        }

        public Task WaitAsync(TKey key, CancellationToken cancellationToken = default) =>
            GetSemaphore(key).WaitAsync(cancellationToken);

        public async Task<bool> WaitAsync(TKey key, int millisecondsTimeout,
            CancellationToken cancellationToken = default)
        {
            var semaphore = GetSemaphore(key);
            var entered = await semaphore.WaitAsync(millisecondsTimeout, cancellationToken).ConfigureAwait(false);
            if (!entered)
                ReleaseSemaphore(key, entered: false);
            return entered;
        }

        public void Wait(TKey key, CancellationToken cancellationToken = default)
            => GetSemaphore(key).Wait(cancellationToken);

        public bool Wait(TKey key, int millisecondsTimeout,
            CancellationToken cancellationToken = default)
        {
            var semaphore = GetSemaphore(key);
            var entered = semaphore.Wait(millisecondsTimeout, cancellationToken);
            if (!entered)
                ReleaseSemaphore(key, entered: false);
            return entered;
        }

        public void Release(TKey key) => ReleaseSemaphore(key, entered: true);

        private SemaphoreSlim GetSemaphore(TKey key)
        {
            SemaphoreSlim semaphore;
            lock (_perKey)
            {
                if (_perKey.TryGetValue(key, out var entry))
                {
                    int counter;
                    (semaphore, counter) = entry;
                    _perKey[key] = (semaphore, ++counter);
                }
                else
                {
                    lock (_pool) semaphore = _pool.Count > 0 ? _pool.Pop() : null;
                    if (semaphore == null) semaphore = new SemaphoreSlim(1, 1);
                    _perKey[key] = (semaphore, 1);
                }
            }
            return semaphore;
        }

        private void ReleaseSemaphore(TKey key, bool entered)
        {
            SemaphoreSlim semaphore; int counter;
            lock (_perKey)
            {
                if (_perKey.TryGetValue(key, out var entry))
                {
                    (semaphore, counter) = entry;
                    counter--;
                    if (counter == 0)
                        _perKey.Remove(key);
                    else
                        _perKey[key] = (semaphore, counter);
                }
                else
                {
                    throw new InvalidOperationException("Key not found.");
                }
            }
            if (entered)
                semaphore.Release();
            if (counter == 0)
                lock (_pool) if (_pool.Count < _poolCapacity) _pool.Push(semaphore);
        }
    }
}