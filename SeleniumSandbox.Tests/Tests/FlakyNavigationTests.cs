using OpenQA.Selenium;
using SeleniumSandbox.Tests.Core;

namespace SeleniumSandbox.Tests.Tests;

// DEMO-PREP FIXTURE
// ----------------------------------------------------------------------------
// Intentional anti-pattern: this test uses a fixed Thread.Sleep to "wait" for
// the page to load. It passes most of the time on a fast connection but flakes
// when the page takes longer than the sleep duration.
//
// Used in the 4/24 All Hands Tech Spotlight demo (Part 1, Beat 1.2) to show
// Copilot Chat diagnosing the flakiness and proposing the explicit-wait fix
// that the rest of this repo already uses (see WaitHelper.cs / BasePage.cs).
// ----------------------------------------------------------------------------
public class FlakyNavigationTests : IDisposable
{
    private readonly IWebDriver _driver;

    public FlakyNavigationTests()
    {
        _driver = DriverFactory.CreateChromeDriver();
    }

    [Fact]
    [Trait("Category", "Flaky")]
    [Trait("Mode", "Headless")]
    public void Navigate_And_ReadHeading_Flaky()
    {
        _driver.Navigate().GoToUrl("https://example.com");

        // "It usually loads within 300ms, right?"
        Thread.Sleep(300);

        var heading = _driver.FindElement(By.TagName("h1"));
        Assert.Equal("Example Domain", heading.Text);
    }

    [Fact]
    [Trait("Category", "Flaky")]
    [Trait("Mode", "Headless")]
    public void Navigate_And_ReadParagraph_Flaky()
    {
        _driver.Navigate().GoToUrl("https://example.com");

        // Implicit wait — works on local, flakes in CI.
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);

        var paragraph = _driver.FindElement(By.TagName("p"));
        Assert.Contains("This domain is for use in documentation", paragraph.Text);
    }

    public void Dispose() => _driver.Quit();
}
