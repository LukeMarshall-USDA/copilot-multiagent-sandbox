using OpenQA.Selenium;
using SeleniumSandbox.Tests.Core;

namespace SeleniumSandbox.Tests.Tests;

// DEMO-PREP FIXTURE
// ----------------------------------------------------------------------------
// Intentional anti-pattern: this test is written procedurally with direct
// _driver.FindElement calls, inline selectors, and no Page Object. It
// duplicates the logic that already exists cleanly in ExampleHomePage.cs.
//
// Used in the 4/24 All Hands Tech Spotlight demo (Part 1, Beat 1.3) to show
// Copilot Chat refactoring this to the Page Object pattern that the rest of
// this repo already follows (see Pages/ExampleHomePage.cs).
// ----------------------------------------------------------------------------
public class LegacyProceduralTests : IDisposable
{
    private readonly IWebDriver _driver;

    public LegacyProceduralTests()
    {
        _driver = DriverFactory.CreateChromeDriver();
    }

    [Fact]
    [Trait("Category", "Legacy")]
    [Trait("Mode", "Headless")]
    public void Navigate_And_VerifyAllFields_Procedural()
    {
        _driver.Navigate().GoToUrl("https://example.com");

        // Title check — direct driver call.
        var title = _driver.Title;
        Assert.Equal("Example Domain", title);

        // Heading check — inline selector, no page object.
        var h1 = _driver.FindElement(By.TagName("h1"));
        Assert.Equal("Example Domain", h1.Text);

        // Paragraph check — another inline lookup.
        var paragraph = _driver.FindElement(By.TagName("p"));
        Assert.Contains("This domain is for use in documentation", paragraph.Text);

        // Link check — raw CSS selector, manual href extraction.
        var link = _driver.FindElement(By.CssSelector("a"));
        var href = link.GetAttribute("href") ?? string.Empty;
        Assert.Contains("iana.org", href);
    }

    [Fact]
    [Trait("Category", "Legacy")]
    [Trait("Mode", "Headless")]
    public void Click_MoreInformationLink_Procedural()
    {
        _driver.Navigate().GoToUrl("https://example.com");
        var originalHandle = _driver.CurrentWindowHandle;

        // Find, read href, click — all inline.
        var link = _driver.FindElement(By.CssSelector("a"));
        var href = link.GetAttribute("href") ?? string.Empty;
        Assert.Contains("iana.org", href);
        link.Click();

        // Manual window-switch loop — no helper, no timeout pattern.
        var handles = _driver.WindowHandles;
        if (handles.Count > 1)
        {
            foreach (var handle in handles)
            {
                if (handle != originalHandle)
                {
                    _driver.SwitchTo().Window(handle);
                    break;
                }
            }
        }

        Assert.Contains("iana.org", _driver.Url);
        _driver.Close();
        _driver.SwitchTo().Window(originalHandle);
    }

    public void Dispose() => _driver.Quit();
}
