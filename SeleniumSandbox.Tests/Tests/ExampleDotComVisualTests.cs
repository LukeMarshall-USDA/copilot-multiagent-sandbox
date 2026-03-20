// Visual Tests: These tests open a visible Chrome browser window
using OpenQA.Selenium;
using SeleniumSandbox.Tests.Core;
using SeleniumSandbox.Tests.Pages;
using SeleniumSandbox.Tests.VisualTesting;
using Xunit.Abstractions;

namespace SeleniumSandbox.Tests.Tests;

public class ExampleDotComVisualTests : VisualTestBase
{
    private readonly ExampleHomePage _page;

    public ExampleDotComVisualTests(ITestOutputHelper output) : base(output)
    {
        _page = new ExampleHomePage(Driver);
    }

    [VisualFact]
    public void Navigate_DisplaysCorrectTitle()
    {
        _page.Navigate();

        Assert.Equal("Example Domain", _page.Title);
    }

    [VisualFact]
    public void Navigate_PageContainsHeading()
    {
        _page.Navigate();

        Assert.Equal("Example Domain", _page.HeadingText);
    }

    [VisualFact]
    public void Navigate_PageContainsParagraph()
    {
        _page.Navigate();

        Assert.Contains("This domain is for use in documentation", _page.ParagraphText);
    }

    [VisualFact]
    public void Navigate_PageContainsLink()
    {
        _page.Navigate();

        Assert.Contains("iana.org", _page.MoreInformationHref);
    }

    [VisualFact]
    public void Navigate_ClicksLink_OpensIanaReservedDomains()
    {
        _page.Navigate();
        var originalHandle = Driver.CurrentWindowHandle;

        var href = _page.ClickMoreInformationLink();
        Assert.Contains("iana.org", href);

        var waited = WaitHelper.WaitForWindowCount(Driver, 2, out var newWindowHandle);
        if (!waited || string.IsNullOrEmpty(newWindowHandle))
        {
            var newTab = Driver.SwitchTo().NewWindow(WindowType.Tab);
            newWindowHandle = newTab.CurrentWindowHandle;
            Driver.Navigate().GoToUrl(href);
        }
        else
        {
            Driver.SwitchTo().Window(newWindowHandle);
        }

        Assert.NotEqual(originalHandle, Driver.CurrentWindowHandle);
        Assert.Contains("iana.org", Driver.Url);

        var reservedPage = new IanaReservedDomainsPage(Driver);
        Assert.Contains("Example Domains", reservedPage.HeadingText);

        Driver.Close();
        Driver.SwitchTo().Window(originalHandle);
    }
}
