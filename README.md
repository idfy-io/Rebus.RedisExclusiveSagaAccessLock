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


## Testing
- `docker-compose up`  (With docker-compose.yaml located in root of repo)
- Run tests located in `Rebus.RedisExclusiveSagaAccessLock.Tests` using your favourite tool