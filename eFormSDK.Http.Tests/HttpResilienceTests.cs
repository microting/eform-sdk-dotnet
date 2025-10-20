/*
The MIT License (MIT)

Copyright (c) 2007 - 2025 Microting A/S

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace eFormSDK.Http.Tests;

/// <summary>
/// Tests for HTTP resilience using Polly retry policies.
/// These tests verify that Http.cs correctly handles:
/// - Connection refused errors
/// - HTTP 500 (Internal Server Error)
/// - HTTP 503 (Service Unavailable)
/// - Retry behavior with exponential backoff
/// - Eventual success after transient failures
/// </summary>
[TestFixture]
public class HttpResilienceTests
{
    private WireMockServer _mockServer;
    private string _serverUrl = string.Empty;
    private const string TestToken = "12345678901234567890123456789012";

    [SetUp]
    public void Setup()
    {
        // Start WireMock server for mocking HTTP endpoints
        _mockServer = WireMockServer.Start();
        _serverUrl = _mockServer.Url;
    }

    [TearDown]
    public void TearDown()
    {
        _mockServer?.Stop();
        _mockServer?.Dispose();
    }

    [Test]
    public async Task Post_Should_Retry_On_Http500_And_Eventually_Succeed()
    {
        // Arrange
        var siteId = "123";
        var testData = "<TestData>Test</TestData>";

        // Setup mock to return 500 twice using InScenario
        _mockServer
            .Given(Request.Create().WithPath("/integration/create").UsingPost())
            .InScenario("Post Retry Test")
            .WillSetStateTo("Attempt 2")
            .RespondWith(Response.Create().WithStatusCode(500));

        _mockServer
            .Given(Request.Create().WithPath("/integration/create").UsingPost())
            .InScenario("Post Retry Test")
            .WhenStateIs("Attempt 2")
            .WillSetStateTo("Attempt 3")
            .RespondWith(Response.Create().WithStatusCode(500));

        _mockServer
            .Given(Request.Create().WithPath("/integration/create").UsingPost())
            .InScenario("Post Retry Test")
            .WhenStateIs("Attempt 3")
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBody("<Response><Value>Success</Value></Response>"));

        var http = new Microting.eForm.Communication.Http(TestToken, _serverUrl, _serverUrl, "org123", _serverUrl, _serverUrl, _serverUrl);

        // Act
        var result = await http.Post(testData, siteId);

        // Assert
        Assert.That(result, Does.Contain("Success"), "Should eventually succeed and return success response");
        Assert.That(_mockServer.LogEntries.Count, Is.EqualTo(3), "Should make 3 attempts");
    }

    [Test]
    public async Task Post_Should_Retry_On_Http503_And_Eventually_Succeed()
    {
        // Arrange
        var siteId = "123";
        var testData = "<TestData>Test</TestData>";

        // Setup mock to return 503 once, then succeed
        _mockServer
            .Given(Request.Create().WithPath("/integration/create").UsingPost())
            .InScenario("503 Retry Test")
            .WillSetStateTo("Success")
            .RespondWith(Response.Create().WithStatusCode(503));

        _mockServer
            .Given(Request.Create().WithPath("/integration/create").UsingPost())
            .InScenario("503 Retry Test")
            .WhenStateIs("Success")
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBody("<Response><Value>Success</Value></Response>"));

        var http = new Microting.eForm.Communication.Http(TestToken, _serverUrl, _serverUrl, "org123", _serverUrl, _serverUrl, _serverUrl);

        // Act
        var result = await http.Post(testData, siteId);

        // Assert
        Assert.That(result, Does.Contain("Success"), "Should eventually succeed and return success response");
        Assert.That(_mockServer.LogEntries.Count, Is.EqualTo(2), "Should make 2 attempts");
    }

    [Test]
    public async Task Post_Should_Fail_After_Max_Retries_On_Persistent_Http500()
    {
        // Arrange
        var siteId = "123";
        var testData = "<TestData>Test</TestData>";

        // Always return 500
        _mockServer
            .Given(Request.Create().WithPath("/integration/create").UsingPost())
            .RespondWith(Response.Create().WithStatusCode(500));

        var http = new Microting.eForm.Communication.Http(TestToken, _serverUrl, _serverUrl, "org123", _serverUrl, _serverUrl, _serverUrl);

        // Act
        // Post method catches exceptions and returns error XML
        var result = await http.Post(testData, siteId);

        // Assert
        Assert.That(result, Does.Contain("converterError"), "Should return error response after all retries exhausted");
        Assert.That(result, Does.Contain("500"), "Should include status code 500 in error message");
        Assert.That(_mockServer.LogEntries.Count, Is.EqualTo(4), "Should make 4 attempts (1 initial + 3 retries)");
    }

    [Test]
    public async Task Status_Should_Retry_On_Http500_And_Eventually_Succeed()
    {
        // Arrange
        var elementId = "elem123";
        var siteId = "123";

        // Setup mock to return 500 once, then succeed
        _mockServer
            .Given(Request.Create().WithPath("/integration/status").UsingGet())
            .InScenario("Status Retry")
            .WillSetStateTo("Success")
            .RespondWith(Response.Create().WithStatusCode(500));

        _mockServer
            .Given(Request.Create().WithPath("/integration/status").UsingGet())
            .InScenario("Status Retry")
            .WhenStateIs("Success")
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBody("<Response><Status>OK</Status></Response>"));

        var http = new Microting.eForm.Communication.Http(TestToken, _serverUrl, _serverUrl, "org123", _serverUrl, _serverUrl, _serverUrl);

        // Act
        var result = await http.Status(elementId, siteId);

        // Assert
        Assert.That(result, Does.Contain("OK"), "Should eventually succeed and return OK status");
        Assert.That(_mockServer.LogEntries.Count, Is.EqualTo(2), "Should make 2 attempts");
    }

    [Test]
    public async Task Delete_Should_Retry_On_Http503_And_Eventually_Succeed()
    {
        // Arrange
        var elementId = "elem123";
        var siteId = "123";

        // Setup mock to return 503 twice, then succeed
        _mockServer
            .Given(Request.Create().WithPath("/integration/delete").UsingDelete())
            .InScenario("Delete Retry")
            .WillSetStateTo("Attempt 2")
            .RespondWith(Response.Create().WithStatusCode(503));

        _mockServer
            .Given(Request.Create().WithPath("/integration/delete").UsingDelete())
            .InScenario("Delete Retry")
            .WhenStateIs("Attempt 2")
            .WillSetStateTo("Success")
            .RespondWith(Response.Create().WithStatusCode(503));

        _mockServer
            .Given(Request.Create().WithPath("/integration/delete").UsingDelete())
            .InScenario("Delete Retry")
            .WhenStateIs("Success")
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBody("<?xml version=\"1.0\" encoding=\"UTF-8\"?><Response><Value type=\"success\">success</Value><Unit fetched_at=\"\" id=\"\"/></Response>"));

        var http = new Microting.eForm.Communication.Http(TestToken, _serverUrl, _serverUrl, "org123", _serverUrl, _serverUrl, _serverUrl);

        // Act
        var result = await http.Delete(elementId, siteId);

        // Assert
        Assert.That(result, Does.Contain("success"), "Should eventually succeed and return success response");
        Assert.That(_mockServer.LogEntries.Count, Is.EqualTo(3), "Should make 3 attempts");
    }

    [Test]
    public async Task Post_Should_Handle_Connection_Refused_With_Retries()
    {
        // Arrange
        var siteId = "123";
        var testData = "<TestData>Test</TestData>";
        
        // Use invalid port to simulate connection refused
        var invalidUrl = "http://localhost:65432"; // Port unlikely to be open
        var http = new Microting.eForm.Communication.Http(TestToken, invalidUrl, invalidUrl, "org123", invalidUrl, invalidUrl, invalidUrl);

        // Act
        // Post method catches exceptions and returns error XML
        var result = await http.Post(testData, siteId);

        // Assert
        Assert.That(result, Does.Contain("converterError"), "Should return error response after all retries exhausted");
        Assert.That(result, Does.Contain("Connection refused"), "Should include connection refused error message");
    }
}
