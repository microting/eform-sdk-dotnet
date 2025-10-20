# HTTP Resilience Tests

This test project validates the HTTP resilience and retry logic implemented in `Http.cs` using the Polly library.

## Overview

The eForm SDK now includes automatic retry logic for handling transient HTTP failures:
- Connection refused errors
- HTTP 500 (Internal Server Error)
- HTTP 503 (Service Unavailable)

## Retry Configuration

- **Retry Count**: 3 retries (4 total attempts including initial request)
- **Backoff Strategy**: Exponential backoff with base 2
  - 1st retry: ~2 seconds delay
  - 2nd retry: ~4 seconds delay
  - 3rd retry: ~8 seconds delay

## Running Tests

Execute the HTTP resilience tests:

```bash
# From repository root
./http-tests.sh

# Or directly with dotnet
dotnet test eFormSDK.Http.Tests/eFormSDK.Http.Tests.csproj --configuration Release
```

## Test Coverage

The test suite includes:

1. **Post_Should_Retry_On_Http500_And_Eventually_Succeed**
   - Simulates transient HTTP 500 errors
   - Verifies successful retry and eventual success

2. **Post_Should_Retry_On_Http503_And_Eventually_Succeed**
   - Simulates HTTP 503 Service Unavailable
   - Validates retry behavior for service temporarily unavailable scenarios

3. **Post_Should_Fail_After_Max_Retries_On_Persistent_Http500**
   - Tests persistent failure scenario
   - Ensures proper error response after all retries exhausted

4. **Status_Should_Retry_On_Http500_And_Eventually_Succeed**
   - Tests GET request retry logic
   - Validates Status endpoint resilience

5. **Delete_Should_Retry_On_Http503_And_Eventually_Succeed**
   - Tests DELETE request retry logic
   - Verifies proper handling of 503 errors

6. **Post_Should_Handle_Connection_Refused_With_Retries**
   - Simulates network connectivity issues
   - Validates retry behavior for connection refused errors

## Implementation Details

### Technology Stack

- **NUnit 4.4.0**: Testing framework
- **WireMock.Net 1.6.11**: HTTP mocking for test scenarios
- **Polly 8.6.4**: Resilience and transient-fault-handling library
- **Polly.Extensions.Http 3.0.0**: HttpClient integration

### How It Works

The tests use WireMock to create a mock HTTP server that simulates various failure scenarios. The tests verify that:

1. The Http class properly retries failed requests
2. Exponential backoff delays are applied correctly
3. After all retries, appropriate error responses are returned
4. Success is achieved after transient failures clear

### Logging

During test execution, you'll see debug output showing:
- Each retry attempt number
- Delay between retries
- Error messages for each failed attempt
- Final success or failure status

Example output:
```
[DBG] HttpPost: called at 10/20/2025 06:23:02 for url http://localhost:41225/integration/create
[DBG] Polly Retry: Retry 1 after 2s. HTTP request failed with status code: InternalServerError
[DBG] Polly Retry: Retry 2 after 4s. HTTP request failed with status code: InternalServerError
[DBG] HttpPost: Finished at 10/20/2025 06:23:08 - took -00:00:06.0087572
```

## Next Steps

Future enhancements could include:
- Circuit breaker pattern for preventing cascading failures
- Configurable retry policies (count, delays, conditions)
- Metrics and monitoring for retry attempts
- Additional test scenarios for other HTTP status codes
