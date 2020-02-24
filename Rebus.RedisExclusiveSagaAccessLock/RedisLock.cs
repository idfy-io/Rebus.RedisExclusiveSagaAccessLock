using System;
using System.Threading;
using System.Threading.Tasks;
using Rebus.Sagas.Exclusive;
using StackExchange.Redis;

namespace Rebus.RedisExclusiveSagaAccessLock
{
    public class RedisLock : IExclusiveSagaAccessLock
    {
        private readonly ConnectionMultiplexer _multiplexer;
        private const string DummyValue = "dummy";
        public RedisLock(ConnectionMultiplexer multiplexer)
        {
            _multiplexer = multiplexer ?? throw new ArgumentNullException(nameof(multiplexer));
        }

        public async Task<bool> AquireLockAsync(string key, CancellationToken cancellationToken)
        {
            var db = _multiplexer.GetDatabase();
            while(!await db.LockTakeAsync(key, DummyValue, TimeSpan.FromMinutes(1),CommandFlags.PreferMaster))
            {
                await Task.Delay(20,cancellationToken);
            }
            return true;
        }

        public async Task<bool> ReleaseLockAsync(string key)
        {
            var db = _multiplexer.GetDatabase();
            return await db.LockReleaseAsync(key, DummyValue, CommandFlags.PreferMaster);
        }
    }
}