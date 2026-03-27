using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var throttle = new Throttler(maxOperations: 5, perSeconds: 1);

        for (int i = 1; i <= 20; i++)
        {
            if (throttle.TryAcquire())
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Request {i} processed");
            }
            else
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Request {i} throttled");
            }
            await Task.Delay(100);
        }
    }
}

class Throttler : IDisposable
{
    private readonly SemaphoreSlim _semaphore;
    private readonly Timer _refillTimer;
    private int _availableTokens;
    private readonly int _maxTokens;
    private readonly int _refillAmount;
    private bool _disposed;

    public Throttler(int maxOperations, double perSeconds)
    {
        _maxTokens = maxOperations;
        _availableTokens = maxOperations;
        _refillAmount = maxOperations;
        _semaphore = new SemaphoreSlim(maxOperations, maxOperations);
        
        int intervalMs = (int)(perSeconds * 1000);
        _refillTimer = new Timer(_ => RefillTokens(), null, intervalMs, intervalMs);
    }

    public bool TryAcquire()
    {
        return _semaphore.Wait(0);
    }

    private void RefillTokens()
    {
        int previousCount = _semaphore.Release(_refillAmount);
        Interlocked.Exchange(ref _availableTokens, _maxTokens);
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _refillTimer.Dispose();
        _semaphore.Dispose();
        GC.SuppressFinalize(this);
    }
}
