using OpenQA.Selenium;
using SeleniumSandbox.Tests.Core;

namespace SeleniumSandbox.Tests.Pages;

public class ExampleHomePage : BasePage
{
    private static readonly By _moreInformationLinkLocator = By.CssSelector("a");

    public ExampleHomePage(IWebDriver driver) : base(driver) { }

    public void Navigate() => _driver.Navigate().GoToUrl("https://example.com");

    public string Title => _driver.Title;

    public string HeadingText => WaitFor(By.TagName("h1")).Text;

    public string ParagraphText => WaitFor(By.TagName("p")).Text;

    public By MoreInformationLinkLocator => _moreInformationLinkLocator;

    public string MoreInformationHref => WaitFor(_moreInformationLinkLocator)
        .GetAttribute("href") ?? string.Empty;

    public string ClickMoreInformationLink()
    {
        var link = WaitHelper.WaitForClickable(_driver, _moreInformationLinkLocator);
        var href = link.GetAttribute("href") ?? string.Empty;
        link.Click();
        return href;
    }
}