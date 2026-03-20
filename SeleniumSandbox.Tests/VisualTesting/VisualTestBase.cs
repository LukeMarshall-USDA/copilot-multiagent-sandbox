using System;
using OpenQA.Selenium;
using SeleniumSandbox.Tests.Core;
using Xunit.Abstractions;

namespace SeleniumSandbox.Tests.VisualTesting;

public abstract class VisualTestBase : IDisposable
{
    protected VisualTestBase(ITestOutputHelper output)
    {
        Output = output ?? throw new ArgumentNullException(nameof(output));
        Driver = DriverFactory.CreateChromeDriver(headless: false) ?? throw new InvalidOperationException("Failed to create WebDriver.");
    }

    protected internal IWebDriver Driver { get; }

    protected internal ITestOutputHelper Output { get; }

    public virtual void Dispose()
    {
        try
        {
            Driver.Quit();
            Driver.Dispose();
        }
        catch
        {
            // Best effort cleanup; do not mask test results
        }
    }
}
