# HTTP Debugging and Resilience

This document describes the debugging features and resilience mechanisms in the eForm SDK HTTP communication layer.

## Enhanced Debug Logging

The HTTP client now provides detailed debugging information to help trace performance issues and long-running operations.

### Debug Log Format

All HTTP operations now include:

- **Precise timestamps**: UTC timestamps with millisecond precision (`yyyy-MM-dd HH:mm:ss`)
- **HTTP method and URL**: Clear identification of the operation being performed
- **Retry information**: Shows retry attempts with context (e.g., "Retry 2/3")
- **Cumulative timing**: Total time spent in the operation across all retries
- **Success/failure details**: Final status codes and total elapsed time

### Example Debug Output

```
[DBG] HttpPost: called at 2025-10-26 11:40:44 UTC for url http://localhost:39329/integration/create?token=***&siteId=123&sdkVer=1.0.0.0
[DBG] HttpPost: Executing HTTP POST request to http://localhost:39329/integration/create?token=***&siteId=123&sdkVer=1.0.0.0 (attempt within retry policy)
[DBG] Polly Retry: POST http://localhost:39329/integration/create?token=***&siteId=123&sdkVer=1.0.0.0 - Retry 1/3 after 10s delay. Total operation time: 0.1s. Error: HTTP request failed with status code: ServiceUnavailable
[DBG] Polly Retry Progress: Long retry delay (1.7 minutes) - this may take a while. Operation: POST http://localhost:39329/integration/create?token=***&siteId=123&sdkVer=1.0.0.0
[DBG] HttpPost: Executing HTTP POST request to http://localhost:39329/integration/create?token=***&siteId=123&sdkVer=1.0.0.0 (attempt within retry policy)
[DBG] HttpPost: Successfully completed at 2025-10-26 11:40:54 UTC - total time 00:00:10.135 (Status: OK)
```

## Retry Policy Configuration

The SDK uses Polly for HTTP resilience with the following configuration:

- **Retry count**: 3 attempts (plus the initial request = 4 total)
- **Backoff strategy**: Exponential - 10s, 100s, 1000s (16.7 minutes)
- **Handled errors**:
  - HTTP 5xx server errors
  - HTTP 408 request timeout
  - Connection refused exceptions
  - Other `HttpRequestException` errors

### Timing Breakdown

For a request that exhausts all retries:
- Initial attempt: ~0.1s
- Wait + Retry 1: 10s + ~0.1s
- Wait + Retry 2: 100s + ~0.1s  
- Wait + Retry 3: 1000s + ~0.1s
- **Total maximum time**: ~18.5 minutes

## Long Wait Progress Indication

For retry delays longer than 60 seconds, the system provides progress messages:

```
[DBG] Polly Retry Progress: Long retry delay (16.7 minutes) - this may take a while. Operation: POST http://localhost:*/integration/create
```

This helps identify when operations are in extended retry periods rather than hung or frozen.

## HTTP Method Coverage

Enhanced debugging is available for all HTTP methods:

- `HttpPost` - Standard POST requests
- `HttpGet` - GET requests  
- `HttpPut` - PUT requests
- `HttpDelete` - DELETE requests
- `PostProto` - Protobuf POST requests (both new and legacy APIs)

## Troubleshooting

### Long Test Execution Times

If HTTP tests are taking 18+ minutes:

1. **Check retry logs**: Look for "Retry 3/3 after 1000s" messages
2. **Identify failing endpoints**: Note which URLs are consistently failing
3. **Consider test environment**: Ensure mock servers are configured correctly
4. **Monitor progress**: Watch for "Long retry delay" messages to confirm the operation is still active

### Performance Analysis

Use the debug logs to:

- **Measure actual vs expected timing**: Compare "total time" with expected duration
- **Identify retry patterns**: See which operations fail and how often
- **Track cumulative impact**: Understand how retries affect overall performance
- **Diagnose network issues**: Correlation between connection errors and retry behavior

## Configuration Notes

The exponential backoff timing (`Math.Pow(10, retryAttempt)`) creates very long delays for the final retry. This is intentional for production resilience but may need adjustment for testing scenarios.

For testing environments, consider:
- Reducing retry counts
- Implementing shorter backoff periods
- Using mock servers with controlled failure scenarios