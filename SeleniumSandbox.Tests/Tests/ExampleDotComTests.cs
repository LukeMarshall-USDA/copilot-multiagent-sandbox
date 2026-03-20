using OpenQA.Selenium;
using SeleniumSandbox.Tests.Core;
using SeleniumSandbox.Tests.Pages;

namespace SeleniumSandbox.Tests.Tests;

public class ExampleDotComTests : IDisposable
{
    private readonly IWebDriver _driver;
    private readonly ExampleHomePage _page;

    public ExampleDotComTests()
    {
        _driver = DriverFactory.CreateChromeDriver();
        _page = new ExampleHomePage(_driver);
    }

    [Fact]
    [Trait("Category", "Smoke")]
    [Trait("Mode", "Headless")]
    public void Navigate_DisplaysCorrectTitle()
    {
        _page.Navigate();

        Assert.Equal("Example Domain", _page.Title);
    }

    [Fact]
    [Trait("Category", "Smoke")]
    [Trait("Mode", "Headless")]
    public void Navigate_DisplaysHeading()
    {
        _page.Navigate();

        Assert.Equal("Example Domain", _page.HeadingText);
    }

    [Fact]
    [Trait("Category", "Smoke")]
    [Trait("Mode", "Headless")]
    public void Navigate_DisplaysParagraph()
    {
        _page.Navigate();

        Assert.Contains("This domain is for use in documentation", _page.ParagraphText);
    }

    [Fact]
    [Trait("Category", "Smoke")]
    [Trait("Mode", "Headless")]
    public void Navigate_DisplaysLinkHref()
    {
        _page.Navigate();

        Assert.Contains("iana.org", _page.MoreInformationHref);
    }

    [Fact]
    [Trait("Category", "Smoke")]
    [Trait("Mode", "Headless")]
    public void Clicking_MoreInformation_OpensIanaReservedDomains()
    {
        _page.Navigate();
        var originalHandle = _driver.CurrentWindowHandle;

        var href = _page.ClickMoreInformationLink();
        Assert.Contains("iana.org", href);

        var waited = WaitHelper.WaitForWindowCount(_driver, 2, out var newWindowHandle);
        if (!waited || string.IsNullOrEmpty(newWindowHandle))
        {
            // Headless Chrome can block popups; open a new tab manually if the click did not.
            var newTab = _driver.SwitchTo().NewWindow(WindowType.Tab);
            newWindowHandle = newTab.CurrentWindowHandle;
            _driver.Navigate().GoToUrl(href);
        }
        else
        {
            _driver.SwitchTo().Window(newWindowHandle);
        }

        var reservedPage = new IanaReservedDomainsPage(_driver);
        Assert.Contains("Example Domains", reservedPage.HeadingText);
        Assert.Contains("iana.org", _driver.Url);

        _driver.Close();
        _driver.SwitchTo().Window(originalHandle);
    }

    public void Dispose() => _driver.Quit();
}