# HTTP Connection Handling Improvements

## Summary

This implementation adds robust HTTP resilience to the eForm SDK using Polly, a .NET resilience and transient-fault-handling library.

## What Was Changed

### 1. Added Polly Dependencies

Added two NuGet packages to `eFormCore/Microting.eForm.csproj`:
- **Polly** (v8.6.4): Core resilience library
- **Polly.Extensions.Http** (v3.0.0): HttpClient integration

### 2. Updated Http.cs with Retry Policies

Modified `eFormCore/Communication/Http.cs` to include:

- **Retry Policy**: Automatically retries transient HTTP failures
  - Handles HTTP 5xx errors (500, 503, etc.)
  - Handles HTTP 408 (Request Timeout)
  - Handles `HttpRequestException` (connection refused, network errors)

- **Exponential Backoff**: Progressive delays between retries
  - 1st retry: 2 seconds
  - 2nd retry: 4 seconds  
  - 3rd retry: 8 seconds

- **Logging**: Debug output for troubleshooting
  - Logs each retry attempt
  - Includes error details and timing

### 3. Updated HTTP Methods

Applied retry policy to all HTTP methods:
- `HttpPost()`
- `HttpPut()`
- `HttpGet()`
- `HttpDelete()`
- `PostProto()` (for protobuf requests)

### 4. Created Test Project

New test project: `eFormSDK.Http.Tests`
- 6 comprehensive tests for HTTP resilience
- Uses WireMock for HTTP mocking
- Tests all critical scenarios:
  - Transient failures with eventual success
  - Persistent failures after max retries
  - Connection refused handling
  - Multiple HTTP verbs (GET, POST, DELETE)

### 5. Test Execution Script

Created `http-tests.sh` for easy test execution.

## How It Works

### Before (Without Polly)

```csharp
var response = await httpClient.PostAsync(url, content);
response.EnsureSuccessStatusCode(); // Throws on first error
```

If the server returns 500 or the connection fails, the request fails immediately.

### After (With Polly)

```csharp
var response = await _retryPolicy.ExecuteAsync(async () =>
{
    var httpClient = new HttpClient();
    return await httpClient.PostAsync(url, content);
});
response.EnsureSuccessStatusCode(); // Only throws after all retries
```

If the server returns 500:
1. First attempt fails
2. Wait 2 seconds, retry
3. If still fails, wait 4 seconds, retry
4. If still fails, wait 8 seconds, retry
5. If still fails, throw exception

## Error Handling Strategy

### Transient Errors (Retried)
- HTTP 500 Internal Server Error
- HTTP 503 Service Unavailable
- HTTP 408 Request Timeout
- Connection refused
- Network timeouts

### Non-Transient Errors (Not Retried)
- HTTP 400 Bad Request
- HTTP 401 Unauthorized
- HTTP 403 Forbidden
- HTTP 404 Not Found

### Final Error Handling

After all retries are exhausted:
- Public methods (Post, Status, Delete, etc.) catch exceptions
- Return error XML/JSON responses
- Maintains backward compatibility with existing error handling

## Benefits

1. **Improved Reliability**: Automatically handles temporary network issues
2. **Better User Experience**: Reduces failures from transient errors
3. **Minimal Code Changes**: Centralized in Http.cs, no changes needed in calling code
4. **Configurable**: Easy to adjust retry count and delays if needed
5. **Observable**: Logging helps troubleshoot issues
6. **Well-Tested**: Comprehensive test coverage validates behavior

## Testing

All tests pass successfully:

```bash
$ ./http-tests.sh
Test Run Successful.
Total tests: 6
     Passed: 6
```

## Future Enhancements

Potential improvements for consideration:

### 1. Circuit Breaker Pattern
Prevent repeated calls to failing services:
```csharp
var circuitBreaker = Policy
    .Handle<HttpRequestException>()
    .CircuitBreakerAsync(
        exceptionsAllowedBeforeBreaking: 5,
        durationOfBreak: TimeSpan.FromSeconds(30)
    );
```

### 2. Configurable Policies
Allow configuration via app settings:
```json
{
  "Polly": {
    "RetryCount": 3,
    "BaseDelay": 2,
    "EnableCircuitBreaker": true
  }
}
```

### 3. Retry Metrics
Track and report retry statistics:
- Number of retries per request
- Success rate after retries
- Average retry duration

### 4. Policy Composition
Combine multiple policies:
```csharp
var policy = Policy.WrapAsync(timeoutPolicy, retryPolicy, circuitBreakerPolicy);
```

### 5. Conditional Retries
Different policies for different endpoints:
- Critical endpoints: More retries
- Non-critical endpoints: Fewer retries
- Read operations: More aggressive retries
- Write operations: Conservative retries

## References

- [Polly Documentation](https://github.com/App-vNext/Polly)
- [Microsoft: Implement HTTP call retries with exponential backoff with Polly](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-http-call-retries-exponential-backoff-polly)
- [WireMock.Net Documentation](https://github.com/WireMock-Net/WireMock.Net)
