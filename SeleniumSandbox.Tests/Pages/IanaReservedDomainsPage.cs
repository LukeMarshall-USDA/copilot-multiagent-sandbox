using OpenQA.Selenium;
using SeleniumSandbox.Tests.Core;

namespace SeleniumSandbox.Tests.Pages;

public class IanaReservedDomainsPage : BasePage
{
    public IanaReservedDomainsPage(IWebDriver driver) : base(driver) { }

    public string Title => _driver.Title;

    public string HeadingText => WaitFor(By.TagName("h1")).Text;

    public string ParagraphText => WaitFor(By.TagName("p")).Text;
}
