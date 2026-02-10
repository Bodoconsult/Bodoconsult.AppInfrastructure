More tools for developers
=============

# Overview

> [Basic watchdog implementation IWatchDog / WatchDog as replacement for timers for non-time critical tasks](#iwatchdog--watchdog)

> [BufferPool\<T\> for reusing frequently used tiny to medium size classes](#bufferpoolt) to reduce garbage collection pressure

> [BufferPoolResetable\<T\> for reusing frequently used tiny to medium size classes implementing IResetable](#bufferpoolresetablet) to reduce garbage collection pressure

> [MemoryStreamBufferPool\<T\> for reusing MemoryStream instances](#memorystreambufferpool) to reduce garbage collection pressure

> [ProducerConsumerQueue<T/> implementing the producer-consumer-pattern for one or many producers but only one consumer](#producerconsumerqueue) with running the consumer always on the same thread

> [DataProtectionManager for protecting secrets like credentials](#dataprotectionmanager-for-protecting-secrets-like-credentials)

> [Protecting entities: EntityProtectionService class](#protecting-entities-entityprotectionservice-class)

> [ZIP file handling](#zip-file-handling)

> [Extension methods for basic and enhanced classes](#extension-methods-for-basic-and-enhanced-classes)



# IWatchDog / WatchDog

WatchDog is something similar to a timer. Timers do have the potential issue with multiple running tasks if tasks may run longer than the timer interval. 
So if your timer task will potentially run longer then the timer interval you should think about using IWatchDog implementations.

The runner method is fired and processed until done. Then the watchdog waits for the delay interval until it restarts the runner method.

For time critical tasks WatchDog is not a good solution. If you have to ensure that your task is running i.e. every minute exactly, you better should use a Timer.

The runner method is running always on the same separated background thread.

``` csharp
WatchDogRunnerDelegate runner = Runner;

var w = new WatchDog(runner, delayTime);
w.StartWatchDog();
...

w.StopWatchDog();		
```

``` csharp
/// <summary>
/// Runner method for the watchdog
/// </summary>
private void Runner()
{
    // Do your tasks here
}
```


# BufferPool\<T\>

BufferPool\<T\> is used to handle and reuse multiple instances of potentially small objects of a type to reduce garbage pressure.

Heavily used small objects of a type lead to a relatively high demand for object creation and disposing. In such cases it might be a good idea to reuse the already created instances.

Classes used with BufferPool\<T\> should implement IResetable interface.

``` csharp
// Arrange 

var myPool = new BufferPool<byte[]>(() => new byte[65535]);
myPool.Allocate(1000);

var buffer = myPool.Dequeue();

// Act  
myPool.Enqueue(buffer);

// Assert
Assert.That(myPool.LengthOfQueue, Is.EqualTo(NumberOfItems));
```

# BufferPoolResetable\<T\>

BufferPoolResetable\<T\> is intended as buffer pool for reusable classes implementing IResetable interface.

Heavily used small objects of a type lead to a relatively high demand for object creation and disposing. In such cases it might be a good idea to reuse the already created instances.

Classes used with BufferPoolResetable\<T\> have to implement IResetable interface.

``` csharp
[Test]
public void Enqueue_ValidSetup_InstanceEnqueued()
{
    // Arrange 
    var myPool = new BufferPoolResetable<TestDataResetable>(() => new TestDataResetable());
    myPool.Allocate(1000);

    var buffer = myPool.Dequeue();

    // Act  
    myPool.Enqueue(buffer);

    // Assert
    Assert.That(myPool.LengthOfQueue, Is.EqualTo(NumberOfItems));
}
```

# MemoryStreamBufferPool

A special buffer pool to reuse MemoryStream instances.

``` csharp
[Test]
public void Enqueue_ValidSetup_InstanceEnqueued()
{
    // Arrange 
    var myPool = new MemoryStreamBufferPool();
    myPool.Allocate(1000);

    var buffer = myPool.Dequeue();

    // Act  
    myPool.Enqueue(buffer);

    // Assert
    Assert.That(myPool.LengthOfQueue, Is.EqualTo(NumberOfItems));
}
```

# ProducerConsumerQueue<T/>

ProducerConsumerQueue<T/> implements the producer-consumer-pattern for one or many producers but only one consumer with running the consumer always on the same thread. This may be helpful if the consumer is maybe a database or a file which should not be accessed by multiple threads at the same time.

``` csharp
const string s1 = "Blubb";
const string s2 = "Blabb";
const string s3 = "Blobb";

var pc = new ProducerConsumerQueue<string>();
pc.ConsumerTaskDelegate = ConsumerTaskDelegate;
pc.StartConsumer();

// Act  
pc.Enqueue(s1);
pc.Enqueue(s2);
pc.Enqueue(s3);

// Assert
Wait.Until(() => _counter > 0);
Assert.That(_counter, Is.EqualTo(3));
Assert.That(_received.Count, Is.EqualTo(3));
Assert.That(_received.Contains(s1), Is.True);
Assert.That(_received.Contains(s2), Is.True);
Assert.That(_received.Contains(s3), Is.True);

...

private void ConsumerTaskDelegate(string value)
{
    _counter++;
    _received.Add(value);
    _wasFired = true;
}
```

# Data protection

## DataProtectionManager for protecting secrets like credentials

DataProtectionManager implements a file based solution for save storage of sensitive data based on Microsoft.AspNetCore.DataProtection to be used in console apps etc. without the need of more sophisticated tools like Azure Vault etc..

Important: unprotecting data is possible only on the machine the data protection was done.

### Using without DI container

See the following example for how to use DataProtectionManager without DI container:

``` csharp
[Test]
public void Unprotect_ValidSetup_SecretUnprotectedCorrectly()
{
    // Arrange 
    var path = Globals.Instance.AppStartParameter.DataPath;
    var instance = DataProtectionService.CreateInstance(path, AppName);

    var filePath = Path.Combine(path, $"appData.{Extension}");
    if (File.Exists(filePath))
    {
        File.Delete(filePath);
    }

    var dpm = new DataProtectionManager(instance, FileProtectionService, filePath);

    dpm.Protect(Key, Secret);

    // Act  
    var result = dpm.Unprotect(Key);

    // Assert
    Assert.That(result, Is.EqualTo(Secret));
    dpm.Dispose();
}
```

### Using with DI container

See the following example for how to use DataProtectionManager without DI container:

``` csharp
private static SimpleDataProtectionDiContainerServiceProvider CreateProvider()
{
    var destinationFolderPath = TestHelper.TempPath;
    var provider = new SimpleDataProtectionDiContainerServiceProvider(destinationFolderPath);
    return provider;
}

[Test]
public void CreateInstance_DefaultSetup_InstanceCreated()
{
    // Arrange
    var filePath = Path.Combine(TestHelper.TempPath, "blubb.dat");

    var provider = CreateProvider();

    var container = new DiContainer();

    Assert.That(container.ServiceCollection.Count, Is.EqualTo(0));

    provider.AddServices(container);
    container.BuildServiceProvider();

    Assert.That(container.ServiceCollection.Count, Is.Not.EqualTo(0));

    var dpm = container.Get<IDataProtectionManagerFactory>();
    Assert.That(dpm, Is.Not.Null);

    // Act 
    var instance = dpm.CreateInstance(filePath);

    // Assert
    Assert.That(instance, Is.Not.Null);
}
```
There is another implementation NoDataProtectionDiContainerServiceProvider of IDiContainerServiceProvider to load a data protection solution letting the file itself unprotected but not the secrets stored in the file.

Please use SimpleDataProtectionDiContainerServiceProvider normally.

## Protecting entities: EntityProtectionService class

If you want to protect properties of an entity use EntityProtectionService class. 

You have to prepare your entity class with two attributes DataProtectionKey and DataProtectionSecret:

``` csharp
internal class EntityWithUidWithSecrets
{
    [DataProtectionKey]
    public Guid Uid { get; set; }

    [DataProtectionSecret]
    public string Secret { get; set; }

    [DataProtectionSecret]
    public string Secret2 { get; set; }
}
```

The DataProtectionKey attribute has to be set once. 

The following sample shows how to use 

``` csharp
private readonly EntityProtectionService _entityProtectionService;

protected const string AppName = "MyApp";

public EntityProtectionServiceTests()
{
    var path = Globals.Instance.AppStartParameter.DataPath;
    var dataProtectionService = DataProtectionService.CreateInstance(path, AppName);
    _entityProtectionService = new EntityProtectionService(dataProtectionService);
}

[Test]
public void Unprotect_MultipleSecretsWithUid_PropsWithDataProtectionSecretAttributeProtected()
{
    // Arrange 
    const string secret = "Secret";
    const string secret2 = "Secret2";
    var uid = Guid.NewGuid();

    var entity = new EntityWithUidWithSecrets
    {
        Uid = uid,
        Secret = secret,
        Secret2 = secret2
    };

    _entityProtectionService.Protect(entity);

    Assert.That(entity.Uid, Is.EqualTo(uid));
    Assert.That(entity.Secret, Is.Not.EqualTo(secret));
    Assert.That(entity.Secret2, Is.Not.EqualTo(secret2));
    Assert.That(entity.Secret, Is.Not.EqualTo(entity.Secret2));

    // Act  
    _entityProtectionService.Unprotect(entity);

    // Assert
    Assert.That(entity.Uid, Is.EqualTo(uid));
    Assert.That(entity.Secret, Is.EqualTo(secret));
    Assert.That(entity.Secret2, Is.EqualTo(secret2));
}
```

``` csharp


```

# IExceptionReplyBuilder / ExceptionReplyBuilder

ExceptionReplyBuilder delivers a central exception management to be used standalone or in conjunction with business transactions.

ToDo: add more information

# ZIP file handling

The classes ZipHelper, ZipHandler and UnZipHandler simplify the creation and extracting of unprotected ZIP files


# Extension methods for basic and enhanced classes

-   AsyncExtensions

    -   GetAwaiter() for CancellationToken

-   BoolExtensions

    -   AsSqlString()

-   DoubleExtensions

    -   AsSqlString()

-   FiletimeExtensions

    -   ToDateTime()

-   IntExtensions

    -   RomanToArabic()

    -   ArabicToRoman()

-   GuidExtensions

    -   ToPlainString()

-   StringExtensions

    -   ReplaceHexTag()

    -   AsSqlString()

    -   FirstCharToLowerCase()

    -   FirstCharToUpperCase()

    -   LimitToLength()

    -   SecureStringToString()

    -   Repeat()

    -   SpaceCount()

    -   ToGuid()

    -   EscapeForXml()

-   TaskExtensions

    -   Forget()

    -   ForgetAwaited()
