using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace SeleniumSandbox.Tests.VisualTesting;

public class VisualTestCaseDiscoverer : IXunitTestCaseDiscoverer
{
    private readonly IMessageSink _diagnosticMessageSink;

    public VisualTestCaseDiscoverer(IMessageSink diagnosticMessageSink)
    {
        _diagnosticMessageSink = diagnosticMessageSink;
    }

    public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
    {
        var defaultMethodDisplay = discoveryOptions.MethodDisplayOrDefault();
        var defaultMethodDisplayOptions = discoveryOptions.MethodDisplayOptionsOrDefault();

        yield return new VisualTestCase(_diagnosticMessageSink, defaultMethodDisplay, defaultMethodDisplayOptions, testMethod);
    }
}
