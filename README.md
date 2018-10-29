# ConfigureAwait
Performance and usage of Async-Await with ConfigureAwait(false) and multiple ways to use await.

RunAsync method simulating a long running async task, this one last about 1 second (1000 ms).
```
public async Task<string> RunAsync(int id)
{
    for (var i = 0; i < 10; i++)
    {
        await Task.Delay(100).ConfigureAwait(false); //Perform async task, don't run task in UI SynchronousContext
    }

    return $"Done RunAsync {id}";
}
```

Which code runs the best are these the same?
```
protected async Task<string> RunAsyncTwice1()
{
    sw.Restart();
    var result1 = await RunAsync(1);
    var result2 = await RunAsync(2);
    var finalResult = result1 + "/n" + result2;
    sw.Stop();
    return "RunAsyncTwice1: " + sw.Elapsed;
}

protected async Task<string> RunAsyncTwice2()
{
    sw.Restart();
    var result1 = RunAsync(1);
    var result2 = RunAsync(2);
    var finalResult = await result1 + "/n" + await result2;
    sw.Stop();
    return "RunAsyncTwice2: " + sw.Elapsed;
}
```

What about these?
```
protected async Task<string> RunAsyncTwice3()
{
    sw.Restart();
    var finalResult = await RunAsync(1) + "/n" + await RunAsync(2);
    sw.Stop();
    return "RunAsyncTwice3: " + sw.Elapsed;
}

protected async Task<string> RunAsyncTwice4()
{
    sw.Restart();
    var finalResult = await Task.WhenAll(RunAsync(3), RunAsync(4));
    sw.Stop();
    return "RunAsyncTwice4: " + sw.Elapsed;
}
```
