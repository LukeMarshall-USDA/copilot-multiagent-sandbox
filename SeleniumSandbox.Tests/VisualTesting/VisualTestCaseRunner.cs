using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace SeleniumSandbox.Tests.VisualTesting;

public class VisualTestCase : XunitTestCase
{
    public VisualTestCase()
    {
    }

    public VisualTestCase(IMessageSink diagnosticMessageSink, TestMethodDisplay defaultMethodDisplay, TestMethodDisplayOptions defaultMethodDisplayOptions, ITestMethod testMethod)
        : base(diagnosticMessageSink, defaultMethodDisplay, defaultMethodDisplayOptions, testMethod)
    {
        Traits ??= new Dictionary<string, List<string>>();
        Traits[VisualFactAttribute.ModeTraitName] = new List<string> { VisualFactAttribute.VisualModeValue };
    }

    public override Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink, IMessageBus messageBus, object?[] constructorArguments, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
    {
        var methodArguments = TestMethodArguments ?? Array.Empty<object?>();

        return new VisualTestCaseRunner(this, DisplayName, SkipReason, constructorArguments, methodArguments, messageBus, aggregator, cancellationTokenSource).RunAsync();
    }
}

public class VisualTestCaseRunner : XunitTestCaseRunner
{
    public VisualTestCaseRunner(IXunitTestCase testCase, string displayName, string? skipReason, object?[] constructorArguments, object?[] testMethodArguments, IMessageBus messageBus, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
        : base(testCase, displayName, skipReason, constructorArguments, testMethodArguments, messageBus, aggregator, cancellationTokenSource)
    {
    }

    protected override Task<RunSummary> RunTestAsync()
    {
        var test = new XunitTest(TestCase, DisplayName);
        var invokerAggregator = new ExceptionAggregator(Aggregator);

        return new VisualTestRunner(
            test,
            MessageBus,
            TestClass,
            ConstructorArguments,
            TestMethod,
            TestMethodArguments,
            SkipReason,
            BeforeAfterAttributes,
            invokerAggregator,
            CancellationTokenSource).RunAsync();
    }
}

public class VisualTestRunner : XunitTestRunner
{
    public VisualTestRunner(ITest test, IMessageBus messageBus, Type? testClass, object?[] constructorArguments, MethodInfo? testMethod, object?[]? testMethodArguments, string? skipReason, IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
        : base(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments, skipReason, beforeAfterAttributes, aggregator, cancellationTokenSource)
    {
    }

    protected override Task<decimal> InvokeTestMethodAsync(ExceptionAggregator aggregator)
    {
        var invoker = new VisualTestInvoker(
            Test,
            MessageBus,
            TestClass,
            ConstructorArguments,
            TestMethod,
            TestMethodArguments,
            BeforeAfterAttributes,
            aggregator,
            CancellationTokenSource);

        return invoker.RunAsync();
    }
}

public class VisualTestInvoker : XunitTestInvoker
{
    public VisualTestInvoker(ITest test, IMessageBus messageBus, Type? testClass, object?[] constructorArguments, MethodInfo? testMethod, object?[]? testMethodArguments, IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
        : base(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments, beforeAfterAttributes, aggregator, cancellationTokenSource)
    {
    }

    protected override async Task<decimal> InvokeTestMethodAsync(object testClassInstance)
    {
        var result = await base.InvokeTestMethodAsync(testClassInstance);

        if (Aggregator.HasExceptions)
        {
            await CaptureScreenshotAsync(testClassInstance);
        }

        return result;
    }

    private Task CaptureScreenshotAsync(object testClassInstance)
    {
        if (testClassInstance is not VisualTestBase visualTest)
        {
            return Task.CompletedTask;
        }

        if (visualTest.Driver is not ITakesScreenshot screenshotDriver)
        {
            return Task.CompletedTask;
        }

        try
        {
            var now = DateTime.Now;
            var testClassName = TestClass?.Name ?? "UnknownClass";
            var testMethodName = TestMethod?.Name ?? "UnknownMethod";
            var directory = Path.Combine("artifacts", "screenshots", "visual", now.ToString("yyyyMMdd"), testClassName);
            Directory.CreateDirectory(directory);

            var fileName = $"{testMethodName}__{now:HHmmssfff}.png";
            var fullPath = Path.Combine(directory, fileName);

            var screenshot = screenshotDriver.GetScreenshot();
            screenshot.SaveAsFile(fullPath);

            var relativePath = Path.GetRelativePath(Directory.GetCurrentDirectory(), fullPath);
            visualTest.Output?.WriteLine($"Screenshot saved: {relativePath}");
        }
        catch (Exception ex)
        {
            visualTest.Output?.WriteLine($"Failed to capture screenshot: {ex.Message}");
        }

        return Task.CompletedTask;
    }
}
