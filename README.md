# ThrottleDemo

A simple C# demo demonstrating rate limiting/throttling using the token bucket algorithm.

## Overview

This demo implements a `Throttler` class that limits operations to a configurable rate (e.g., 5 operations per second). It uses a semaphore-based token bucket pattern where tokens refill automatically at a fixed interval.

## Features

- Token bucket rate limiting
- Configurable max operations and time window
- Automatic token refill via timer
- IDisposable pattern for cleanup

## Usage

```bash
dotnet run
```

## Output

The demo processes 20 requests with a limit of 5 operations per second:
```
[14:30:00.001] Request 1 processed
[14:30:00.002] Request 2 processed
...
[14:30:00.005] Request 5 processed
[14:30:00.105] Request 6 throttled
[14:30:1.001] Request 6 processed
...
```

## Docker

### Build the image

```bash
docker build -t throttledemo .
```

### Run the container

```bash
docker run throttledemo
```

## Requirements

- .NET 10.0 (or Docker for containerized execution)
