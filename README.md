# Rebus.RedisExclusiveSagaAccessLock
Distributed sagalocking with Redis

## Usage
```csharp
//Configure lock
var redisLocker = new RedisLock(ConnectionMultiplexer.Connect("localhost"));

//Configure Rebus to use the distributed lock
Configure.With(SomeContainerAdapter)
.Sagas(s => s.EnforceExclusiveAccess(redisLocker))
.Start();
```
