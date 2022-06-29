using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using StackExchange.Redis;

namespace Rebus.RedisExclusiveSagaAccessLock.Tests;

[TestFixture]
public class Tests
{
    [Test]
    public async Task VerifyItWorks()
    {
        var multiplexer = await ConnectionMultiplexer.ConnectAsync("localhost");

        var l = new RedisLock(multiplexer);
        var lock1 = "lock1";
        
        var success = await l.AcquireLockAsync(lock1, CancellationToken.None);
        success.Should().BeTrue();

        var locked = await l.IsLockAcquiredAsync(lock1, CancellationToken.None);
        locked.Should().BeTrue();


        var timer = Stopwatch.StartNew();
        var cancellation = new CancellationTokenSource();
        cancellation.CancelAfter(TimeSpan.FromSeconds(5));
        try
        {
            await l.AcquireLockAsync(lock1, cancellation.Token);
        }
        catch (TaskCanceledException e)
        {
            // ignored
        }

        timer.Elapsed.TotalSeconds.Should().BeGreaterThan(5);

        var unlocked = await l.ReleaseLockAsync(lock1);
        unlocked.Should().BeTrue();

        locked = await l.IsLockAcquiredAsync(lock1, CancellationToken.None);
        locked.Should().BeFalse();
    }
}