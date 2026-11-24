# HTTP Resilience Tests

This test project validates the HTTP resilience and retry logic implemented in `Http.cs` using the Polly library.

## Overview

The eForm SDK now includes automatic retry logic for handling transient HTTP failures:
- Connection refused errors
- HTTP 500 (Internal Server Error)
- HTTP 503 (Service Unavailable)

## Retry Configuration

- **Retry Count**: 3 retries (4 total attempts including initial request)
- **Backoff Strategy**: Exponential backoff with base 10 (`Http.DefaultRetryDelay`)
  - 1st retry: 10 seconds delay
  - 2nd retry: 100 seconds delay
  - 3rd retry: 1000 seconds delay

The tests do not wait this schedule. An `internal` constructor (exposed via `InternalsVisibleTo`)
takes the retry delay; the tests pass one that returns zero and records the retry attempts it was
asked for. `DefaultRetryDelay_Should_Be_10s_100s_1000s` pins the production schedule.

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

7. **DefaultRetryDelay_Should_Be_10s_100s_1000s**
   - Pins the production retry delay schedule

## Implementation Details

### Technology Stack

- **NUnit**: Testing framework
- **WireMock.Net**: HTTP mocking for test scenarios
- **Polly**: Resilience and transient-fault-handling library
- **Polly.Extensions.Http**: HttpClient integration

### How It Works

The tests use WireMock to create a mock HTTP server that simulates various failure scenarios. The tests verify that:

1. The Http class properly retries failed requests
2. The retry delay is requested for the expected retry attempts, and the production schedule is unchanged
3. After all retries, appropriate error responses are returned
4. Success is achieved after transient failures clear

### Logging

In Debug builds you'll see the output below (`WriteDebugConsoleLogEntry` is compiled out in Release,
which `./http-tests.sh` uses):
- Each retry attempt number
- Delay between retries (0 s in these tests)
- Error messages for each failed attempt

Example output (`dotnet test -c Debug`):
```
dbug: Polly Retry[0]
      POST http://localhost:36333/integration/create - Retry 1/3 after 0s delay. Total operation time: 0.4s. Error: HTTP request failed with status code: InternalServerError
dbug: Polly Retry[0]
      POST http://localhost:36333/integration/create - Retry 2/3 after 0s delay. Total operation time: 0.4s. Error: HTTP request failed with status code: InternalServerError
```

## Next Steps

Future enhancements could include:
- Circuit breaker pattern for preventing cascading failures
- Configurable retry policies (count, delays, conditions)
- Metrics and monitoring for retry attempts
- Additional test scenarios for other HTTP status codes
